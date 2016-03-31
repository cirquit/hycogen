using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class CollaborationCrossover
{
    // how many children are created in %
    private float beta;

    // defines the type of crossover we are using (no higher order functions available)
    // * 1 <-> OnePointCrossover
    // * 2 <-> TwoPointCrossover
    private int mode;

    private CollaborationFactory cFactory = null;

    /*
     * β - how many children are created in %
     * mode: 1 <-> OnePointCrossover
     *       2 <-> TwoPointCrossover
     **/
    public CollaborationCrossover(float beta, int mode, CollaborationFactory cFactory)
    {
        this.beta     = beta;
        this.mode     = mode;
        this.cFactory = cFactory;

        if (beta < 0.0f || beta > 1.0f)
        {
            Debug.Log("CollaborationCrossover.cs: Constructor - beta has to be between 0.0 and 1.0, it's: " + beta.ToString());
        }

        if (mode < 1 || mode > 2)
        {
            Debug.Log("CollaborationCrossover.cs: Constructor - mode has to be either 1 or 2, it's: " + mode.ToString()); 
        }
    }

    /*
     * array should have at least 2 items to apply crossover
     * 
     * parents are shuffled, then applying crossover with sliding window with size 2
     * children array has the size := parents.length * beta (rounded down) 
     **/
    public Collaboration[] Apply(Collaboration[] parents)
    {
        if (parents.Length < 2)
        {
            Debug.Log("PathCO.cs: Apply - parents.Count doesn't allow crossover, count = " + parents.Length.ToString());
            return parents;
        }

        Utils.Shuffle(parents);

        int             childrenSize = Mathf.FloorToInt(beta * parents.Length);
        Collaboration[] children     = new Collaboration[childrenSize];

        for (int i = 0; i < childrenSize-1; i++)
        {
            children[i] = Crossover(parents[i], parents[i + 1]);
        }

        if (childrenSize != 0)
        {
            children[childrenSize - 1] = Crossover(parents[0], parents[childrenSize - 1]);
        }

        return children;
    }


    public Collaboration Crossover(Collaboration a, Collaboration b)
    {
        Collaboration child = null;
        switch (mode)
        {
            case 1: child = OnePointCrossover(a, b); break;
            case 2: child = TwoPointCrossover(a, b); break;
            default:
                Debug.Log("CollaborationCO.cs: Apply - mode: " + mode.ToString() + " is undefined, using OnePointCO");
                child = OnePointCrossover(a, b); break;
        }

        return child;
    }

    /*
     * Example crossover with different sized paths
     * child will always have the length of the longest parent
     * 
     * Example:
     * 
     * AAA
     * 
     * BBBBB
     * 
     * minimum length of both lists = 3
     * random point generated = 2 from (0, 3)
     * 
     * splitting at 2
     * 
     * res = AABBB
     * 
     * @TODO: make private after testing
     **/
    public Collaboration OnePointCrossover(Collaboration a, Collaboration b)
    {
        int minLength = Mathf.Min(a.agentSCount, b.agentSCount);
        int cut       = Random.Range(0, minLength);

        List<AgentSettings> preCut   = a.agentSList.Take(cut).ToList();
        List<AgentSettings> afterCut = b.agentSList.Skip(cut).ToList();
        preCut.AddRange(afterCut);

        return cFactory.GenCustomCollaboration(preCut);

    }

    /*
     * Example crossover with different sized paths
     * child will always have the length from the first path
     * 
     * Example:
     * 
     * AA AA
     * BB BB BB
     *
     * minimum length of both lists = 4
     * 
     * random point generated = 1 from Range(0,4)
     *
     * new minimum length of both lists = 3
     * 
     * random point generated = 2 from Range(0,3)
     *
     * splitting at 1 and (1+2)
     *
     * => AB BA
     * 
     * @TODO:
     *      make private after testing
     *      maybe rewrite for performace purposes
     **/
    public Collaboration TwoPointCrossover(Collaboration a, Collaboration b)
    {
        int minLength = Mathf.Min(a.agentSCount, b.agentSCount);
        int cut       = Random.Range(0, minLength);

        List<AgentSettings> a1     = a.agentSList.Take(cut).ToList();
        List<AgentSettings> a1Rest = a.agentSList.Skip(cut).ToList();
        List<AgentSettings> b1Rest = b.agentSList.Skip(cut).ToList();

        int sublen = minLength - cut;
        int subcut = Random.Range(0, sublen);

        List<AgentSettings> a3 = a1Rest.Skip(subcut).ToList();
        List<AgentSettings> b2 = b1Rest.Take(subcut).ToList();

        b2.AddRange(a3);
        a1.AddRange(b2);

        return cFactory.GenCustomCollaboration(a1);
    }
}
