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

    public static AgentSettings CreateAgentSettings()
    {
        int collisionMinBound  = -100;
        int collisionMaxBound  =  100;
        float greekMinBound    = 0.0f;
        float greekMaxBound    = 1.0f;

        int wallCollision      = Random.Range(collisionMinBound, collisionMaxBound);
        int riverCollision     = Random.Range(collisionMinBound, collisionMaxBound);
        int agentCollision     = Random.Range(collisionMinBound, collisionMaxBound);
        int targetCollision    = Random.Range(collisionMinBound, collisionMaxBound);
        int agentPathCollision = Random.Range(collisionMinBound, collisionMaxBound);

        float alpha = Random.Range(greekMinBound, greekMaxBound);
        float beta  = Random.Range(greekMinBound, greekMaxBound);
        float gamma = Random.Range(greekMinBound, greekMaxBound);
        float delta = Random.Range(greekMinBound, greekMaxBound);

        int popSize         = Random.Range(1, 100);     // this should not be below zero
        int subPathCount    = Random.Range(1, 10);      // this should not be below zero
        float subPathLength = Random.Range(0.5f, 2.0f); // this should not be below zero
        int generationCount = Random.Range(1, 50);      // this should not be below one
        int mode            = Random.Range(1, 2);       // these are the current maxBounds (OnePoint- & TwoPointCrossover)
        float maxDeviation  = Random.Range(0.5f, 5.0f); // this should not be too low (maximum Mutation)
        float speed         = Random.Range(0.5f, 5.0f); // this should not be below zero

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
     * Creates a random AgentSettingsList for an Collaboration (-individual)
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
