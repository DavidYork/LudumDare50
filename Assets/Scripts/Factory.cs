using System;
using UnityEngine;

public class Factory: MonoBehaviour {
    [SerializeField] Transform _commandAnchor;

    [SerializeField] SelectableRow _commandRowPrefab;

    public SelectableRow CreateCommandRow(string text, Action callback) {
        var rv = Instantiate<SelectableRow>(_commandRowPrefab, _commandAnchor);
        rv.name = text;
        rv.Setup(text, callback);
        return rv;
    }
}
