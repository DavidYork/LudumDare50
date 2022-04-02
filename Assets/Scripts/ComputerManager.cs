using TMPro;
using UnityEngine;

public class ComputerManager: MonoBehaviour {
    [SerializeField] TextMeshProUGUI text;

    public void SetText(string msg) {
        text.text = msg;
    }
}
