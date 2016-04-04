using UnityEngine;
using System.Collections;

public class GoForGoal : MonoBehaviour {
    [SerializeField]
    Vector3 goal;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        var direction = goal - transform.position;
        GetComponent<CharacterController>().Move(direction.normalized*0.01f);
	}
}
