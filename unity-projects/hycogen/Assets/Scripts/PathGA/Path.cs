﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Path
{

    // testing puposes
    // @TODO remove (remove in PathSimulator too)
    public int rivercount     = 0;
    public int targetcount    = 0;
    public int wallcount      = 0;
    public int agentPathcount = 0;
    public int agentcount     = 0;

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

    public Path(int subpathCount = 3, float subpathLength = 1.0f)
    {
        this.path          = CreatePath(subpathCount, subpathLength);
        this.subpathCount  = subpathCount;
        this.subpathLength = subpathLength;
    }

    /*
     * creates a semi random point with a random ° based on the unit circle
     * its length is in the range of [0.5f, subpathLength]
     * 
     * 0.5, so the agent doesn't have to calculate every couple frames a new path
     * 
     **/
    public static Vector2 CreateSemiRandomPoint(float subpathLength)
    {
        float phi = Random.Range(0, 2 * 3.1415926f);
        float x = Mathf.Cos(phi);
        float z = Mathf.Sin(phi);

        float length = Random.Range(0.5f, subpathLength);

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
            fitness     = 0;
            rivercount  = 0;
            wallcount   = 0;
            targetcount = 0;
        }
    }

    /*
     * Full representation of the path
     **/

    override public string ToString()
    {
        string result = "";

        foreach (Vector2 v in path)
        {
            result += "(" + v.x.ToString("F1") + ", " + v.y.ToString("F1") + ")";
        }

        result += "| GLd: " + generationsLived.ToString() + " | F: " + fitness.ToString();

        return result;
    }

    /*
     * shortened representation of the path for the GUI
     **/

    public string ToViewString()
    {
        return this.GetCustomHash(5) + "| GLd: " + generationsLived.ToString() + " | F: " + fitness.ToString()
            + " | RC: " + rivercount + " | TC: " + targetcount + " | WC: " + wallcount + " | AC: " + agentcount
            + " | APC: " + agentPathcount;
    }

    /*
     * simple hash function to see distinctive paths
     **/
    public string GetCustomHash(int length)
    {
        string res = "";
        foreach (char c in this.ToString().GetHashCode().ToString().Take(length).ToArray())
        {
            res += c;
        }
        return res;
    }

}