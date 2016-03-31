using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class CollaborationMutation
{
    // how many individuals are selected for mutation in %
    private float gamma;

    // how much of every individual should be mutated in %
    private float delta;

    public CollaborationMutation (float gamma, float delta)
    {
        this.gamma = gamma;
        this.delta = delta;
    }

    public Collaboration[] Apply(Collaboration[] cols)
    {
        for (int i = 0; i < cols.Length; i++)
        {
            if (gamma >= Random.Range(0.0f, 1.0f))
            {
                cols[i] = Mutate(cols[i]);
            }
        }

        return cols;
    }

    // @TODO make private after tests
    public Collaboration Mutate(Collaboration c)
    {
        List<AgentSettings> newAgentSList = new List<AgentSettings>();

        for (int i = 0; i < c.agentSCount; i++)
        {
            if (delta >= Random.Range(0.0f, 1.0f))
            {
                AgentSettings newAS = Mutate(c.agentSList.ElementAt(i));
                newAgentSList.Add(newAS);
            }
        }

        c.agentSList = newAgentSList;

        return c;
    }


    /*
     * if the AgentSetting was chosen to be mutated, it will mutate ALL the settings randomly
     * if new settings will be
     **/
    public AgentSettings Mutate(AgentSettings a)
    {
        for (int i = 0; i < AgentSettings.settingsCount+1; i++)
        {
            switch (i)
            {
                case 0:
                    a.pathGA.wallCollision = MutateInt(a.pathGA.wallCollision, Collaboration.collisionBounds);
                    break;
                case 1:
                    a.pathGA.riverCollision = MutateInt(a.pathGA.riverCollision, Collaboration.collisionBounds);
                    break;
                case 2:
                    a.pathGA.agentCollision = MutateInt(a.pathGA.agentCollision, Collaboration.collisionBounds);
                    break;
                case 3:
                    a.pathGA.agentPathCollision = MutateInt(a.pathGA.agentPathCollision, Collaboration.collisionBounds);
                    break;
                case 4:
                    a.pathGA.targetCollision = MutateInt(a.pathGA.targetCollision, Collaboration.collisionBounds);
                    break;
                case 5:
                    a.pathGA.popSize = MutateInt(a.pathGA.popSize, Collaboration.popSizeBounds);
                    break;
                case 6:
                    a.pathGA.subPathCount = MutateInt(a.pathGA.subPathCount, Collaboration.subPathCountBounds);
                    break;
                case 7:
                    a.pathGA.subPathLength = MutateFloat(a.pathGA.subPathLength, Collaboration.subPathLengthBounds);
                    break;
                case 8:
                    a.pathGA.generationCount = MutateInt(a.pathGA.generationCount, Collaboration.generationCountBounds);
                    break;
                case 9:
                    a.pathGA.subPathLength = MutateFloat(a.pathGA.subPathLength, Collaboration.subPathLengthBounds);
                    break;
                case 10:
                    a.pathGA.alpha = MutateFloat(a.pathGA.alpha, Collaboration.greekBounds);
                    break;
                case 11:
                    a.pathGA.beta = MutateFloat(a.pathGA.beta, Collaboration.greekBounds);
                    break;
                case 12:
                    a.pathGA.gamma = MutateFloat(a.pathGA.gamma, Collaboration.greekBounds);
                    break;
                case 13:
                    a.pathGA.delta = MutateFloat(a.pathGA.delta, Collaboration.greekBounds);
                    break;
                case 14:
                    a.pathGA.mode = MutateInt(a.pathGA.mode, Collaboration.modeBounds);
                    break;
                case 15:
                    a.pathGA.maxDeviation = MutateFloat(a.pathGA.maxDeviation, Collaboration.maxDeviationBounds);
                    break;
                case 16:
                    a.speed = MutateFloat(a.speed, Collaboration.speedBounds);
                    break;
                default:
                    Debug.Log("CollaborationMutation.cs: a setting was added to AgentSettings and this case was not processed: " + i.ToString());
                    break;
            }
        }

        a.pathGA.ReinitalizeGA();
        return a;
    }

    /*
     * Mutates a float, until the random addition or subtraction is inside the bounds
     **/
    public static float MutateFloat(float x, Tuple<float,float> bounds)
    {
        float deviation = Random.Range(bounds.First, bounds.Second);

        while (x + deviation > bounds.Second
            || x - deviation < bounds.First)
        {
            deviation = Random.Range(bounds.First, bounds.Second);
        }

        if (x + deviation > bounds.Second)
        {
            return x + deviation;
        }
        else
        {
            return x - deviation;
        }
    }

    /*
     * Mutates an int, until the random addition or subtraction is inside the bounds
     **/
    public static int MutateInt(int x, Tuple<int,int> bounds)
    {
        int deviation = Random.Range(bounds.First, bounds.Second);

        while (x + deviation > bounds.Second
            || x - deviation < bounds.First)
        {
            deviation = Random.Range(bounds.First, bounds.Second);
        }

        if (x + deviation > bounds.Second)
        {
            return x + deviation;
        }
        else
        {
            return x - deviation;
        }
    }
}
