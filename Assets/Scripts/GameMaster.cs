using UnityEngine;

public class GameMaster: MonoBehaviour {
    public void DoEndDay() {
        Ludum.Dare.Temperature.OnNextDay();
        Ludum.Dare.State.Current = State.Dream;
    }

    public void DoStartNewGame() {
        Ludum.Dare.Resources.OnStartNewGame();
        Ludum.Dare.State.Current = State.Dream;
    }

    // Private

    void Start() => DoStartNewGame();
}
