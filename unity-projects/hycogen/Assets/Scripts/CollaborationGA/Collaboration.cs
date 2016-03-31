using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Collaboration
{
    // fitness will be evaluated in the simulation
    public float fitness = 0;

    // here are all the settings for all possible agents stored
    public List<AgentSettings> agentSList = null;

    // length of the agentSList
    public int agentSCount = 0;

    // this may be important for future tweaking
    public int generationsLived = 0;

    // bounds for random generation of AgentSettings and future Mutation
    // .First ^= Minimum
    // .Second ^= Maximum
    public static Tuple<int,int>      collisionBounds       = new Tuple<int, int>(-100, 100);
    public static Tuple<float,float>  greekBounds           = new Tuple<float, float>(0.0f, 1.0f);
    public static Tuple<int,int>      popSizeBounds         = new Tuple<int, int>(2, 10);
    public static Tuple<int,int>      subPathCountBounds    = new Tuple<int, int>(2, 5);
    public static Tuple<float,float>  subPathLengthBounds   = new Tuple<float, float>(0.5f, 2.0f);
    public static Tuple<int,int>      generationCountBounds = new Tuple<int, int>(1, 10);
    public static Tuple<int,int>      modeBounds            = new Tuple<int, int>(1, 2);
    public static Tuple<float,float>  maxDeviationBounds    = new Tuple<float, float>(0.5f, 5.0f);
    public static Tuple<float,float>  speedBounds           = new Tuple<float, float>(0.5f, 5.0f);

    /*
     * creation of a Collaboration(-individual) randomly by specifiying the numbers of agents
     **/
    public Collaboration(int agentSCount)
    {
        this.agentSCount = agentSCount;
        this.agentSList  = CreateAgentSList(agentSCount);
    }

    /*
     * custom creation of a Collaboration(-individual) by specifying the agents
     **/
    public Collaboration(List<AgentSettings> agentSList)
    {
        this.agentSList  = agentSList;
        this.agentSCount = agentSList.Count;
    }

    /*
     * random generator for AgentSettings
     * the bounds for the percentages are readonly (not supported yet by current c# version)
     * the bounds for the collisions are defined by good measure - not tested yet
     **/

    public static AgentSettings CreateAgentSettings()
    {
        int wallCollision      = Random.Range(collisionBounds.First, collisionBounds.Second);
        int riverCollision     = Random.Range(collisionBounds.First, collisionBounds.Second);
        int agentCollision     = Random.Range(collisionBounds.First, collisionBounds.Second);
        int targetCollision    = Random.Range(collisionBounds.First, collisionBounds.Second);
        int agentPathCollision = Random.Range(collisionBounds.First, collisionBounds.Second);

        float alpha = Random.Range(greekBounds.First, greekBounds.Second);
        float beta  = Random.Range(greekBounds.First, greekBounds.Second);
        float gamma = Random.Range(greekBounds.First, greekBounds.Second);
        float delta = Random.Range(greekBounds.First, greekBounds.Second);

        int popSize         = Random.Range(popSizeBounds.First, popSizeBounds.Second);
        int subPathCount    = Random.Range(subPathCountBounds.First, subPathCountBounds.Second);
        float subPathLength = Random.Range(subPathLengthBounds.First, subPathLengthBounds.Second);
        int generationCount = Random.Range(generationCountBounds.First, generationCountBounds.Second);
        int mode            = Random.Range(modeBounds.First, modeBounds.Second);
        float maxDeviation  = Random.Range(maxDeviationBounds.First, maxDeviationBounds.Second);
        float speed         = Random.Range(speedBounds.First, speedBounds.Second);

        return CreateCustomAgentSettings(
              wallCollision,      riverCollision, agentCollision, targetCollision
            , agentPathCollision, popSize,        subPathCount,   subPathLength
            , generationCount,    alpha,          beta,           mode
            , gamma,              delta,          maxDeviation,   speed);
    }


    /*
     * unsafe creation of AgentSettings without checking the bounds
     **/
    public static AgentSettings CreateCustomAgentSettings(
          int   wallCollision,   int   riverCollision,     int   agentCollision
        , int   targetCollision, int   agentPathCollision, int   popSize
        , int   subPathCount,    float subPathLength,      int   generationCount
        , float alpha,           float beta,               int   mode          
        , float gamma,           float delta,              float maxDeviation
        , float speed)
    {
        PathGA pathGA = new PathGA(
            wallCollision,      riverCollision, agentCollision, targetCollision
          , agentPathCollision, popSize,        subPathCount,   subPathLength
          , generationCount,    alpha,          beta,           mode
          , gamma,              delta,          maxDeviation);

        return new AgentSettings(pathGA, speed);
    }

    /*
     * Creates a random AgentSettingsList for a Collaboration (-individual)
     **/
    public static List<AgentSettings> CreateAgentSList(int agentSCount)
    {
        List<AgentSettings> agentsSCount = new List<AgentSettings>();

        for (int i = 0; i < agentSCount; i++)
        {
            agentsSCount.Add(CreateAgentSettings());
        }

        return agentsSCount;
    }

}
