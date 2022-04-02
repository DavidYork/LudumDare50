using UnityEngine;
using UnityEngine.UI;

public class TemperatureBar: MonoBehaviour {
    public Image Icon;

    public int Cold {
        get => _cold;
        set {
            _cold = value;
            // TODO: Set icon
        }
    }

    public bool HourHasPassed {
        get => _hourHasPassed;
        set {
            _hourHasPassed = value;
            // TODO: Set icon
        }
    }

    bool _hourHasPassed;
    int _cold;
}
