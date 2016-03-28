using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Agent : MonoBehaviour {



//	private Vector3  lastPosition;
//	private bool active = true;
//	private bool once   = true;


    public float speed;
    public PathGA pathGA = null;

    private Vector3 target;
    private GameObject go;

    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Wall")
            target = transform.position;

        if (col.gameObject.tag == "River")
            Debug.Log("hit a river...");
    }


    private void MoveForward()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    private void FixedUpdate()
    {

      //  Path p = pathGA.SimulatePaths(transform.position, true);
        if (Vector3.Distance(transform.position,target) >= 0.01) 
        {
            transform.LookAt(target);
            MoveForward();
        }
        else
        {
            //GameObject go = GameObject.FindGameObjectWithTag ("Target");
            //target = go.transform.position; //transform.LookAt (target.transform);
            Path p = pathGA.SimulatePaths(transform.position, true);
            target = p.CreateAbsolutePath(transform.position).First();
            Utils.DrawPath(p, transform.position, 1.0f);

            Debug.Log("[Agent]: fitness - " + p.fitness.ToString());
        }
    }

    private void Start()
    {
        target = transform.position;
    }



}

/*	void OnCollisionEnter(Collision col)
	{
		if (col.gameObject.tag == "River") {
			active = false;
		}
	} */

	/**
	 * move forward per step ^= speed * deltaTime
	 */

	
/*
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
*/
/*		if (!StillMoving () && once) {
			Vector3 pos = new Vector3 (4.0f, 1.5f, 0.0f);
			Quaternion rot = Quaternion.identity;
			Instantiate (Resources.Load ("AgentPrefab"), pos, rot);
			once = false;
		}
*/
//	}
//}
