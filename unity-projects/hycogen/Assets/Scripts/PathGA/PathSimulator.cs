using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathSimulator
{
    private float wallCollision;
    private float riverCollision;
    private float agentPathCollision;
    private float targetCollision;

    public PathSimulator(float wallCollision, float riverCollision, float agentPathCollision, float targetCollision)
    {
        this.wallCollision      = wallCollision;
        this.riverCollision     = riverCollision;
        this.agentPathCollision = agentPathCollision;
        this.targetCollision    = targetCollision;
    }

    /*
     * simulates the path with a 0.5f diameter sphere and looks up every collision
     * every collision has a fitness value which is accumulated and returned
     **/
    public float CollectFitness(Vector3 curPos, Path p)
    {
        float         fitness = 0.0f;
        Vector3       lastPos = curPos;
        List<Vector3> absPath = p.CreateAbsolutePathWithStart(curPos);

        foreach (Vector3 target in absPath)
        {
            Ray ray = new Ray(lastPos, target);
            lastPos = target;

            foreach(RaycastHit hit in Physics.SphereCastAll(ray, 0.5f, Vector3.Distance(lastPos, target)))
            {
                switch (hit.collider.gameObject.tag)
                {   
                    case "Wall":          fitness += wallCollision;      break;
                    case "RiverTrigger":  fitness += riverCollision;     break;
                    case "AgentPath":     fitness += agentPathCollision; break;
                    case "TargetTrigger": fitness += targetCollision;    break;

                        // Tags which are only for the Agent to collide with, e.g these can't be percepted via "sensors"
                    case "Agent":                                         break;
                    case "Target":                                        break;
                    case "Platform":                                      break;
                    default:
                        Debug.Log("PathSimulatior.cs: CollectFitness - collided with an unknown tag: " + hit.collider.tag);
                        break;
                }
            }
        }

        return fitness;
    }

    /*
     * simulates the fitness for every path in the array and updates its fitness corresponding to the result
     * 
     **/
    public void SimulatePaths(Vector3 curPos, Path[] ps, bool draw)
    {
        for (int i = 0; i < ps.Length; i++)
        {
            ps[i].fitness = CollectFitness(curPos, ps[i]);

            //Debug.Log("Is this even called?");
            //if (draw)
            //{
            // Utils.DrawPath(ps[i], curPos, Color.black, 0.05f);
            //}
        }
    }
}
