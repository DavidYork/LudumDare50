using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Ludum))]
public class LudumEditor: Editor {
	public override void OnInspectorGUI () {
        var ld = (Ludum)target;

        if (GUILayout.Button("Show Map")) {
            ld.State.Current = StateManager.State.Map;
        }
        if (GUILayout.Button("Show Hab")) {
            ld.State.Current = StateManager.State.Hab;
        }
        if (GUILayout.Button("Show Dream")) {
            ld.State.Current = StateManager.State.Dream;
        }
		base.OnInspectorGUI();
    }
}