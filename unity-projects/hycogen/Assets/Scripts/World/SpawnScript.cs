using UnityEngine;
using System.Collections;

public class SpawnScript : MonoBehaviour {

	void Start ()
    {
		Vector3    pos = new Vector3 (3.0f, 1.5f, 0.0f);
		Quaternion rot = new Quaternion ();

        GameObject agent = (GameObject) Instantiate(Resources.Load("AgentPrefab"), pos, rot);   

        Agent agentScript  = (Agent) agent.GetComponent<Agent>();

        agentScript.speed  = 1.0f;
        agentScript.pathGA = new PathGA(
                -1   // wallCollision
              , -10  // riverCollision
              , -2   // agentCollision
              , 100  // targetCollision
              , -1   // agentPathCollision
              , 10   // popSize
              , 3    // subPathCount
              , 2.0f // subPathLength
              , 5    // generationCount
              , 0.2f // alpha (NS - how many should be new)
              , 0.3f // beta  (NS - how many new children)
              , 1    // mode  (CO)
              , 0.5f // gamma (MU - how many should be mutated)
              , 0.5f // delta (MU - how much should be mutated in the individual)
              , 0.5f // maxDeviation (MU)
             );
	}
}

