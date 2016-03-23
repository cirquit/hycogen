using UnityEngine;
using System.Collections;

public class Agent : MonoBehaviour {



	public float speed = 1;

	private Vector3  lastPosition;
	private float    step 	  	  = 1;
	private Renderer rend         = null;

	private bool active = true;
	private bool once = true;

	void OnCollisionEnter(Collision col){

		if (col.gameObject.tag == "River") {
			active = false;
		}
	}

	/**
	 * move forward per step 
	 */
	void MoveForward(){
		step = speed * Time.deltaTime;
		transform.position += transform.forward * step;
	}
		

	bool StillMoving(){

		if (!Utils.IsInBounds(transform.position.x, lastPosition.x)) {
			lastPosition = transform.position;
			return true;
		}

		if (!Utils.IsInBounds(transform.position.z, lastPosition.z)) {
			lastPosition = transform.position;
			return true;
		}

		return false;
	}

	void Start() {
		rend = GetComponent<Renderer>();
		lastPosition = transform.position;
	}

	void FixedUpdate () {

		if (active) {
			GameObject target = GameObject.FindGameObjectWithTag ("Target");
			transform.LookAt (target.transform);
			MoveForward ();
		}

		if (!StillMoving () && once) {
			Vector3 pos = new Vector3 (4.0f, 1.5f, 0.0f);
			Quaternion rot = new Quaternion ();
			GameObject agent = (GameObject)Instantiate (Resources.Load ("AgentPrefab"), pos, rot);
			once = false;
		}

	}
}
