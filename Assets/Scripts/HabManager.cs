using UnityEngine;

public class HabManager: MonoBehaviour {
    [SerializeField] GameObject[] _habObjects;

    public void Show() {
        foreach (var obj in _habObjects) {
            obj.SetActive(true);
        }
    }

    public void Hide() {
        foreach (var obj in _habObjects) {
            obj.SetActive(false);
        }
    }
}
