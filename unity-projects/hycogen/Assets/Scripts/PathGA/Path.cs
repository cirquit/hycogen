using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Path
{
    // fitness will be evaluated in the simulation
    public float fitness = 0;

    // encoding the path as points to go to (x,z)
    public List<Vector2> path = null;
  
    // subpathSize defines the maximum length of the difference between to points in path
    public float subpathLength;

    // subpaths defines the exact amount of subpaths this path can have
    public int   subpathCount;

    // how long are you living? @TODO maybe plays some role in how long the path should be
    public int   generationsLived = 0;

    public Path(List<Vector2> path)
    {
        this.path          = path;
        this.subpathCount  = path.Count;
        this.subpathLength = CalculateSubpathLength(path);

    }

    public Path(int subpathCount = 3, float subpathLength = 1)
    {
        this.path          = CreatePath(subpathCount, subpathLength);
        this.subpathCount  = subpathCount;
        this.subpathLength = subpathLength;
    }

    /*
     * creates a semi random point with a random ° based on the unit circle
     * its length is in the range of [0, subpathLength]
     * 
     **/
    public static Vector2 CreateSemiRandomPoint(float subpathLength)
    {
        float phi = Random.Range(0, 2 * 3.1415926f);
        float x = Mathf.Cos(phi);
        float z = Mathf.Sin(phi);

        float length = Random.Range(0.0f, subpathLength);

        return new Vector2(x * length, z * length);
    }

    /*
     * creates a pathlist with absolute coordiantes based on the current position
     **/
    public List<Vector3> CreateAbsolutePath(Vector3 start)
    {
        List<Vector3> absPath = new List<Vector3>();

        Vector3 lastPos = start;

        foreach (Vector2 curr in path)
        {
            lastPos = new Vector3(lastPos.x + curr.x, lastPos.y, lastPos.z + curr.y);
            absPath.Add(lastPos);
        }

        return absPath;
    }

    /*
     * creates a pathlist with absolute cordinates based on the current position
     * the current position is the first element
     **/
    public List<Vector3> CreateAbsolutePathWithStart(Vector3 start)
    {
        List <Vector3> absPath = CreateAbsolutePath(start);
        absPath.Insert(0, start);
        return absPath;
    }

    /*
     * creates #subpathCount paths in relative representation from (0,0)
     * the calculation is based on the unit circle
     * the real length of the subpaths are [0,subpathLength]
     **/

    public static List<Vector2> CreatePath(int subpathCount, float subpathLength)
    {
        List<Vector2> path = new List<Vector2>();

        for(int i = 0; i < subpathCount; i++)
        {
            path.Add(Path.CreateSemiRandomPoint(subpathLength));
        }

        return path;
    }


    /*
     * if the list is empty or has a single element it's not possible to take a difference
     *  => returns 0
     * else
     *  => take the difference with sliding window of size 2 and calculates the mean
     **/
    public static float CalculateSubpathLength(List<Vector2> path)
    {
        int pathCount = path.Count;

        if (pathCount < 2)
        {
            Debug.Log("Path.cs: CalculateSubpathSize - path.Count is less than 2 - exactly: " + pathCount.ToString());
            return 0;
        }
       
        Vector2[] pathArr = path.ToArray();
        float maxDifference = 0.0f;

        for (int i = 0; i < pathCount-1; i++)
        {
            int next = i + 1;
            maxDifference = Mathf.Max(maxDifference, Vector2.Distance(pathArr[i], pathArr[next]));
        }

        return maxDifference;
    }

    /*
     * drops the first step, adds a new one at the end and resets the fitness
     **/
    public void BuildNextStep()
    {
        if (path.Count == 0)
        {
            Debug.Log("Path.cs: BuildNextStep - path has the length 0, this should never happen");
        }
        else
        {
            path.RemoveAt(0);
            path.Add(Path.CreateSemiRandomPoint(subpathLength));
            fitness = 0;
        }
    }

}