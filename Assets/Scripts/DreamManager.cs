using TMPro;
using UnityEngine;

public class DreamManager: MonoBehaviour {
    [SerializeField] GameObject target;
    [SerializeField] TextMeshProUGUI dreamText;
    [SerializeField] TextAsset dreamContent;
    [SerializeField] string defaultDream;

    string[] dreams;
    float showTime;

    public void DoNextDream() {
        target.SetActive(true);
        if (!Application.isPlaying) {
            return;
        }

        showTime = Time.time;
        dreamText.text = getTodaysDream();
    }

    public void Hide() => target.SetActive(false);
    public void OnDonePressed() => endDream();

    // Private

    void Awake() => dreams = dreamContent.text.Split('\n');
    void endDream() => Ludum.Dare.State.Current = State.Hab;

    string getTodaysDream() {
        var idx = (Mathf.Min(dreams.Length - 1, Ludum.Dare.Temperature.Day));
        var dream = dreams[idx];
        if (string.IsNullOrEmpty(dream)) {
            dream = defaultDream;
        }

        return dream.Replace('†', '\n');
    }

    void Update() {
        if (Ludum.Dare.State.Current != State.Dream) {
            return;
        }

        if (Kbd.NextPressed(showTime) || Kbd.CancelPressed(showTime)) {
            endDream();
        }
    }
}
