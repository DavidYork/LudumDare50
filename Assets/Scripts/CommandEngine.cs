using UnityEngine;
using System;
using System.Collections.Generic;
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
        _story.BindExternalFunction("DoGoBackToHab", doGoBackToHab);
        _story.BindExternalFunction("DoGoOutside", doGoOutside);
        _story.BindExternalFunction("DoGoToBed", doGoToBed);
        _story.BindExternalFunction("DoReturnToRover", doReturnToRover);

        _story.BindExternalFunction("GetCost", (string costName) => GetCost(Enum.Parse<Cost>(costName)));
        _story.BindExternalFunction("GetEvent", (string eventName) => GetEvent(Enum.Parse<Event>(eventName)));
        _story.BindExternalFunction("TryPurchase", (string costName) => tryPurchase(Enum.Parse<Cost>(costName)));

        _story.BindExternalFunction("SetEvent", (string eventName, bool val) =>
            SetEvent(Enum.Parse<Event>(eventName), val));

    }


    // Ink interop
    void doGoOutside() => Ludum.Dare.State.Current = State.Map;
    void doGoBackToHab() => Ludum.Dare.State.Current = State.Hab;
    void doGoToBed() => Ludum.Dare.GM.DoEndDay();
    void doReturnToRover() => Ludum.Dare.Commands.LoseFocus();

    bool tryPurchase(Cost item) {
        var cost = Ludum.Dare.Data.GetCost(item);
        var res = Ludum.Dare.Resources;

        if (res.Scrap.Amount < cost.Scrap || res.Fungus.Amount < cost.Fungus ||
            res.Energy.Amount < cost.Energy || res.Energy.Max < cost.Batteries)
        {

            return false;
        }
        res.Scrap.Amount -= cost.Scrap;
        res.Energy.Amount -= cost.Energy;
        res.Energy.Max -= cost.Batteries;
        res.Fungus.Amount -= cost.Fungus;
        return true;
    }

    string GetCost(Cost cost) => Ludum.Dare.Data.GetCost(cost).ToString();
    bool GetEvent(Event evt) => Ludum.Dare.Events.GetEvent(evt);
    void SetEvent(Event evt, bool val) => Ludum.Dare.Events.SetEvent(evt, val);
}