using UnityEngine;

public class MapManager: MonoBehaviour {
    public void Show() {
        gameObject.SetActive(true);
    }

    public void Hide() {
        gameObject.SetActive(false);
    }
}
