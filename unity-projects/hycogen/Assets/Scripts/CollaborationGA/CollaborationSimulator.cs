using UnityEngine;
using System.Collections;

public class CollaborationSimulator : MonoBehaviour
{

    /** STATES **/
    public  bool active          = false;
    public  bool evaluated       = false;

    private bool simulating      = false;

    private int  colCounter      = 0;
    private int  simCounter      = 0;    
    public  int  simulationCount = 0;
    public  int  popSize         = 0;

    // this defines the duration of a single simulation
    public  int maxFrames;
    private int currentFrames;

    public Collaboration[] population = null;

    // hardcoded fitness function - get to the target!
    // we take the minimum distance from all agents to the x-coord '-5', thats the x.pos of the target

    // Target targetScript = null;


    /*
     * Initialize all the agent in the current collaboration right next to each other with 0.2f units padding
     **/
    public void StartSimulation(Collaboration c)
    {
        float z = -3.0f;

        foreach (AgentSettings agentS in c.agentSList)
        {
            agentS.Initialize(new Vector3(3.0f, 1.5f, z));
            z += 3.0f;
            Debug.Log("ColSim.cs: Starting with agentSettings: " + agentS.ToString());
        }
        simulating = true;
    }

    /*
     * a more descriptive name for future use
     **/
    public void ResetFrames()
    {
        currentFrames = 0;
    }

    /*
     * a more descriptive name for future use
     **/

    public void ResetSimCounter()
    {
        simCounter = 0;
    }


    /*
     * We calculate the minimum distance to the point -5 on the x-axis, as this is the target!
     * 1 / minDistance, so we get a fitness where higher ^= better
     **/
    public float CalculateFitness()
    {
        float minDistance = 10.0f;
        int   agentCount  = 0;  

        foreach (GameObject agent in GameObject.FindGameObjectsWithTag("Agent"))
        {
            agentCount++;
            Agent ascript = agent.GetComponent<Agent>();
            minDistance = Mathf.Min(minDistance, agent.transform.position.x - (-5.0f));
            Debug.Log("ColSim.cs: pathGA.maxDev:" + ascript.pathGA.maxDeviation + ". minDistance = " + minDistance);
        }
            
        if (agentCount < 1)
        {
            Debug.Log("ColSim.cs: CalculateFitness - There are not agents to be found, this should never happen");
            return 0.0f;
        }

        return Mathf.Abs(minDistance);
    }

    public void RemoveAllAgents()
    {
        foreach (GameObject agent in GameObject.FindGameObjectsWithTag("Agent"))
        {
            Destroy(agent);
        }
    }

    private void Start()
    {
//        GameObject target = GameObject.FindGameObjectWithTag("Target");
//        targetScript = (Target) target.GetComponent<Target>();
    }


    /*
     * This could be easily written more compact, but for the sake of discriptivity
     * I'm not doing this - it's a state machine
     **/
    private void FixedUpdate()
    {
        if (active)
        {
            if (simulating)
            {
                currentFrames += 1;
              //  Debug.Log("Frames...: " + currentFrames);
                if (maxFrames <= currentFrames)
                {
                    float fitness = CalculateFitness();
                    population[colCounter].fitness += fitness / simulationCount;
//                    Debug.Log("CollaborationSimulator.cs: Assessed the following fitness: " + fitness.ToString());
                    if (fitness <= 4.5)
                    {
                        Debug.Log("GOT IT!");
                    }
                    RemoveAllAgents();
                    ResetFrames();
                    simCounter += 1;

                    if (simulationCount <= simCounter)
                    {
                        population[colCounter].generationsLived += 1;
                        colCounter++;
                        simulating = false;
                        ResetSimCounter();

                    } else
                    {
                        StartSimulation(population[colCounter]);
                    }

                }
            }

            if (!simulating)
            {
                if (colCounter < popSize)
                {
                    StartSimulation(population[colCounter]);
                    // Debug.Log("Starting simulation for " + colCounter.ToString());
                }
                else
                {
                    active    = false;
                    evaluated = true;
                    colCounter = 0;
                    Debug.Log("CollaborationSimulator.cs: Update - Finished all Simulations!");
                }
            }
        }
    }


    private void OnGUI()
    {
        GUI.TextField(new Rect(225, 40, 200, 20), "#Individual: " + colCounter.ToString() + ", #Sim: " + simCounter.ToString());
    }
}
