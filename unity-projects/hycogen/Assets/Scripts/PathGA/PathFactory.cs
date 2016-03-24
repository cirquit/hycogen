using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathFactory
{
    public int   popSize;
    public int   subPathCount;
    public float subPathLength;

    public PathFactory(int popSize, int subPathCount, float subPathLength)
    {
        this.popSize       = popSize;
        this.subPathCount  = subPathCount;
        this.subPathLength = subPathLength;
    }

    public Path[] GenPaths()
    {
        Path[] paths = new Path[popSize];

        for (int i = 0; i < popSize; i++)
        {
            paths[i] = GenPath();
        }

        return paths;
    }

    public Path GenPath()
    {
        return new Path(subPathCount, subPathLength);
    }

    public Path GenCustomPath(List<Vector2> path)
    {
        return new Path(path);
    }
}
