using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Agent : MonoBehaviour
{
    public float speed;
    public PathGA pathGA = null;

    private Vector3 target;
    private Vector3 lastPos;
    private GameObject go;

    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Wall")
        {
            target = transform.position;
            lastPos = transform.position;
        }
         
            //Debug.Log("hit a wall");
/*
        if (col.gameObject.tag == "River")
        {
        }
 */           //Debug.Log("hit a river...");
    }


    private void MoveForward()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    private void FixedUpdate()
    {

        bool atTarget  = Vector3.Distance(transform.position, target)  < 0.02f;
        bool notMoving = Vector3.Distance(transform.position, lastPos) < 0.01f;

        if (!atTarget)
        {
            //Debug.Log("Distance to target >= 0.01f: " + Vector3.Distance(transform.position, target));
            transform.LookAt(target);
            lastPos = transform.position;
            MoveForward();

            if (notMoving)
            {

                target = transform.position;
                Debug.Log("Not moving...");
            }
        }
        else
        { 
            Path p = pathGA.SimulatePaths(transform.position, true);
            target = p.CreateAbsolutePath(transform.position).First();
            //Utils.DrawPath(p, transform.position, 10.0f);
            Utils.DrawLine(p.CreateAbsolutePathWithStart(transform.position).Take(2).ToList(), 1.0f); 

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