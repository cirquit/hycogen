using UnityEngine;
using System.Collections;

public class CollaborationGA : MonoBehaviour
{
    /** STATE **/

    bool active = true;
    int  currentGenerationCount = 0;

    /** GENERATION **/

    private int popSize = 20;
    private int generationCount = 100;

    /** CROSSOVER **/

    // how many new individuals should be created in %
    private float alpha = 0.2f;

    // how many children are created via crossover in %
    private float beta  = 0.3f;

    // defines the type of crossover we are using (no higher order functions available)
    // * 1 <-> OnePointCrossover
    // * 2 <-> TwoPointCrossover
    private int mode = 1;

    /** MUTATION **/

    // how many individuals are selected for mutation in %
    private float gamma = 0.5f;

    // how much of every individual should be mutated in %
    private float delta = 0.5f;

    /** SIMULATION **/
    private int maxFrames       = 60 * 10; // 10 seconds
    private int simulationCount = 10;      // amount of simulations for every individual

    private CollaborationFactory          cFactory      = null;
    private CollaborationCrossover        cCrossover    = null;
    private CollaborationMutation         cMutation     = null;
    private CollaborationNaturalSelection cNatSelection = null;
    private CollaborationSimulator        cSimulator    = null;
    public  Collaboration[]               population    = null;


    private void Start()
    {
        cFactory      = new CollaborationFactory(popSize);
        cCrossover    = new CollaborationCrossover(beta, mode, cFactory);
        cMutation     = new CollaborationMutation(gamma, delta);
        cNatSelection = new CollaborationNaturalSelection(alpha, beta, cFactory);

        GameObject obj = GameObject.FindGameObjectWithTag("ScriptObject");
        cSimulator     = (CollaborationSimulator) obj.GetComponent<CollaborationSimulator>();
    }

    /*
     * This could be easily written more compact, but for the sake of discriptivity
     * I'm not doing this - it's a state machine
     **/
    private void FixedUpdate()
    {
        if (active)
        {
            if (population == null)
            {
                population = cFactory.GenCollaborations();
            }

            if (currentGenerationCount < generationCount)
            {
                if (cSimulator.evaluated)
                {

                    Debug.Log("Starting Collaboration GA...");
                    Collaboration[] children   = cCrossover.Apply(population);
                    Debug.Log("Applied CO");
                    Collaboration[] mutated    = cMutation.Apply(children);
                    Debug.Log("Applied MU");
                    Collaboration[] selected   = cNatSelection.Apply(population);
                    Debug.Log("Applied NS");
                    population                 = cNatSelection.Repopulate(selected, mutated);
                    Debug.Log("Applied RP");
                    currentGenerationCount    += 1;
                    cSimulator.evaluated       = false;
                }

                if (!cSimulator.active && !cSimulator.evaluated && currentGenerationCount < generationCount)
                {
                    Debug.Log("starting simulation für genCount " + currentGenerationCount);
                    cSimulator.population      = population;
                    cSimulator.popSize         = popSize;
                    cSimulator.maxFrames       = maxFrames;
                    cSimulator.simulationCount = simulationCount;
                    cSimulator.active          = true;
                }
            }
            else
            {
                active = false;
                Debug.Log("CollaborationGA.cs: I'm done!");
            }
        }
    }
        
    private void OnGUI()
    {
        int height = 20;
        int width  = 200;
        int lines  = Mathf.Min(20, popSize);

        if (population != null)
        {
            GUI.Box(new Rect(0, 0, width, height * (lines + 1)), "Path fitness");

            for (int i = 0; i < lines; i++)
            {
                string line = "#" + i.ToString() + ": " + population[i].ToString();
                GUI.TextField(new Rect(0, (i+1) * height, width, height), line);
            }
        }

        GUI.TextField(new Rect(225, 20, 100, 20), "Generation: " + currentGenerationCount.ToString());
    }
}
