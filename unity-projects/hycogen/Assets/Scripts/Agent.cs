using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Agent : MonoBehaviour
{
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

        if (Vector3.Distance(transform.position,target) >= 0.01) 
        {
            transform.LookAt(target);
            MoveForward();
        }
        else
        { 
            Path p = pathGA.SimulatePaths(transform.position, true);
            target = p.CreateAbsolutePath(transform.position).First();
            Utils.DrawPath(p, transform.position, 1.0f);

            Debug.Log("[Agent]: fitness: " + p.ToViewString());
        }


        /*
        Path p = pathGA.RandomPath(transform.position);
        Debug.Log("[Agent]: path: " + p.ToViewString());
        */
    }

    private void Start()
    {
        target = transform.position;
    }
}