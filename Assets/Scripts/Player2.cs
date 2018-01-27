using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2 : MonoBehaviour {

	const float SPEED = 0.1f;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
		Vector3 pos = this.transform.position;
		if (Input.GetAxis ("LeftStickH") > 0) {
			pos.x += SPEED;
		} 
		if (Input.GetAxis ("LeftStickH") < 0){
			pos.x -= SPEED;
		}
		if (Input.GetAxis ("LeftStickV") > 0) {
			pos.y += SPEED;
		} 
		if (Input.GetAxis ("LeftStickV") < 0){
			pos.y -= SPEED;
		}




		this.transform.position = pos;	

		var mouse = Input.mousePosition;
		var screenPoint = Camera.main.WorldToScreenPoint(transform.localPosition);
		var offset = new Vector2(mouse.x - screenPoint.x, mouse.y - screenPoint.y);
		var angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.Euler(0, 0, angle);
	}
}
