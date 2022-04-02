using TMPro;
using UnityEngine;

public class TemperatureManager: MonoBehaviour {
    [SerializeField] TemperatureBar[] hours;
    [SerializeField] TextMeshProUGUI topline;

    public int Day {
        get => _day;
        set {
            _day = value;
            rebuildToplineText();
        }
    }

    public int Hour {
        get => _hour;
        set {
            _hour = value;
            for (var i=0; i<hours.Length; i++) {
                hours[i].HourHasPassed = (i < _hour);
            }
        }
    }

    int _day;
    int _hour;

    // Private

    void rebuildToplineText() {
        topline.text = $"Day {_day}\t\tHours until sleep: {24 - _hour}";
    }
}
