using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player1 : MonoBehaviour {

    
	const float SPEED = 0.1f;
	public GameObject bulletPrefab;
	const float BULLET_FORCE = 1000f;
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

        var mouse = Input.mousePosition;
        var screenPoint = Camera.main.WorldToScreenPoint(transform.localPosition);
        var offset = new Vector2(mouse.x - screenPoint.x, mouse.y - screenPoint.y);
        var angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
		if (Input.GetMouseButtonDown(0)){//when the left mouse button is clicked
			FireBullet(offset.normalized);//look for and use the fire bullet operation
    	}
	}

	public void FireBullet(Vector2 v){

		//spawning the bullet at position
		GameObject Clone;
		Clone = (Instantiate(bulletPrefab, transform.position+1f*transform.forward,this.transform.rotation));
		Destroy (Clone, 5f);

		//add force to the spawned objected
		Clone.GetComponent<Rigidbody2D>().AddForce(new Vector2(BULLET_FORCE * v.x, BULLET_FORCE * v.y));
	}
}
