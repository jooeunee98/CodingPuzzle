//	modified ParticleTester.cs from Simple Particle Pack
//	https://www.assetstore.unity3d.com/en/#!/content/3045

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ParticleTester_cartoon_explosion_fire_smoke : MonoBehaviour {
	public Object[] particleSystems;
	private Vector2 scrollPosition;
	
	void Awake () {
		particleSystems = (Object[])Resources.LoadAll(Application.loadedLevelName+"/effects", typeof(GameObject));
	}
	
	void OnGUI () {
		scrollPosition = GUILayout.BeginScrollView (scrollPosition, GUILayout.Width (250), GUILayout.Height (550));

				/*foreach (GameObject ps in particleSystems) {
					if (GUILayout.Button (ps.name)) {
						GameObject go = Instantiate (ps, Vector3.zero, Quaternion.identity) as GameObject;
						Destroy (go, 10);
					}
				}*/		
	    GUILayout.EndScrollView ();
	}
}
