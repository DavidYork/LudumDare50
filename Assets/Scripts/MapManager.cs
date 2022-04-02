using UnityEngine;

public class MapManager: MonoBehaviour {
    [SerializeField] GameObject[] mapAnchors;

    public void Show() {
        foreach (var anchor in mapAnchors) {
            anchor.SetActive(true);
        }
    }

    public void Hide() {
        foreach (var anchor in mapAnchors) {
            anchor.SetActive(false);
        }
    }
}
