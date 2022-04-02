using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectableRow: MonoBehaviour {
    public enum SelectionEffect {
        ChangeColorAndBackground, ChangeColor, ChangeBackground, IconOnly
    }

    [SerializeField] SelectionEffect effect;
    [SerializeField] Image backdrop;
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] GameObject showIfSelected;
    [SerializeField] Color selectedTextColor = Color.red;
    [SerializeField] Color unselectedTextColor = Color.white;
    [SerializeField] Color selectedImageColor = Color.black;
    [SerializeField] Color unselectedImageColor = Color.black;

    public Action Callback { get; private set; }

    public bool Selected {
        get => _selected;
        set {
            _selected = value;

            if (effect == SelectionEffect.ChangeColorAndBackground || effect == SelectionEffect.ChangeColor) {
                text.color = (_selected) ? selectedTextColor : unselectedTextColor;
            }

            if (effect == SelectionEffect.ChangeColorAndBackground || effect == SelectionEffect.ChangeBackground) {
                backdrop.color = (_selected) ? selectedImageColor : unselectedImageColor;
            }

            showIfSelected?.SetActive(_selected);
        }
    }
    bool _selected;

    public void Setup(string msg, Action callback) {
        text.text = msg;
        Callback = callback;
    }

    // Private

    void Update() {
        if (!_selected) {
            return;
        }

        if (Kbd.NextPressed(0)) {
            Callback();
        }
    }
}
