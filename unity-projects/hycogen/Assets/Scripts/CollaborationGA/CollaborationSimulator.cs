using UnityEngine;
using System.Collections;

public class CollaborationSimulator : MonoBehaviour
{

    /** STATES **/
    public  bool active     = false;
    public  bool evaluated  = false;

    private bool simulating = false;

    private int  colCounter = 0;
    public  int  popSize    = 0;

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
            //Debug.Log("ColSim.cs: Starting with agentSettings: " + agentS.ToString());
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
     * We calculate the minimum distance to the point -5 on the x-axis, as this is the target!
     * 1 / minDistance, so we get a fitness where higher ^= better
     **/
    public float CalculateFitness()
    {
        float minDistance = 10; // the field is no longer than 10 units in width so this is the lowest fitness there is

        foreach (GameObject agent in GameObject.FindGameObjectsWithTag("Agent"))
        {
            minDistance = Mathf.Min(agent.transform.position.x - (-5.0f), minDistance);
        }
            
        return 1 / Mathf.Abs(minDistance);
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
                Debug.Log("Frames...: " + currentFrames);
                if (maxFrames <= currentFrames)
                {
                    population[colCounter].fitness = CalculateFitness();
                    population[colCounter].generationsLived += 1;
                    Debug.Log("CollaborationSimulator.cs: Assessed the following fitness: " + population[colCounter].fitness.ToString());
                    RemoveAllAgents();
                    ResetFrames();
                    colCounter++;
                    simulating = false;
                }
            }

            if (!simulating)
            {
                if (colCounter < popSize)
                {
                    StartSimulation(population[colCounter]);
                    Debug.Log("Starting simulation for " + colCounter.ToString());
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
        GUI.TextField(new Rect(225, 40, 100, 20), "#Individual: " + colCounter.ToString());
    }
}
