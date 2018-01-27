using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2 : MonoBehaviour, IHauoliHandler {

	private const int P2 = 0;
	private const int maxDim = 3;

	public float[] mCoor = new float[3];
	public float[] mGyro = new float[3];
	public bool mouse = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void ReadBufferData(HauoliEventData eventData) {
		for (int i = 0; i < maxDim; i++) {
			mCoor[i] = eventData.mCoor[P2][i];
			mGyro[i] = eventData.mGyro[P2][i];
		}
		mouse = eventData.mouse[P2];
	}
}
