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
                setupCommands();
                Ludum.Dare.Dream.Hide();
                Ludum.Dare.Computer.SetText("You are inside the Hab.");
                break;
            case State.Map:
                Ludum.Dare.Commands.LoseFocus();
                Ludum.Dare.Hab.Hide();
                Ludum.Dare.Map.Show();
                setupCommands();
                Ludum.Dare.Dream.Hide();
                Ludum.Dare.Computer.SetText("You are in your rover, outside the Hab.");
                Ludum.Dare.Rover.OnEnterMapState();
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

    void setupCommands() {
        if (!Application.isPlaying) {
            return;
        }
        Ludum.Dare.Commands.RebuildActions();
    }
}
