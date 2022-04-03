using UnityEngine;

public class GameMaster: MonoBehaviour {
    public void DoGoToBed() {
        Ludum.Dare.Temperature.GoToSleep();
    }

    public void DoEndDay() {
        Ludum.Dare.State.Current = State.Dream;
    }

    public void DoStartDay() {
        Ludum.Dare.Temperature.OnNextDay();
        Ludum.Dare.State.Current = State.Hab;
    }

    public void DoStartNewGame() {
        Ludum.Dare.Resources.OnStartNewGame();
        Ludum.Dare.State.Current = State.Dream;
    }

    public void DoEndGame() {
        Ludum.Dare.GameIsOver = true;
        Ludum.Dare.Commands.RebuildActions("Gameover");
        Ludum.Dare.Commands.GainFocus();
    }

    // Private

    void Start() => DoStartNewGame();
}
