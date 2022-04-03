using UnityEngine;

public static class Kbd {
    public static bool CancelPressed(float deadTimeStart) => alive(deadTimeStart) &&  Input.GetKeyDown(KeyCode.Escape);

    public static bool NextPressed(float deadTimeStart) {
        if (!alive(deadTimeStart)) {
            return false;
        }

        return check( deadTimeStart, KeyCode.KeypadEnter, KeyCode.Return, KeyCode.Space);
    }

    public static bool UpPressed(float time) => checkCmd(time, KeyCode.UpArrow, KeyCode.W, KeyCode.Keypad8, KeyCode.Alpha8);
    public static bool DownPressed(float time) => checkCmd(time, KeyCode.DownArrow, KeyCode.S, KeyCode.Keypad2, KeyCode.Alpha2);
    public static bool LeftPressed(float time) => checkCmd(time, KeyCode.LeftArrow, KeyCode.A, KeyCode.Keypad4, KeyCode.Alpha4);
    public static bool RightPressed(float time) => checkCmd(time, KeyCode.RightArrow, KeyCode.D, KeyCode.Keypad6, KeyCode.Alpha6);

    public static bool NorthPressed(float time) => checkDir(time, KeyCode.W, KeyCode.Keypad8, KeyCode.Alpha8);
    public static bool SouthPressed(float time) => checkDir(time, KeyCode.S, KeyCode.Keypad2, KeyCode.Alpha2);
    public static bool NorthEastPressed(float time) => checkDir(time, KeyCode.E, KeyCode.Keypad9, KeyCode.Alpha9);
    public static bool NorthWestPressed(float time) => checkDir(time, KeyCode.Q, KeyCode.Keypad7, KeyCode.Alpha7);
    public static bool SouthEastPressed(float time) => checkDir(time, KeyCode.D, KeyCode.Keypad3, KeyCode.Alpha3);
    public static bool SouthWestPressed(float time) => checkDir(time, KeyCode.A, KeyCode.Keypad1, KeyCode.Alpha1);

    // Private

    static bool alive(float startTime) {
        return Time.time > startTime + .05f;//Ludum.Dare.Data.KeyDebounceTime;
    }

    static bool checkCmd(float startTime, params KeyCode[] keys) {
        if (!Ludum.Dare.Commands.HasFocus) {
            return false;
        }
        return check(startTime, keys);
    }

    static bool checkDir(float startTime, params KeyCode[] keys) {
        if (Ludum.Dare.Commands.HasFocus) {
            return false;
        }
        return check(startTime, keys);
    }

    static bool check(float startTime, params KeyCode[] keys) {
        if (!alive(startTime)) {
            return false;
        }
        foreach (var key in keys) {
            if (Input.GetKeyDown(key)) {
                return true;
            }
        }
        return false;
    }
}