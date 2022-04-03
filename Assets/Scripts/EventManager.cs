using System.Collections.Generic;
using UnityEngine;

public class EventManager: MonoBehaviour {
    Dictionary<Event, bool> _events;

    public bool GetEvent(Event evt) {
        bool val = false;
        _events.TryGetValue(evt, out val);
        return val;
    }

    public void SetEvent(Event evt, bool val) {
        _events[evt] = val;
    }

    void Awake() => _events = new Dictionary<Event, bool>();
}
