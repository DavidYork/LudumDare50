using TMPro;
using UnityEngine;

public class TemperatureManager: MonoBehaviour {
    [SerializeField] TemperatureBar[] hours;
    [SerializeField] Sprite[] barImages;
    [SerializeField] TextMeshProUGUI topline;
    [SerializeField] TextMeshProUGUI rightLine;

    int[] temperatures;
    bool _asleep;
    float _sleepTimestamp;
    float _hour;

    public int Day { get; private set; }
    public bool IsBedtime => (Hour >= 16);
    public string TimeNow => timeAsString(_hour);

    public int CurrentColdDamage => (_hour >= 24) ? 0 : hours[(int)_hour].ColdDamage;

    public float Hour {
        get => _hour;
        set {
            var dead = false;
            var oldHour = (int)_hour;
            _hour = value;
            for (var i=0; i<hours.Length; i++) {
                var hourBar = hours[i];
                hourBar.HourHasPassed = i < (int)(_hour);
                hourBar.IsNow = (i == (int)_hour);
            }
            rebuildToplineText();
            if (oldHour != (int)_hour) {
                Ludum.Dare.Heat.ProcessHour(out dead);
                if (dead) {
                    Debug.Log("Player died");
                    Ludum.Dare.GM.DoEndGame();
                }
            }

            if (IsBedtime && !_asleep && !dead) {
                Ludum.Dare.Commands.RebuildActions("Sleep");
                Ludum.Dare.Commands.GainFocus();
            }
        }
    }

    public bool TodayIsSafe {
        get {
            foreach (var temp in hours) {
                if (temp.ColdDamage > 0) {
                    return false;
                }
            }
            return true;
        }
    }

    public void GoToSleep() {
        _sleepTimestamp = Time.time;
        _asleep = true;
        Ludum.Dare.Commands.LoseFocus();
    }

    public void OnNextDay() {
        Day++;
        Hour = 0;

        var today = UnityEngine.Random.Range(0, 3) - 7;
        for (var i=0; i<hours.Length; i++) {
            var temp = (2*i / 7) + (Day / 3) + UnityEngine.Random.Range(0, 5) + today;
            hours[i].Cold = Mathf.Min(Day * 4, temp);
        }

        rebuildToplineText();
    }

    public void OnInstallInsulation(int amount) {
        foreach (var hour in hours) {
            hour.Cold -= amount;
        }
    }

    public Sprite IconForTemp(int temp) {
        temp = Mathf.Min(temp, barImages.Length - 1);
        return barImages[temp];
    }

    // Private

    void rebuildToplineText() {
        var time = timeAsString(Hour);
        var sleepWait = 16 - _hour;
        var sleepTime = (sleepWait <= 0 || _asleep) ? "Time to sleep now" : $"Sleep in: {timeAsString(sleepWait)}";

        topline.text = $"Day {Day}  {time}";
        rightLine.text = $"{sleepTime}";
    }

    string timeAsString(float time) {
        var partial = time - (float)(int)time;
        var minutes = 60f * partial;
        var strMin = $"{minutes}";
        int index = strMin.IndexOf(".");
        if (index >= 0) {
            strMin = strMin.Substring(0, index);
        }
        while (strMin.Length < 2) {
            strMin += "0";
        }

        return $"{(int)time}:{strMin}";
    }

    void Update() {
        if (!_asleep || Time.time < _sleepTimestamp + Ludum.Dare.Data.HourInGameTime || Ludum.Dare.GameIsOver) {
            return;
        }

        _sleepTimestamp = Time.time;
        Ludum.Dare.Temperature.Hour += 1f;

        if (Ludum.Dare.Temperature.Hour >= 24) {
            _asleep = false;
            Ludum.Dare.Commands.GainFocus();
            Ludum.Dare.GM.DoEndDay();
        }
    }
}
