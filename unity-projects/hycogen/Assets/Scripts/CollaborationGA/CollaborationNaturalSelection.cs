using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class CollaborationNaturalSelection
{
    // how many new individuals should be created in %
    private float                alpha;

    // how many children are created via crossover in %
    private float                beta;

    private CollaborationFactory cFactory;

    /*
     * α - how many individuals should be created in %
     * β - how many children are created via crossover in %
     **/
    public CollaborationNaturalSelection(float alpha, float beta, CollaborationFactory cFactory)
    {
        this.alpha    = alpha;
        this.beta     = beta;
        this.cFactory = cFactory;

        if (alpha < 0.0f || alpha > 1.0f)
        {
            Debug.Log("CollaborationNaturalSelection.cs: Constructor - alpha has to be between 0.0 and 1.0, but it's: " + alpha.ToString());
        }

        if (beta < 0.0f || beta > 1.0f)
        {
            Debug.Log("CollaborationNaturalSelection.cs: Constructor - beta has to be between 0.0 and 1.0, but it's: " + beta.ToString());
        }
    }

    /*
     * We get the evaluated parents and we know that there is β amount of children waiting to be merged
     * know we want to make room for exactly α amount of new individuals
     **/
    public Collaboration[] Apply(Collaboration[] cols)
    {
        int popSize        = cFactory.popSize;
        int newIndividuals = Mathf.FloorToInt(alpha * popSize);
        int futureChildren = Mathf.FloorToInt(beta  * popSize);
        int best = popSize - futureChildren - newIndividuals;

        //Debug.Log("CollaborationNS.cs: Apply - Popsize from CollaborationFactory - " + popSize.ToString() + " | cols.Count - " + cols.Count().ToString());
        //Debug.Log("CollaborationNS.cs: Apply - α = " + alpha.ToString() + " | newIndividuals = " + newIndividuals.ToString());
        //Debug.Log("CollaborationNS.cs: Apply - β = " + beta.ToString()  + " | futureChildren = " + futureChildren.ToString());

        cols = cols.OrderByDescending(c => c.fitness).ToArray();

        Collaboration[] selected = new Collaboration[best];

        for (int i = 0; i < best; i++)
        {
            selected[i] = cols[i];
        }

        return selected;
    }

    /*
     * Creates the new population with the popSize from CollaborationFactory
     * 
     * First we copy every parent
     * If there is some space left, copy children
     * If there is still some space left, create new random paths
     * 
     * @TODO remove sorting by fitness after tests
     **/
    public Collaboration[] Repopulate(Collaboration[] parents, Collaboration[] children)
    {
        Collaboration[] newPopulation = new Collaboration[cFactory.popSize];

        int parentsCount  = parents.Count();
        int childrenCount = children.Count();

        //   Debug.Log("CollaborationNS.cs: Repopulate - CollaborationFactory.popSize: " + cFactory.popSize.ToString());
        //   Debug.Log("CollaborationNS.cs: Repopulate - parentsCount: " + parentsCount.ToString());
        //   Debug.Log("CollaborationNS.cs: Repopulate - childrenCount: " + childrenCount.ToString());

        parents = parents.OrderByDescending(c => c.fitness).ToArray();

        for (int i = 0; i < cFactory.popSize; i++)
        {
            if (i < parentsCount)
            {
                newPopulation[i] = parents[i];
            }
            else if ((i - parentsCount) < childrenCount)
            {
                newPopulation[i] = children[i - parentsCount];
            }
            else
            {
                newPopulation[i] = cFactory.GenCollaboration();
            }
        }

        return newPopulation;
    }

}
