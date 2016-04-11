using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathSimulator
{
    private float wallCollision;
    private float riverCollision;
    private float agentPathCollision;
    private float targetCollision;
    private float agentCollision;

    public PathSimulator(float wallCollision, float riverCollision, float agentPathCollision, float targetCollision, float agentCollision)
    {
        this.wallCollision      = wallCollision;
        this.riverCollision     = riverCollision;
        this.agentPathCollision = agentPathCollision;
        this.targetCollision    = targetCollision;
        this.agentCollision     = agentCollision;
    }

    /*
     * simulates the path with a 1.0f diameter sphere and looks up every collision
     * every collision has a fitness value which is accumulated and returned
     * 
     * alters the path collision counters based on the collision
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

            foreach(RaycastHit hit in Physics.SphereCastAll(ray, 1.0f, Vector3.Distance(lastPos, target)))
            {
                switch (hit.collider.gameObject.tag)
                {   
                    case "Wall":
                            fitness += wallCollision;
                            p.wallcount += 1;
                        break;
                    case "RiverTrigger":
                            fitness += riverCollision;
                            p.rivercount += 1;
                        break;
                    case "AgentPath":
                            fitness += agentPathCollision;
                            p.agentPathcount += 1;
                        break;
                        // we only allow to get the reward once for the targetcollision
                    case "TargetTrigger":
                        if (p.targetcount < 1)
                        {
                            fitness += targetCollision;
                            p.targetcount += 1; 
                        }
                        break;
                    case "Agent":
                            fitness += agentCollision;
                            p.agentcount += 1;
                        break;

                    // Tags which are only for the Agent to collide with, e.g these can't be percepted via "sensors"
                    case "River":                                         break;
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
    public void SimulatePaths(Vector3 curPos, Path[] ps)
    {
        for (int i = 0; i < ps.Length; i++)
        {
            ps[i].fitness = CollectFitness(curPos, ps[i]);
            ps[i].generationsLived += 1;
        }
    }
}
