using TMPro;
using UnityEngine;

public class TemperatureManager: MonoBehaviour {
    [SerializeField] TemperatureBar[] hours;
    [SerializeField] TextMeshProUGUI topline;

    public int Day { get; private set; }

    public int Hour {
        get => _hour;
        set {
            _hour = value;
            for (var i=0; i<hours.Length; i++) {
                hours[i].HourHasPassed = (i < _hour);
            }
        }
    }

    int _hour;

    public void OnNextDay() {
        Day++;
        Hour = 0;
        rebuildToplineText();
    }

    // Private

    void rebuildToplineText() {
        topline.text = $"Day {Day}\t\tHours until sleep: {24 - _hour}";
    }
}
