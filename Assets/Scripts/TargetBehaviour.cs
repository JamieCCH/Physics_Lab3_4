using System.Collections;
using System.Collections.Generic;
using UnityEngine;



//this target is the landing spot object (which is the end of the arc)
public class TargetBehaviour : MonoBehaviour {


    [SerializeField] GameObject player;

    ProjectileComponent pc;

	void Start () {
        pc = player.GetComponent<ProjectileComponent>();
    }
	
	
	void Update () {
        this.transform.position = pc.GetTrajectoryEnd();
	}

}
