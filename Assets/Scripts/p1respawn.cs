using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class p1respawn : MonoBehaviour {
	public GameObject player1;
	public GameObject originalPrefab;
	bool respawning = false;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if (player1 == null && !respawning) {
			StartCoroutine (respawn());
		}

	}

	IEnumerator respawn() {
		respawning = true;
		Debug.Log ("Respawning p1");
		yield return new WaitForSeconds (2);
		respawning = false;
		player1 = Instantiate (originalPrefab);
	}
}
