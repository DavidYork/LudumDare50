using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreen: MonoBehaviour {
    [SerializeField] GameObject _spaceToStart;

    void Update() {
        if (Kbd.NextPressed(0)) {
            SceneManager.LoadScene("Main Game");
        }
        _spaceToStart.SetActive((int)Time.time % 2 == 0);
    }
}
