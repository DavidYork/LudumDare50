using UnityEngine;

public class TestText: MonoBehaviour {
    [SerializeField] SelectableRow[] rows;
    [SerializeField] RowSelector rowSelector;

    void Start() {
        foreach (var row in rows) {
            rowSelector.RegisterRow(row);
        }
        rowSelector.GainFocus();
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Tab)) {
            if (rowSelector.IsFocused) {
                rowSelector.LoseFocus();
            } else {
                rowSelector.GainFocus();
            }
        }
    }
}
