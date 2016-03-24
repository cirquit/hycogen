using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Agent : MonoBehaviour {

	public float speed = 10;

	private Vector3  lastPosition;
	private Renderer rend         = null;

	private bool active = true;
	private bool once = true;

	void OnCollisionEnter(Collision col)
	{
		if (col.gameObject.tag == "River") {
			active = false;
		}
	}

	/**
	 * move forward per step ^= speed * deltaTime
	 */
	void MoveForward()
	{
        transform.position += transform.forward * speed * Time.deltaTime;
	}
		

	bool StillMoving()
	{
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

	void Start()
	{
		rend = GetComponent<Renderer>();
		lastPosition = transform.position;
	}

	void FixedUpdate ()
	{
		if (active) {
			GameObject target = GameObject.FindGameObjectWithTag ("Target");
			transform.LookAt (target.transform);
			MoveForward ();

			List<Vector3> l = new List<Vector3> ();

            l.Add (transform.position);
			l.Add (target.transform.position);
			
			Utils.DrawLine (l, Color.black);
		}

		if (!StillMoving () && once) {
			Vector3 pos = new Vector3 (4.0f, 1.5f, 0.0f);
			Quaternion rot = Quaternion.identity;
			GameObject agent = (GameObject)Instantiate (Resources.Load ("AgentPrefab"), pos, rot);
			once = false;
		}

	}
}
