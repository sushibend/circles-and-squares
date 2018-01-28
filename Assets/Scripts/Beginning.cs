using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beginning : MonoBehaviour {

	// Use this for initialization
	public void changeMenu (string name){
		Application.LoadLevel (name);
	}
}
