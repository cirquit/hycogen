using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PathGA 
{
    /** FITNESS VARIABLES **/
    public int   wallCollision;
    public int   riverCollision;
    public int   agentCollision;
    public int   agentPathCollision;
    public int   targetCollision;


    /** GENERATION **/

    public int   popSize;
    public int   subPathCount;
    public float subPathLength;
    public int   generationCount;

    /** CROSSOVER **/

    // how many new individuals should be created in %
    public float alpha;

    // how many children are created via crossover in %
    public float beta;

    // defines the type of crossover we are using (no higher order functions available)
    // * 1 <-> OnePointCrossover
    // * 2 <-> TwoPointCrossover
    public int   mode;

    /** MUTATION **/

    // how many individuals are selected for mutation in %
    public float gamma;

    // how much of every individual should be mutated in %
    public float delta;

    // how much is the maximum deviation for the coordinates
    public float maxDeviation;


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
        this.mode          = mode;

        this.pFactory      = new PathFactory(this.popSize, this.subPathCount, this.subPathLength);
        this.pSimulator    = new PathSimulator(this.wallCollision, this.riverCollision, this.agentPathCollision, this.targetCollision, this.agentCollision);
        this.pNatSelection = new PathNaturalSelection(this.alpha, this.beta, this.pFactory); 
        this.pCrossover    = new PathCrossover(this.beta, this.mode, this.pFactory);
        this.pMutation     = new PathMutation(this.gamma, this.delta, this.maxDeviation);
    }

    /*
     * This will be called from CollaborationMutation after mutating the parameters
     **/
    public void ReinitalizeGA()
    {
        this.pFactory      = new PathFactory(this.popSize, this.subPathCount, this.subPathLength);
        this.pSimulator    = new PathSimulator(this.wallCollision, this.riverCollision, this.agentPathCollision, this.targetCollision, this.agentCollision);
        this.pNatSelection = new PathNaturalSelection(this.alpha, this.beta, this.pFactory); 
        this.pCrossover    = new PathCrossover(this.beta, this.mode, this.pFactory);
        this.pMutation     = new PathMutation(this.gamma, this.delta, this.maxDeviation);
    }

    /*
     * starts the genetic algorithm and returns the next best path after #generationCount generations
     **/
    public Path SimulatePaths(Vector3 curPos)
    {
        if (population == null)
        {
            population = pFactory.GenPaths();
        }
        else
        { 
            for (int i = 0; i < popSize; i++)
            {
                population[i].BuildNextStep();
            }
        }


        for (int i = 0; i < generationCount - 1; i++)
        {
            pSimulator.SimulatePaths(curPos, population);
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

    override public string ToString()
    {
        return "WC: "   + this.wallCollision        + ", RC: "    + this.riverCollision  + ", AC: "     + this.agentCollision
          + ", APC: "   + this.agentPathCollision   + ", TC: "    + this.targetCollision + ", PopS: "   + this.popSize 
          + ", subPC: " + this.subPathCount         + ", subPL: " + this.subPathLength.ToString("F2")   + ", GENC: "   + this.generationCount
          + ", α: "     + this.alpha.ToString("F2") + ", β: "     + this.beta.ToString("F2")            + ", COm: "    + this.mode
          + ", γ: "     + this.gamma.ToString("F2") + ", δ: "     + this.delta.ToString("F2")           + ", maxDev: " + this.maxDeviation.ToString("F2");
    }
}
