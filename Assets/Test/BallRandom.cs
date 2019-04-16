using UnityEngine;
using System.Collections;

public class BallRandom : MonoBehaviour {

	// Use this for initialization
	void Start () {
        var force = 10f;
        GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(-force, force), Random.Range(-force, force), 0), ForceMode.Impulse);
	}
	
	
}
