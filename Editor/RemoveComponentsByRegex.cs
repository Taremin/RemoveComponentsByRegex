namespace RemoveComponentsByRegex {
	using System.Collections.Generic;
	using System.Collections;
	using System.Linq;
	using System.Text.RegularExpressions;
	using UnityEditor;
	using UnityEngine;

	public class RemoveComponentsByRegexWindow : EditorWindow {
		static GameObject activeObject;
		static string pattern = "";
		static bool isDryrun = true;

		void OnEnable () {
			pattern = EditorUserSettings.GetConfigValue ("RemoveComponentsByRegex/pattern") ?? "";
			isDryrun = bool.Parse (EditorUserSettings.GetConfigValue ("RemoveComponentsByRegex/isDryrun") ?? isDryrun.ToString ());
		}

		void OnSelectionChange () {
			var editorEvent = EditorGUIUtility.CommandEvent ("ChangeActiveObject");
			editorEvent.type = EventType.Used;
			SendEvent (editorEvent);
		}

		// Use this for initialization
		[MenuItem ("GameObject/Remove Components By Regex", false, 20)]
		public static void ShowWindow () {
			activeObject = Selection.activeGameObject;
			EditorWindow.GetWindow (typeof (RemoveComponentsByRegexWindow));
		}

		static void RemoveWalkdown (GameObject go, ref Regex regex, bool dryrun = false, int depth = 0) {
			// Components
			foreach (Component component in go.GetComponents<Component> ()) {
				if (regex.Match (component.GetType ().ToString ()).Success) {
					Debug.Log (typeof (RemoveComponentsByRegexWindow).Name + " - Remove:" + component.GetType ().Name + " in " + go.name);
					if (!dryrun) {
						Object.DestroyImmediate (component);
					}
				}
			}

			// Children
			var children = go.GetComponentInChildren<Transform> ();
			foreach (Transform child in children) {
				RemoveWalkdown (child.gameObject, ref regex, dryrun, depth + 1);
			}
		}

		private void OnGUI () {
			activeObject = Selection.activeGameObject;
			EditorGUILayout.LabelField ("アクティブなオブジェクト");
			using (new GUILayout.VerticalScope (GUI.skin.box)) {
				EditorGUILayout.LabelField (activeObject ? activeObject.name : "");
			}
			EditorGUILayout.LabelField ("アクティブなオブジェクトのコンポーネント");
			using (new GUILayout.VerticalScope (GUI.skin.box)) {
				if (!activeObject) {
					EditorGUILayout.LabelField ("");
				} else {
					EditorGUILayout.LabelField (
						string.Join (
							"\n",
							activeObject.GetComponents<Component> ()
							.Select (component => component.GetType ().ToString ()).ToArray ()
						),
						GUILayout.ExpandHeight (true)
					);
				}
			}

			EditorUserSettings.SetConfigValue (
				"RemoveComponentsByRegex/pattern",
				(pattern = EditorGUILayout.TextField ("コンポーネント正規表現", pattern))
			);
			EditorUserSettings.SetConfigValue (
				"RemoveComponentsByRegex/isDryrun",
				(isDryrun = GUILayout.Toggle (isDryrun, "削除対象をConsoleで確認(DryRun)")).ToString ()
			);

			if (GUILayout.Button ("Remove")) {
				var regex = new Regex (pattern);
				RemoveWalkdown (activeObject, ref regex, isDryrun);
			}
		}
	}
}