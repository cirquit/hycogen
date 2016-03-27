using UnityEngine;
using System.Collections;

public class SpawnScript : MonoBehaviour {

	void Start ()
    {
		Vector3 pos = new Vector3 (4.0f, 1.5f, 0.0f);
		Quaternion rot = new Quaternion ();

        GameObject agent = (GameObject) Instantiate(Resources.Load("AgentPrefab"), pos, rot);   

        Agent agentScript  = (Agent) agent.GetComponent<Agent>();

        agentScript.speed  = 1.0f;
        agentScript.pathGA = new PathGA(-1, -10, -2, 100, -1, 10, 3, 2, 0.2f, 0.3f, 1, 0.5f, 0.5f, 0.5f);
	}
}

