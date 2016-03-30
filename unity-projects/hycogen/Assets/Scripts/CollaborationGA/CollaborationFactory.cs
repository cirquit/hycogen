using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CollaborationFactory
{
    public int popSize;

    public CollaborationFactory(int popSize)
    {
        this.popSize = popSize;
    }

    public Collaboration[] GenCollaborations()
    {
        Collaboration[] collaborations = new Collaboration[popSize];

        for (int i = 0; i < popSize; i++)
        {
            collaborations[i] = GenCollaboration();
        }

        return collaborations;
    }

    // @TODO encode the amount of agents
    public Collaboration GenCollaboration(int agentSCount = 3)
    {
        return new Collaboration(agentSCount); 
    }

    public Collaboration GenCustomCollaboration(List<AgentSettings> agentSList)
    {
        return new Collaboration(agentSList);
    }
}
