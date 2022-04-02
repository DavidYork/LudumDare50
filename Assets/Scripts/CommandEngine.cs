using UnityEngine;
using System;
using System.Collections.Generic;
using System.Text;
using Ink.Runtime;

static class Extensions {
	public static TEnum ToEnum<TEnum>(this InkList src) where TEnum: struct, System.Enum {
		var values = src.Values;
		var enumerator = values.GetEnumerator();
		enumerator.MoveNext();
		var val = enumerator.Current;

		TEnum enumVal = (TEnum)Enum.ToObject(typeof(TEnum) , val - 1);
		return enumVal;
	}

	public static void SetVariableToListItem<TEnum>(this Story story, string name, TEnum newValue)
		where TEnum: struct, Enum
	{
		var fullName = $"{newValue.GetType()}";
		var enumName = fullName.Substring(fullName.LastIndexOf('+') + 1);
		var newList = new Ink.Runtime.InkList(enumName, story);
		newList.AddItem($"{newValue}");
		story.variablesState[name] = newList;
	}
}

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

public struct Command {
    public string Description => _choice.text;
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
    Dictionary<Event, bool> _events;

    public void ExecuteCommand(Command command) {
        _story.ChooseChoiceIndex(command.ChoiceIndex);
        Ludum.Dare.Commands.RebuildActions();
    }

    public bool GetEvent(Event evt) {
        bool val = false;
        _events.TryGetValue(evt, out val);
        return val;
    }

    public void SetEvent(Event evt, bool val) {
        _events[evt] = val;
    }

    public void GetCommands(out string text, out Command[] commands) {
        // TODO: Jump directly to the commands section of the story
        var sb = new StringBuilder();
        while (_story.canContinue) {
            sb.Append(_story.Continue());
            sb.Append('\n');
        }
        text = sb.ToString();

        commands = new Command[_story.currentChoices.Count];
        for (var idx=0; idx<commands.Length; idx++) {
            commands[idx] = new Command(this, _story.currentChoices[idx]);
        }
    }

    // Private
    void Awake() {
        _events = new Dictionary<Event, bool>();

		_story = new Story (_inkJSONAsset.text);
        _story.BindExternalFunction("DoGoOutside", doGoOutside);
        _story.BindExternalFunction("DoGoToBed", doGoToBed);
    }

    // Ink interop
    void doGoOutside() => Ludum.Dare.State.Current = State.Map;
    void doGoToBed() => Debug.LogWarning("TODO: Implement going to bed. IRONIC!");
}