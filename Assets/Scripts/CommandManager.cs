using System;
using UnityEngine;

public class CommandManager: MonoBehaviour {
    [SerializeField] RowSelector _rowSelector;
    [SerializeField] CommandEngine _habActions;
    [SerializeField] CommandEngine _mapActions;
    [SerializeField] GameObject _commandsAnchor;

    public CommandEngine Engine {
        get {
            switch (Ludum.Dare.State.Current) {
                case State.Hab: return _habActions;
                case State.Map: return _mapActions;
                default:
                    return null;
            }
        }
    }

    public bool HasFocus => _commandsAnchor.gameObject.activeSelf;

    public void DoInteract(HexType hex) {
        GainFocus();
        RebuildActions($"Interact_{hex}");
    }

    public void GainFocus() {
        _rowSelector.GainFocus();
        _commandsAnchor.gameObject.SetActive(true);
    }

    public void LoseFocus() {
        _rowSelector.LoseFocus();
        _commandsAnchor.gameObject.SetActive(false);
    }

    public void RebuildActions(string resetPosition = null) {
        string msg;
        Command[] commands;
        var currentEngine = Engine;

        if (resetPosition != null) {
            currentEngine.ResetPosition(resetPosition);
        }

        Engine.GetCommands(out msg, out commands);
        if (currentEngine != Engine || Engine == null) {
            return;
        }
        Ludum.Dare.Computer.SetText(msg);

        _rowSelector.UnregisterAllRows();
        foreach (var cmd in commands) {
            var newRow = Ludum.Dare.Factory.CreateCommandRow(cmd.Description, () => cmd.Execute());
            _rowSelector.RegisterRow(newRow);
        }

        _rowSelector.GainFocus();
    }

    public void AddAction(string action, Action callback) { Debug.LogError("TODO: Implement"); }
}
