using UnityEngine;

public class HabManager: MonoBehaviour {
    public void Show() {
        gameObject.SetActive(true);
    }

    public void Hide() {
        gameObject.SetActive(false);
    }
}
