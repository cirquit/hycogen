using UnityEngine;
using System.Collections;

public class SpawnScript : MonoBehaviour {

	void Start () {
		Vector3 pos = new Vector3 (4.0f, 1.5f, 0.0f);
		Quaternion rot = new Quaternion ();
		GameObject agent = (GameObject) Instantiate(Resources.Load("AgentPrefab"), pos, rot);
	}
	

	void FixedUpdate () {
	}
}

