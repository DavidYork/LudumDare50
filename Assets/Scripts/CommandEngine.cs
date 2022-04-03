using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Text;
using Ink.Runtime;

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

public struct Command {
    public string Description => _choice.text.Sanitize();
    public int ChoiceIndex => _choice.index;

    Choice _choice;
    CommandEngine _engine;

    public Command(CommandEngine engine, Choice choice) {
        _choice = choice;
        _engine = engine;
    }

    public void Execute() => _engine.ExecuteCommand(this);
}

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

public class CommandEngine: MonoBehaviour {
	[SerializeField] TextAsset _inkJSONAsset;

    Story _story;

    public void ExecuteCommand(Command command) {
        _story.ChooseChoiceIndex(command.ChoiceIndex);
        Ludum.Dare.Commands.RebuildActions();
    }


    public void GetCommands(out string text, out Command[] commands) {
        var sb = new StringBuilder();
        while (_story.canContinue) {
            var storyBit = _story.Continue();
            sb.Append(storyBit.Sanitize());
        }
        text = sb.ToString();

        commands = new Command[_story.currentChoices.Count];
        for (var idx=0; idx<commands.Length; idx++) {
            commands[idx] = new Command(this, _story.currentChoices[idx]);
        }
    }

    public void ResetPosition(string position) => _story.ChoosePathString(position);

    // Private
    void Awake() {
		_story = new Story (_inkJSONAsset.text);
        _story.BindExternalFunction("AdvanceTime", (float hours) => doAdvanceTime(hours));
        _story.BindExternalFunction("DoGoBackToHab", doGoBackToHab);
        _story.BindExternalFunction("DoGoOutside", doGoOutside);
        _story.BindExternalFunction("DoGoToBed", doGoToBed);
        _story.BindExternalFunction("GetTodayIsSafe", () => Ludum.Dare.Temperature.TodayIsSafe);
        _story.BindExternalFunction("IsBedtime", () => getIsBedtime());
        _story.BindExternalFunction("DoReturnToRover", doReturnToRover);
        _story.BindExternalFunction("GainResource", (string resource, int amount) =>
            gainResource(resource, amount));
        _story.BindExternalFunction("GetToWork", () => Ludum.Dare.Building.GetToWork());

        _story.BindExternalFunction("HasBuildProject", () => Ludum.Dare.Building.IsBuilding);
        _story.BindExternalFunction("CanPurchase", (string costName) => canPurchase(Enum.Parse<Cost>(costName)));
        _story.BindExternalFunction("GetCost", (string costName) => GetCost(Enum.Parse<Cost>(costName)));
        _story.BindExternalFunction("GetEvent", (string eventName) => GetEvent(Enum.Parse<Event>(eventName)));
        _story.BindExternalFunction("DoPurchase", (string costName) => doPurchase(Enum.Parse<Cost>(costName)));

        _story.BindExternalFunction("SetEvent", (string eventName, bool val) =>
            SetEvent(Enum.Parse<Event>(eventName), val));

        _story.BindExternalFunction("GetDeathText", () => getDeathText());
        _story.BindExternalFunction("DoRestartGame",() => {
            Debug.Log("Reloading scene");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        });
    }


    // Ink interop
    void doAdvanceTime(float hours) => Ludum.Dare.Temperature.Hour += hours;
    void doGoOutside() => Ludum.Dare.State.Current = State.Map;
    void doGoBackToHab() => Ludum.Dare.State.Current = State.Hab;
    void doGoToBed() => Ludum.Dare.GM.DoGoToBed();
    bool getIsBedtime() => Ludum.Dare.Temperature.IsBedtime;
    void doReturnToRover() => Ludum.Dare.Commands.LoseFocus();

    string getDeathText() {
        var temp = Ludum.Dare.Temperature;
        var text = $"It was inevitable. You froze to death {temp.Day} days and {temp.Hour} hours after crash landing.";
        return text;
    }

    void gainResource(string resource, int amount) {
        switch (resource) {
        case "Battery": Ludum.Dare.Resources.Energy.Max += amount; break;
        case "Energy": Ludum.Dare.Resources.Energy.Amount += amount; break;
        case "EnergyToHeat": Ludum.Dare.Resources.EnergyToHeat.Amount += amount; break;
        case "Heat": Ludum.Dare.Resources.Heat.Amount += amount; break;
        case "Fungus": Ludum.Dare.Resources.Fungus.Amount += amount; break;
        case "Scrap": Ludum.Dare.Resources.Scrap.Amount += amount; break;
        default: Debug.LogError($"Cannot understand {resource}"); break;
        }
    }

    bool canPurchase(Cost item) => Ludum.Dare.Building.CanAfford(item) && !Ludum.Dare.Building.IsBuilding;
    void doPurchase(Cost item) => Ludum.Dare.Building.StartBuilding(item);

    string GetCost(Cost cost) => Ludum.Dare.Data.GetCost(cost).ToString();
    bool GetEvent(Event evt) => Ludum.Dare.Events.GetEvent(evt);
    void SetEvent(Event evt, bool val) => Ludum.Dare.Events.SetEvent(evt, val);
}