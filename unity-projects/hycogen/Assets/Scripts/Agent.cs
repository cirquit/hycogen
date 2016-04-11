using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Agent : MonoBehaviour
{
    public float speed;
    public bool active = true;
    public PathGA pathGA = null;

    private Vector3 target;
    private Vector3 lastPos;

    private void OnCollisionEnter(Collision col)
    {
/*        if (col.gameObject.tag == "Wall")
        {
//            Debug.Log("Hit a wall...");
        }
*/
        if (col.gameObject.tag == "River")
        {
            active = false;
        }
         
    }

    private void MoveForward()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    private void FixedUpdate()
    {
        if (active)
        {
            bool atTarget = Vector3.Distance(transform.position, target) < 0.05f;
            bool notMoving = Vector3.Distance(transform.position, lastPos) < 0.01f;

            if (!atTarget)
            {
                transform.LookAt(target);
                lastPos = transform.position;
                MoveForward();

                if (notMoving)
                {
                    //Debug.Log("Not moving, requesting next path");
                    target = RequestNextPath();
                }
            }
            else
            { 
                target = RequestNextPath();
            }
        }
    }

    public Vector3 RequestNextPath()
    {   
        Path p = pathGA.SimulatePaths(transform.position);
        Vector3 nextTarget = p.CreateAbsolutePath(transform.position).First();

        Utils.DrawPath(p, transform.position, 1.0f);

        //Utils.DrawLine(p.CreateAbsolutePathWithStart(transform.position).Take(2).ToList(), 0.5f); 

//        Debug.Log("[Agent]: fitness: " + p.ToViewString());

        return nextTarget;
    }

    private void Start()
    {
        target = transform.position;
    }
}