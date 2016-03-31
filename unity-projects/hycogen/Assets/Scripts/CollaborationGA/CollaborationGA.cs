using UnityEngine;
using System.Collections;

public class CollaborationGA : MonoBehaviour
{
    /** STATE **/

    bool active = true;
    int  currentGenerationCount = 0;

    /** GENERATION **/

    private int popSize = 10;
    private int generationCount = 10;

    /** CROSSOVER **/

    // how many new individuals should be created in %
    private float alpha = 0.2f;

    // how many children are created via crossover in %
    private float beta  = 0.2f;

    // defines the type of crossover we are using (no higher order functions available)
    // * 1 <-> OnePointCrossover
    // * 2 <-> TwoPointCrossover
    private int mode = 2;

    /** MUTATION **/

    // how many individuals are selected for mutation in %
    private float gamma = 0.5f;

    // how much of every individual should be mutated in %
    private float delta = 0.3f;

    /** SIMULATION **/
    private int maxFrames = 60 * 15; // 15 seconds

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

            if (currentGenerationCount < generationCount - 1)
            {
                if (cSimulator.evaluated)
                {
                    Collaboration[] children   = cCrossover.Apply(population);
                    Collaboration[] mutated    = cMutation.Apply(children);
                    Collaboration[] selected   = cNatSelection.Apply(population);
                    population                 = cNatSelection.Repopulate(selected, mutated);
                    currentGenerationCount    += 1;
                    cSimulator.evaluated       = false;
                }

                if (!cSimulator.active && !cSimulator.evaluated)
                {
                    cSimulator.population = population;
                    cSimulator.maxFrames = maxFrames;
                    cSimulator.active = true;
                }
            }
            else
            {
                active = false;
                Debug.Log("CollaborationGA.cs: I'm done!");
            }
        }
    }
}
