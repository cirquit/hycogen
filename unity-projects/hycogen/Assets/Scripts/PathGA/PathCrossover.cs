using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PathCrossover
{
    // how many children are created in %
    private float beta;

    // defines the type of crossover we are using (no higher order functions available)
    // * 1 <-> OnePointCrossover
    // * 2 <-> TwoPointCrossover
    private int mode;

    private PathFactory pFactory = null;

    /*
     * β - how many children are created in %
     * mode: 1 <-> OnePointCrossover
     *       2 <-> TwoPointCrossover
     **/
    public PathCrossover(float beta, int mode, PathFactory pFactory)
    {
        this.beta     = beta;
        this.mode     = mode;
        this.pFactory = pFactory;

        if (beta < 0.0f || beta > 1.0f)
        {
            Debug.Log("PathCrossover.cs: Constructor - beta has to be between 0.0 and 1.0, it's: " + beta.ToString());
        }
    }


    /*
     * array should have at least 2 items to apply crossover
     * 
     * parents are shuffled, then applying crossover with sliding window with size 2
     * children array has the size := parents.length * beta (rounded down) 
     **/
    public Path[] Apply(Path[] parents)
    {
        if (parents.Length < 2)
        {
            Debug.Log("PathCO.cs: Apply - parents.Count doesn't allow crossover, count = " + parents.Length.ToString());
            return parents;
        }

        Utils.Shuffle(parents);

        int    childrenSize = Mathf.FloorToInt(beta * parents.Length);
        Path[] children     = new Path[childrenSize];

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


    /*
     * crossover based on the mode set
     * 1 <-> OnePointCrossover
     * 2 <-> TwoPointCrossover
     * 
     * if the mode was invalid, OnePointCrossover will be used
     **/
    public Path Crossover(Path a, Path b)
    {
        Path child = null;
        switch (mode)
        {
            case 1: child = OnePointCrossover(a, b); break;
            case 2: child = TwoPointCrossover(a, b); break;
            default:
                Debug.Log("PathCO.cs: Apply - mode: " + mode.ToString() + " is undefined, using OnePointCO");
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
    public Path OnePointCrossover(Path a, Path b)
    {
        int minLength = Mathf.Min(a.path.Count, b.path.Count);
        int cut       = Random.Range(0, minLength);

        List<Vector2> preCut   = a.path.Take(cut).ToList();
        List<Vector2> afterCut = b.path.Skip(cut).ToList();
        preCut.AddRange(afterCut);

        return pFactory.GenCustomPath(preCut);
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
    public Path TwoPointCrossover(Path a, Path b)
    {
        int len = Mathf.Min(a.path.Count, b.path.Count);
        int cut = Random.Range(0, len);

        List<Vector2> a1     = a.path.Take(cut).ToList();
        List<Vector2> a1Rest = a.path.Skip(cut).ToList();
        List<Vector2> b1Rest = b.path.Skip(cut).ToList();

        int sublen = len - cut;
        int subcut = Random.Range(0, sublen);

        List<Vector2> a3 = a1Rest.Skip(subcut).ToList();
        List<Vector2> b2 = b1Rest.Take(subcut).ToList();

        b2.AddRange(a3);
        a1.AddRange(b2);

        return pFactory.GenCustomPath(a1);
    }
}
