using UnityEngine;
using System.Collections;
using System.Linq;

public class PathMutation
{

    // how many individuals are selected for mutation in %
    private float gamma;

    // how much of every individual should be mutated in %
    private float delta;

    // how much is the maximum deviation for the coordinates
    private float maxDeviation;

    public PathMutation (float gamma, float delta, float maxDeviation)
    {
        this.gamma        = gamma;
        this.delta        = delta;
        this.maxDeviation = maxDeviation;
    }

    public Path[] Apply(Path[] paths)
    {
        for (int i = 0; i < paths.Length; i++)
        {
            if (gamma >= Random.Range(0.0f, 1.0f))
            {
                paths[i] = Mutate(paths[i]);
            }
        }

        return paths;
    }

    // @TODO make private after tests
    public Path Mutate(Path p)
    {
        Vector2[] parr = p.path.ToArray();

        for (int i = 0; i < parr.Length; i++)
        {
            if (delta >= Random.Range(0.0f, 1.0f))
            {
                switch (Random.Range(0, 2))
                {
                    // mutate x
                    case 0:
                        //float x = MutateFloat(parr[i].x);
                        //parr[i] = new Vector2(x, parr[i].y);

                        // this should work too @TODO testing
                        parr[i].x = MutateFloat(parr[i].x);
                        break;

                    // mutate z
                    case 1:
                        parr[i].y = MutateFloat(parr[i].y);
                        break;

                    // mutate x and z
                    default:
                        parr[i].x = MutateFloat(parr[i].x);
                        parr[i].y = MutateFloat(parr[i].y);
                        break;
                }
            }
        }

        p.path = parr.ToList();
        return p;
    }

    // adds or subtracts randomly [0, maxDeviaton] from the float
    // @TODO set private after tests
    public float MutateFloat(float x)
    {
        int   f         = Random.Range(0, 1);
        float deviation = Random.Range(0.0f, maxDeviation);
        float res;

        switch (f)
        {
            // (+)
            case 0:
                res = x + deviation;
                break;

            // (-)
            default:
                res = x - deviation;
                break;
        }

        return res;
    }

}