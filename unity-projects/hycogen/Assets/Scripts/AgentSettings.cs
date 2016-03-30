using UnityEngine;
using System.Collections;

/*
 * This is a simple wrapper to access easibly the AgentPrefab and set its parameters
 **/

public class AgentSettings
{
    public PathGA pathGA = null;
    public float  speed  = 0;

    public AgentSettings(PathGA pathGA, float speed)
    {
        this.pathGA = pathGA;
        this.speed  = speed;
        if (this.speed <= 0.0f)
            Debug.Log("AgentSettings.cs: speed is " + this.speed.ToString("F2") + ", this agent will not move");
    }

    // this will create a moving Agent at the preferred position with the predefined GA and speed
    public void Initialize(Vector3 position)
    {
        if (pathGA == null)
        {
            Debug.Log("AgentSettings.cs: Initialize - pathGA is null, this should never happen");
        }
        else
        {
            Quaternion rot    = new Quaternion ();
            GameObject agent  = (GameObject) MonoBehaviour.Instantiate(Resources.Load("AgentPrefab"), position, rot);   
            Agent agentScript = (Agent) agent.GetComponent<Agent>();

            agentScript.pathGA = this.pathGA;
            agentScript.speed  = this.speed;
        }
    }


    override public string ToString()
    {
        if (pathGA == null)
        {
            Debug.Log("AgentSettings.cs: pathGA is null, this should never happen");
            return "<null>";
        }
        else
        {
            return pathGA.ToString() + ", speed:" + this.speed; 
        }
    }
}
