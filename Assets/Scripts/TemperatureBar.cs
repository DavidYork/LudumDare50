using UnityEngine;
using UnityEngine.UI;

public class TemperatureBar: MonoBehaviour {
    [SerializeField] Image _cursor;
    [SerializeField] Color _inThePastColor;
    [SerializeField] Sprite _safeCursor;
    [SerializeField] Sprite _dangerCursor;

    public Image Icon;

    bool _hourHasPassed;
    int _cold;

    public int Cold {
        get => _cold;
        set {
            _cold = Mathf.Max(0, value);
            Icon.sprite = Ludum.Dare.Temperature.IconForTemp(_cold);
            _cursor.sprite = (ColdDamage > 0) ? _dangerCursor : _safeCursor;
        }
    }

    public int ColdDamage {
        get {
            if (_cold < 5) { return 0; }
            if (_cold < 8) { return 1; }
            if (_cold < 12) { return 2; }
            else { return 3; }
        }
    }

    public bool HourHasPassed {
        get => _hourHasPassed;
        set {
            _hourHasPassed = value;
            var color = (_hourHasPassed) ? _inThePastColor : Color.white;
            _cursor.color = color;
            Icon.color = color;
        }
    }

    public bool IsNow {
        get => _cursor.gameObject.activeSelf;
        set => _cursor.gameObject.SetActive(value);
    }
}
