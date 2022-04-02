using UnityEngine;

public class StateManager: MonoBehaviour {
    public enum State {
        Uninitialized, Hab, Map, Dream, Dead
    }

    public State Current {
        get => _state;
        set {
            _state = value;
            switch (value) {
            case State.Hab:
                Ludum.Dare.Hab.Show();
                Ludum.Dare.Map.Hide();
                setupCommands();
                break;
            case State.Map:
                Ludum.Dare.Hab.Hide();
                Ludum.Dare.Map.Show();
                setupCommands();
                break;
            case State.Dream:
                Ludum.Dare.Hab.Hide();
                Ludum.Dare.Map.Hide();
                break;
            case State.Dead:
                Ludum.Dare.Hab.Hide();
                Ludum.Dare.Map.Hide();
                break;
            default:
                Debug.LogError($"Cannot understand {value}");
                break;
            }
        }
    }

    State _state;

    // Private

    void setupCommands() {
        if (!Application.isPlaying) {
            return;
        }
        Ludum.Dare.Commands.RebuildActions();
    }

    void Start() {
        // TODO: Start game here and now
        Current = State.Hab;
    }
}
