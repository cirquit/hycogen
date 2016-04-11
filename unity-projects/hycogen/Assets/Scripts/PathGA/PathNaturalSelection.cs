using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PathNaturalSelection
{
    // how many new individuals should be created in %
    private float       alpha;

    // how many children are created via crossover in %
    private float       beta;

    private PathFactory pFactory;

    /*
     * α - how many individuals should be created in %
     * β - how many children are created via crossover in %
     **/
    public PathNaturalSelection(float alpha, float beta, PathFactory pFactory)
    {
        this.alpha    = alpha;
        this.beta     = beta;
        this.pFactory = pFactory;

        if (alpha < 0.0f || alpha > 1.0f)
        {
            Debug.Log("PathNaturalSelection.cs: Constructor - alpha has to be between 0.0 and 1.0, but it's: " + alpha.ToString());
        }

        if (beta < 0.0f || beta > 1.0f)
        {
            Debug.Log("PathNaturalSelection.cs: Constructor - beta has to be between 0.0 and 1.0, but it's: " + beta.ToString());
        }
    }

    /*
     * We get the evaluated parents and we know that there is β amount of children waiting to be merged
     * know we want to make room for exactly α amount of new individuals
     **/
    public Path[] Apply(Path[] paths)
    {
        int popSize        = pFactory.popSize;
        int newIndividuals = Mathf.FloorToInt(alpha * popSize);
        int futureChildren = Mathf.FloorToInt(beta  * popSize);
        int best = Mathf.Max(0, popSize - (futureChildren + newIndividuals));

        //Debug.Log("PathNS.cs: Apply - Popsize from PathFactory - " + popSize.ToString() + " | paths.Count - " + paths.Count().ToString());
        //Debug.Log("PathNS.cs: Apply - α = " + alpha.ToString() + " | newIndividuals = " + newIndividuals.ToString());
        //Debug.Log("PathNS.cs: Apply - β = " + beta.ToString()  + " | futureChildren = " + futureChildren.ToString());

        paths = paths.OrderByDescending(path => path.fitness).ToArray();

        Path[] selected = new Path[best];

        for (int i = 0; i < best; i++)
        {
            selected[i] = paths[i];
        }

        return selected;
    }

    /*
     * Creates the new population with the popSize from PathFactory
     * 
     * First we copy every parent
     * If there is some space left, copy children
     * If there is still some space left, create new random paths
     * 
     * @TODO remove sorting by fitness after tests
     **/
    public Path[] Repopulate(Path[] parents, Path[] children)
    {
        Path[] newPopulation = new Path[pFactory.popSize];

        int parentsCount  = parents.Count();
        int childrenCount = children.Count();

     //   Debug.Log("PathNS.cs: Repopulate - PathFactory.popSize: " + pFactory.popSize.ToString());
     //   Debug.Log("PathNS.cs: Repopulate - parentsCount: " + parentsCount.ToString());
     //   Debug.Log("PathNS.cs: Repopulate - childrenCount: " + childrenCount.ToString());

        parents = parents.OrderByDescending(path => path.fitness).ToArray();

        for (int i = 0; i < pFactory.popSize; i++)
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
                newPopulation[i] = pFactory.GenPath();
            }
        }

        return newPopulation;
    }
}
