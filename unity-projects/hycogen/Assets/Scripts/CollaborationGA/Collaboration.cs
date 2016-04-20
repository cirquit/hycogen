using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

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
    public static Tuple<float,float>  subPathLengthBounds   = new Tuple<float, float>(0.5f, 4.0f);
    public static Tuple<int,int>      generationCountBounds = new Tuple<int, int>(1, 10);
    public static Tuple<int,int>      modeBounds            = new Tuple<int, int>(1, 2);
    public static Tuple<float,float>  maxDeviationBounds    = new Tuple<float, float>(0.5f, 5.0f);
    public static Tuple<float,float>  speedBounds           = new Tuple<float, float>(0.5f, 2.75f);

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
        int agentPathCollision = 0; //Random.Range(collisionBounds.First, collisionBounds.Second);

        float alpha = Random.Range(greekBounds.First, greekBounds.Second);
        float beta  = Random.Range(greekBounds.First, greekBounds.Second);
        float gamma = Random.Range(greekBounds.First, greekBounds.Second);
        float delta = Random.Range(greekBounds.First, greekBounds.Second);

        int   popSize         = Random.Range(popSizeBounds.First, popSizeBounds.Second);
        int   subPathCount    = Random.Range(subPathCountBounds.First, subPathCountBounds.Second);
        float subPathLength   = Random.Range(subPathLengthBounds.First, subPathLengthBounds.Second);
        int   generationCount = Random.Range(generationCountBounds.First, generationCountBounds.Second);
        int   mode            = Random.Range(modeBounds.First, modeBounds.Second);
        float maxDeviation    = Random.Range(maxDeviationBounds.First, maxDeviationBounds.Second);
        float speed           = Random.Range(speedBounds.First, speedBounds.Second);

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
        List<AgentSettings> agentsSList = new List<AgentSettings>();

/*        for (int i = 0; i < agentSCount; i++)
        {
            agentsSList.Add(CreateAgentSettings());
  
        }
*/
// custom
//        agentsSList.Add(CreateCustomAgentSettings(-50, 100, 0,    0, 0, 10, 4, 3.0f, 10, 0.25f, 0.25f, 2, 0.5f, 0.5f, 2.0f, 2.0f)); 
//        agentsSList.Add(CreateCustomAgentSettings(-50, -25, 50, 100, 0, 10, 4, 3.0f, 10, 0.25f, 0.25f, 2, 0.5f, 0.5f, 2.0f, 3.0f)); 

// #1 4/200
//        agentsSList.Add(CreateCustomAgentSettings(-69, -60,  94, 0,  81, 6, 3, 1.89f, 7, 0.09f, 0.55f, 1, 0.32f, 0.38f, 0.80f, 2.64f)); 
//        agentsSList.Add(CreateCustomAgentSettings(-2,   33, -22, 0, -19, 2, 4, 3.50f, 6, 0.68f, 0.76f, 1, 0.21f, 0.84f, 1.58f, 2.67f)); 

// #2 1/200
//        agentsSList.Add(CreateCustomAgentSettings(-65, -79, -69, 0,  37, 8, 3, 1.86f, 1, 0.59f, 0.29f, 1, 0.00f, 0.15f, 1.83f, 2.11f)); 
//        agentsSList.Add(CreateCustomAgentSettings(1,   -74, -47, 0, -90, 3, 3, 2.69f, 7, 0.79f, 0.62f, 1, 0.57f, 0.53f, 3.42f, 2.05f)); 

// #3  6/200
//        agentsSList.Add(CreateCustomAgentSettings(20,  73,  65, 0, -58, 9, 3, 3.64f, 5, 0.73f, 0.33f, 1, 0.74f, 0.49f, 2.01f, 2.66f)); 
//        agentsSList.Add(CreateCustomAgentSettings(-49, 70, -69, 0,  10, 4, 4, 3.86f, 6, 0.70f, 0.06f, 1, 0.73f, 0.62f, 2.11f, 0.67f));

// #4 0/200
//        agentsSList.Add(CreateCustomAgentSettings(31,  8,  82, -26, -102, 5, 4, 1.19f, 1, 0.65f, 0.62f, 1, 0.05f, 0.07f, 1.38f, 2.28f)); 
//        agentsSList.Add(CreateCustomAgentSettings(-63, 42, -9,   0,  -62, 6, 2, 2.99f, 5, 0.53f, 0.32f, 1, 0.52f, 0.78f, 1.75f, 2.37f)); 

// #5 2/200
//        agentsSList.Add(CreateCustomAgentSettings(-53, 19, 19, 0,   6, 2, 3, 3.89f, 4, 0.45f, 0.18f, 1, 0.80f, 0.35f, 2.98f, 2.73f)); 
//        agentsSList.Add(CreateCustomAgentSettings(-52, 38, 33, 0, -28, 3, 3, 2.66f, 4, 0.08f, 0.46f, 1, 0.03f, 0.30f, 4.53f, 2.37f)); 

// #6 3/200
//        agentsSList.Add(CreateCustomAgentSettings(97, -44,  65, 0, 99, 8, 4, 2.44f, 6, 0.83f, 0.36f, 1, 0.55f, 0.88f, 1.18f, 2.25f)); 
//        agentsSList.Add(CreateCustomAgentSettings(-38, 35, -66, 0, 81, 5, 3, 3.78f, 5, 0.14f, 0.15f, 1, 0.03f, 0.06f, 2.94f, 2.46f)); 

// #7 2/200
//        agentsSList.Add(CreateCustomAgentSettings(70, -93, -78, 0, 62, 8, 4, 3.80f, 7, 0.42f, 0.73f, 1, 0.32f, 0.14f, 2.47f, 2.31f)); 
//        agentsSList.Add(CreateCustomAgentSettings(-85, 45, -44, 0,  9, 7, 4, 2.04f, 8, 0.48f, 0.02f, 1, 0.50f, 0.35f, 0.55f, 1.50f)); 

// #8 3/200
//        agentsSList.Add(CreateCustomAgentSettings(-87, 9,  -76,  23, -85, 5, 4, 3.35f, 6, 0.78f, 0.71f, 1, 0.70f, 0.35f, 4.83f, 1.66f)); 
//        agentsSList.Add(CreateCustomAgentSettings(-62, 66, -55, -11, 73,  8, 3, 2.64f, 4, 0.13f, 0.06f, 1, 0.85f, 0.59f, 3.88f, 2.19f)); 

// #9 0/200
//        agentsSList.Add(CreateCustomAgentSettings(-34, -59, -26, 0, 7,  2, 4, 3.60f, 9, 0.32f, 0.36f, 1, 0.49f, 0.03f, 2.82f, 2.61f)); 
//        agentsSList.Add(CreateCustomAgentSettings(-54, -95, 77,  0, 10, 6, 4, 0.66f, 5, 0.06f, 0.62f, 1, 0.46f, 0.69f, 2.74f, 0.60f)); 

// #10 7/200
//        agentsSList.Add(CreateCustomAgentSettings(-34, -59, -26, 0,  7, 2, 4, 3.60f, 9, 0.32f, 0.36f, 1, 0.49f, 0.03f, 2.82f, 2.61f)); 
//        agentsSList.Add(CreateCustomAgentSettings(76,   -8,  90, 0, 91, 2, 2, 1.88f, 3, 0.58f, 0.20f, 1, 0.81f, 0.36f, 3.51f, 2.41f)); 

// #11 0/200
//        agentsSList.Add(CreateCustomAgentSettings(-87, 9,  -76, 0, -95, 5, 4, 3.35f, 2, 0.51f, 0.08f, 1, 0.32f, 0.01f, 3.35f, 1.66f)); 
//        agentsSList.Add(CreateCustomAgentSettings(-54, 66, -55, 0, 73, 8, 3, 2.64f, 8, 0.13f, 0.66f, 1, 0.85f, 0.22f, 3.88f, 2.19f)); 

// #12 1/200
//        agentsSList.Add(CreateCustomAgentSettings(-85, 45,-44, 0,  9, 7, 4, 2.04f, 8, 0.48f, 0.02f, 1, 0.50f, 0.35f, 0.55f, 1.50f)); 
//        agentsSList.Add(CreateCustomAgentSettings( 70,-93,-78, 0, 62, 8, 4, 3.80f, 7, 0.42f, 0.73f, 1, 0.32f, 0.14f, 2.47f, 2.31f)); 

          agentsSList.Add(CreateCustomAgentSettings(-100, 100, -100, -100, 0, 10, 5, 4.0f, 10, 0.50f, 0.25f, 1, 0.25f, 0.50f, 2.50f, 2.25f)); 
          agentsSList.Add(CreateCustomAgentSettings(-100, -60, -20,   100, 0, 10, 5, 4.0f, 10, 0.50f, 0.25f, 1, 0.25f, 0.50f, 2.50f, 2.75f)); 

        return agentsSList;
    }

    public override string ToString()
    {
        int hash = 0;

        foreach (AgentSettings aS in agentSList)
        {
            hash += aS.GetHashCode();
        }

        hash = hash % 100000;

        string res = hash.ToString() + " | GLd: " + generationsLived.ToString() + "| F: " + fitness.ToString("F2");

        return res;
    }
}
