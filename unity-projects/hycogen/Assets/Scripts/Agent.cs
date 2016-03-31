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

    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Wall")
        {
            target = transform.position;
            lastPos = transform.position;
        }
         
    }

    private void MoveForward()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    private void FixedUpdate()
    {

        bool atTarget  = Vector3.Distance(transform.position, target)  < 0.05f;
        bool notMoving = Vector3.Distance(transform.position, lastPos) < 0.05f;

        if (!atTarget)
        {
            transform.LookAt(target);
            lastPos = transform.position;
            MoveForward();

            if (notMoving)
            {
                target = transform.position;
            }
        }
        else
        { 
            Path p = pathGA.SimulatePaths(transform.position, true);
            target = p.CreateAbsolutePath(transform.position).First();
            //Utils.DrawPath(p, transform.position, 10.0f);
            Utils.DrawLine(p.CreateAbsolutePathWithStart(transform.position).Take(2).ToList(), 0.5f); 

            Debug.Log("[Agent]: fitness: " + p.ToViewString());
        }
    }

    private void Start()
    {
        target = transform.position;
    }
}