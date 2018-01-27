using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player1 : MonoBehaviour, IHauoliHandler {

	private const int P1 = 0;
	private const int maxDim = 3;

	public float[] mCoor = new float[3];
	public float[] mGyro = new float[3];
	public bool mouse = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 coor = new Vector3 (mCoor [0], mCoor [1], 0);
		// Playing area is +/-7 (x) +-7 (y)
		transform.position = coor;
	}

	public void ReadBufferData(HauoliEventData eventData) {
		for (int i = 0; i < maxDim; i++) {
			mCoor[i] = eventData.mCoor[P1][i];
			mGyro[i] = eventData.mGyro[P1][i];
		}
		mouse = eventData.mouse[P1];
	}
}
