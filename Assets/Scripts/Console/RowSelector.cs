using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Responsible for knowing which row is selected, changing selection, etc
public class RowSelector: MonoBehaviour {
    [SerializeField] ScrollRect scrollRect;
    [SerializeField] GameObject scrollUpIndicator;
    [SerializeField] GameObject scrollDownIndicator;

    List<SelectableRow> rows;
    int idx = -2;

    public Action<SelectableRow> OnSelectedChanged;

    public SelectableRow SelectedRow => (idx >= 0) ? rows[idx] : null;

    public bool IsFocused => idx >= 0;

    public int SelectedIndex {
        get => idx;
        set {
            var oldVal = idx;
            idx = value;
            for (var i=0; i<rows.Count; i++) {
                rows[i].Selected = (i == idx);
            }

            if (oldVal != idx) {
                var row = SelectedRow;
                if (row == null) {
                    scrollRect.verticalNormalizedPosition = 1;
                } else {
                    var rowRT = row.GetComponent<RectTransform>();
                    var boundsRT = scrollRect.GetComponent<RectTransform>();
                    var val = IsOutsideY(rowRT, boundsRT);
                    while (val < 0 && scrollRect.verticalNormalizedPosition > 0f) {
                        scrollRect.verticalNormalizedPosition -= .01f;
                        val = IsOutsideY(rowRT, boundsRT);
                    }
                    while (val > 0 && scrollRect.verticalNormalizedPosition < 1f) {
                        scrollRect.verticalNormalizedPosition += .01f;
                        val = IsOutsideY(rowRT, boundsRT);
                    }
                }

                if (OnSelectedChanged != null) {
                    OnSelectedChanged(row);
                }
            }
        }
    }

    public void RegisterRow(SelectableRow row) {
        rows.Add(row);
        row.Selected = false;
    }

    public void UnregisterRow(SelectableRow row) {
        rows.Remove(row);
        row.Selected = false;
        GameObject.Destroy(row.gameObject);
    }

    public void UnregisterAllRows() {
        SelectedIndex = -1;
        foreach (var row in rows) {
            GameObject.Destroy(row.gameObject);
        }
        rows.Clear();
    }

    public void GainFocus() {
        if (idx < 0 || idx >= rows.Count) {
            SelectedIndex = (rows.Count > 0) ? 0 : -1;
        }
    }

    public void LoseFocus() {
        SelectedIndex = -1;
    }

    // Private

    void Awake() => rows = new List<SelectableRow>();
    void Start() => SelectedIndex = (rows.Count == 0) ? -1 : 0;

    void Update() {
        if (IsFocused) {
            if (Kbd.DownPressed(0)) {
                if (SelectedIndex + 1 < rows.Count) {
                    SelectedIndex++;
                }
            }
            if (Kbd.UpPressed(0)) {
                if (SelectedIndex > 0) {
                    SelectedIndex--;
                }
            }
            if (Input.GetKeyDown(KeyCode.Home)) {
                SelectedIndex = 0;
            }
            if (Input.GetKeyDown(KeyCode.End)) {
                SelectedIndex = rows.Count - 1;;
            }
        }

        if (rows.Count == 0) {
            scrollDownIndicator.SetActive(false);
            scrollUpIndicator.SetActive(false);
        } else {
            var bounds = scrollRect.GetComponent<RectTransform>();
            scrollDownIndicator.SetActive(IsOutsideY(rows[^1].GetComponent<RectTransform>(), bounds) != 0);
            scrollUpIndicator.SetActive(IsOutsideY(rows[0].GetComponent<RectTransform>(), bounds) != 0);
        }
    }


    // Returns how many pixels "inside" is above or below "bounds"
    static float IsOutsideY(RectTransform inside, RectTransform bounds) {
        Vector3[] insideCorders = new Vector3[4];
        inside.GetWorldCorners(insideCorders);
        Vector3[] outsideCorners = new Vector3[4];
        bounds.GetWorldCorners(outsideCorners);

        var outsideTopLeftScreenSpace = Camera.main.WorldToScreenPoint(outsideCorners[0]);
        var outsideBottomRightScreenSpace = Camera.main.WorldToScreenPoint(outsideCorners[2]);

        Vector3 tempScreenSpaceCorner; // Cached
        for (var i=0; i<insideCorders.Length; i++) {
            tempScreenSpaceCorner = Camera.main.WorldToScreenPoint(insideCorders[i]);
            if (tempScreenSpaceCorner.y < outsideTopLeftScreenSpace.y) {
                return tempScreenSpaceCorner.y - outsideTopLeftScreenSpace.y;
            }
            if (tempScreenSpaceCorner.y > outsideBottomRightScreenSpace.y) {
                return tempScreenSpaceCorner.y - outsideBottomRightScreenSpace.y;
            }
        }
        return 0;
    }
}
