using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player1 : MonoBehaviour, IPlayer {

    float IPlayer.speedX { get; set; }
    float IPlayer.speedY { get; set; }
	const float SPEED = 0.1f;

	Vector3 mouse_pos;
	Transform target;
	Vector3 object_pos;
	float angle;
	public GameObject mouse;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 pos = this.transform.position;
		if (Input.GetKey (KeyCode.A))
			pos.x -= SPEED;
		if (Input.GetKey (KeyCode.D))
			pos.x += SPEED;
		if (Input.GetKey (KeyCode.S))
			pos.y -= SPEED;
		if (Input.GetKey (KeyCode.W))
			pos.y += SPEED;
		this.transform.position = pos;

        /*
		mouse_pos = Input.mousePosition;
		mouse_pos.x = mouse_pos.x - pos.x;
		mouse_pos.y = mouse_pos.y - pos.y;
		angle = Mathf.Atan2(mouse_pos.y, mouse_pos.x) * Mathf.Rad2Deg;
		Quaternion q = Quaternion.Euler(0f,0f,angle);
		transform.rotation = q;
	

		float AngleRad = Mathf.Atan2 (mouse.transform.position.y - this.transform.position.y, mouse.transform.position.x - this.transform.position.x);
		float AngleDeg = (180 / Mathf.PI) * AngleRad;
		this.transform.rotation = Quaternion.Euler (0, 0, AngleDeg);
		*/

        var mouse = Input.mousePosition;
        var screenPoint = Camera.main.WorldToScreenPoint(transform.localPosition);
        var offset = new Vector2(mouse.x - screenPoint.x, mouse.y - screenPoint.y);
        var angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}
