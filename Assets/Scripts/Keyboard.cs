using UnityEngine;

public static class Kbd {
    public static bool CancelPressed(float deadTimeStart) => alive(deadTimeStart) &&  Input.GetKeyDown(KeyCode.Escape);

    public static bool NextPressed(float deadTimeStart) {
        if (!alive(deadTimeStart)) {
            return false;
        }

        return Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return)
            || Input.GetKeyDown(KeyCode.Space);
    }

    public static bool UpPressed(float deadTimeStart) {
        return check(deadTimeStart, KeyCode.UpArrow) || check(deadTimeStart, KeyCode.W);
    }

    public static bool DownPressed(float deadTimeStart) {
        return check(deadTimeStart, KeyCode.DownArrow) || check(deadTimeStart, KeyCode.S);
    }

    public static bool LeftPressed(float deadTimeStart) {
        return check(deadTimeStart, KeyCode.LeftArrow) || check(deadTimeStart, KeyCode.A);
    }

    public static bool RightPressed(float deadTimeStart) {
        return check(deadTimeStart, KeyCode.RightArrow) || check(deadTimeStart, KeyCode.D);
    }

    public static bool NorthPressed(float time) => check(time, KeyCode.W, KeyCode.Keypad8, KeyCode.Alpha8);
    public static bool SouthPressed(float time) => check(time, KeyCode.S, KeyCode.Keypad2, KeyCode.Alpha2);
    public static bool NorthEastPressed(float time) => check(time, KeyCode.E, KeyCode.Keypad9, KeyCode.Alpha9);
    public static bool NorthWestPressed(float time) => check(time, KeyCode.Q, KeyCode.Keypad7, KeyCode.Alpha7);
    public static bool SouthEastPressed(float time) => check(time, KeyCode.D, KeyCode.Keypad3, KeyCode.Alpha3);
    public static bool SouthWestPressed(float time) => check(time, KeyCode.A, KeyCode.Keypad1, KeyCode.Alpha1);

    // Private

    static bool alive(float startTime) {
        return Time.time > startTime + .05f;
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