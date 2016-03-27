using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathGA 
{
    /** FITNESS VARIABLES **/
    private int   wallCollision;
    private int   riverCollision;
    private int   agentCollision;
    private int   agentPathCollision;
    private int   targetCollision;


    /** GENERATION **/

    private int   popSize;
    private int   subPathCount;
    private int   subPathLength;

    /** CROSSOVER **/

    // how many new individuals should be created in %
    private float alpha;

    // how many children are created via crossover in %
    private float beta;

    // defines the type of crossover we are using (no higher order functions available)
    // * 1 <-> OnePointCrossover
    // * 2 <-> TwoPointCrossover
    private int   mode;

    /** MUTATION **/

    // how many individuals are selected for mutation in %
    private float gamma;

    // how much of every individual should be mutated in %
    private float delta;

    // how much is the maximum deviation for the coordinates
    private float maxDeviation;


    /** GENETIC ALGORITHM **/

    private PathFactory          pFactory      = null;
    private PathNaturalSelection pNatSelection = null;
    private PathCrossover        pCrossover    = null;
    private PathMutation         pMutation     = null;


    public PathGA(int   wallCollision,   int   riverCollision,     int   agentCollision
                , int   targetCollision, int   agentPathCollision, int   popSize
                , int   subPathCount,    int   subPathLength,      float alpha
                , float beta,            int   mode,               float gamma
                , float delta,           float maxDeviation)
    {

        this.wallCollision      = wallCollision;
        this.riverCollision     = riverCollision;
        this.agentCollision     = agentCollision;
        this.agentPathCollision = agentPathCollision;
        this.targetCollision    = targetCollision;

        this.popSize       = popSize;
        this.subPathCount  = subPathCount;
        this.subPathLength = subPathLength;
        this.alpha         = alpha;
        this.beta          = beta;
        this.gamma         = gamma;
        this.delta         = delta;
        this.maxDeviation  = maxDeviation;

        this.pFactory      = new PathFactory(popSize, subPathCount, subPathLength);
        this.pNatSelection = new PathNaturalSelection(alpha, beta, this.pFactory); 
        this.pCrossover    = new PathCrossover(beta, mode, this.pFactory);
        this.pMutation     = new PathMutation(gamma, delta, maxDeviation);
    }


    public Path SimulatePaths(Vector3 curPos, float time, bool drawpaths)
    {

        Path p = pFactory.GenPath();
        Utils.DrawPath(p, curPos, Color.black, 0.05f);

        // Debug.Log("PathGA.cs: SimulatePaths: fitness - " + CollectFitness(curPos, p));
        float x = CollectFitness(curPos, p);
        return p;
    }


    public float CollectFitness(Vector3 curPos, Path p)
    {
        float         fitness = 0.0f;
        Vector3       lastPos = curPos;
        List<Vector3> absPath = p.CreateAbsolutePath(curPos);

        foreach (Vector3 target in absPath)
        {
            //RaycastHit hit;
            Ray        ray = new Ray(lastPos, target);
            lastPos        = target;

            foreach(RaycastHit hit in Physics.SphereCastAll(ray, 0.00f, Vector3.Distance(lastPos, target)))
            {
                switch (hit.collider.gameObject.tag)
                {   
                    case "Wall":      fitness += wallCollision;      break;
                    case "River":     fitness += riverCollision;     break;
                    case "Agent":     fitness += agentCollision;     break;
                    case "AgentPath": fitness += agentPathCollision; break;
                    case "Target":    fitness += targetCollision;    break;
                    default:
                        Debug.Log("PathGA.cs: CollectFitness - collided with an unknown tag: " + hit.collider.tag);
                        break;
                }
            }
        }

        return fitness;
    }

}
