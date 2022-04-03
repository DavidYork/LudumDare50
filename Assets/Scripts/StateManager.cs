using UnityEngine;

public class StateManager: MonoBehaviour {
    State _state;

    public State Current {
        get => _state;
        set {
            _state = value;
            switch (value) {
            case State.Hab:
                Ludum.Dare.Commands.GainFocus();
                Ludum.Dare.Hab.Show();
                Ludum.Dare.Map.Hide();
                setupCommands(true);
                Ludum.Dare.Dream.Hide();
                break;
            case State.Map:
                Ludum.Dare.Hab.Hide();
                Ludum.Dare.Map.Show();
                Ludum.Dare.Dream.Hide();
                Ludum.Dare.Rover.OnEnterMapState();
                setupCommands(true);
                Ludum.Dare.Commands.LoseFocus();
                break;
            case State.Dream:
                Ludum.Dare.Commands.LoseFocus();
                Ludum.Dare.Hab.Hide();
                Ludum.Dare.Map.Hide();
                Ludum.Dare.Dream.DoNextDream();
                break;
            case State.Dead:
                Ludum.Dare.Commands.LoseFocus();
                Ludum.Dare.Hab.Hide();
                Ludum.Dare.Map.Hide();
                Ludum.Dare.Dream.Hide();
                break;
            default:
                Debug.LogError($"Cannot understand {value}");
                break;
            }
        }
    }

    // Private

    void setupCommands(bool resetPosition) {
        if (!Application.isPlaying) {
            return;
        }
        Ludum.Dare.Commands.RebuildActions("Commands");
    }
}
