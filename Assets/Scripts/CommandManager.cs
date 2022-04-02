using System;
using UnityEngine;

public class CommandManager: MonoBehaviour {
    [SerializeField] RowSelector _rowSelector;
    [SerializeField] CommandEngine _habActions;
    [SerializeField] CommandEngine _mapActions;

    CommandEngine engine {
        get {
            switch (Ludum.Dare.State.Current) {
                case StateManager.State.Hab: return _habActions;
                case StateManager.State.Map: return _mapActions;
                default:
                    Debug.LogError($"Should not be asking for commands in state {Ludum.Dare.State.Current}");
                    return null;
            }
        }
    }

    public void RebuildActions() {
        string msg;
        Command[] commands;

        engine.GetCommands(out msg, out commands);
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
