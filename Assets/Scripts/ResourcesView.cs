using TMPro;
using UnityEngine;

public class ResourcesView: MonoBehaviour {
    [SerializeField] TextMeshProUGUI _amountText;
    [SerializeField] int _amount;
    [SerializeField] bool _hasMax;
    [SerializeField] int _max;

    public bool Ratio;

    public int Amount {
        get => _amount;
        set {
            _amount = value;
            if (_amount > 0) {
                gameObject.SetActive(true);
            }
            refreshText();
        }
    }

    public bool HasMax {
        get => _hasMax;
        set {
            _hasMax = value;
            refreshText();
        }
    }

    public int Max {
        get => _max;
        set {
            _max = value;
            refreshText();
        }
    }

    // Private

    void refreshText() {
        if (Ratio) {
            _amountText.text = $"1 : {_amount}";
        } else {
            _amountText.text = (_hasMax) ? $"{_amount}\n{_max}" : $"{_amount}";
        }
    }
}
