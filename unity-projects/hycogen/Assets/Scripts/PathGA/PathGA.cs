using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PathGA 
{
    /** FITNESS VARIABLES **/
    private int   wallCollision;
    private int   riverCollision;
    private int   agentCollision;
    private int   agentPathCollision;
    private int   targetCollision;


    /** GENERATION **/

    public  int   popSize;
    private int   subPathCount;
    private float subPathLength;
    private int   generationCount;

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
    private PathSimulator        pSimulator    = null;
    private PathNaturalSelection pNatSelection = null;
    private PathCrossover        pCrossover    = null;
    private PathMutation         pMutation     = null;
    public  Path[]               population    = null;

    public PathGA(int   wallCollision,   int   riverCollision,     int   agentCollision
                , int   targetCollision, int   agentPathCollision, int   popSize
                , int   subPathCount,    float subPathLength,      int   generationCount
                , float alpha,           float beta,               int   mode          
                , float gamma,           float delta,              float maxDeviation)
    {

        this.wallCollision      = wallCollision;
        this.riverCollision     = riverCollision;
        this.agentCollision     = agentCollision;
        this.agentPathCollision = agentPathCollision;
        this.targetCollision    = targetCollision;
        this.generationCount    = generationCount;

        this.popSize       = popSize;
        this.subPathCount  = subPathCount;
        this.subPathLength = subPathLength;
        this.alpha         = alpha;
        this.beta          = beta;
        this.gamma         = gamma;
        this.delta         = delta;
        this.maxDeviation  = maxDeviation;

        this.pFactory      = new PathFactory(popSize, subPathCount, subPathLength);
        this.pSimulator    = new PathSimulator(wallCollision, riverCollision, agentPathCollision, targetCollision);
        this.pNatSelection = new PathNaturalSelection(alpha, beta, this.pFactory); 
        this.pCrossover    = new PathCrossover(beta, mode, this.pFactory);
        this.pMutation     = new PathMutation(gamma, delta, maxDeviation);
    }

    /*
     * starts the genetic algorithm and returns the next best path after #generationCount generations
     **/
    public Path SimulatePaths(Vector3 curPos, bool drawPaths)
    {
        if (population == null)
        {
            population = pFactory.GenPaths();
        }
        else
        { 
            population.Select(p => p.BuildNextStep());
        }


        for (int i = 0; i < generationCount - 1; i++)
        {
            pSimulator.SimulatePaths(curPos, population, drawPaths);
            Path[] children   = pCrossover.Apply(population);
            Path[] mutated    = pMutation.Apply(children);
            Path[] selected   = pNatSelection.Apply(population);
            population        = pNatSelection.Repopulate(selected, mutated);
        }

        if (population.Length == 0)
        {
            Debug.Log("PathGA.cs: SimulatePaths - population is zero, this should never happen");
            return pFactory.GenPath();
        }
        else
        {
            return population[0];
        }
    }


    /*
     * draws a random path for testing purposes
     * @TODO remove
     **/
    public Path RandomPath(Vector3 curPos)
    {
        Path p = pFactory.GenPath();
        pSimulator.CollectFitness(curPos, p);
        Utils.DrawPath(p, curPos, 0.10f);

        return p;
    }
}
