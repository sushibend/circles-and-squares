using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2 : MonoBehaviour {

	const float SPEED = 0.1f;
	public GameObject bulletPrefab;
	const float BULLET_FORCE = 1000f;

	public enum Gun { MAGNET, OTHER }
	public Gun gun = Gun.MAGNET;
	public GameObject gunObject;
	public GameObject body;

    // Animation/Image
    public Sprite idle;
    public Sprite walking;
    public Sprite hit;
    Sprite temp;

    // Hit status
    int hitFrames = 30;
    bool hitStatus = false;

    Vector2 offset = new Vector2(1f,0);

	// Use this for initialization
	void Start () {
        transform.localScale = new Vector3(.2f, .2f, 0);
    }

	// Update is called once per frame
	void Update () {
		
        if (hitStatus == true && hitFrames == 30)
        {
            temp = GetComponent<SpriteRenderer>().sprite;
            GetComponent<SpriteRenderer>().sprite = hit;
        }
        if (hitStatus == true)
            hitFrames--;
        if (hitStatus == true && hitFrames == 0)
        {
            hitFrames = 30;
            hitStatus = false;
            GetComponent<SpriteRenderer>().sprite = temp;
        }

        if ((Input.GetAxis("LeftStickH") != 0 || Input.GetAxis("LeftStickH") != 0) &&
            GetComponent<SpriteRenderer>().sprite != walking)
        {
            GetComponent<SpriteRenderer>().sprite = walking;
        }

        if ((Input.GetAxis("LeftStickH") == 0 || Input.GetAxis("LeftStickH") == 0) &&
            GetComponent<SpriteRenderer>().sprite != idle)
        {
            GetComponent<SpriteRenderer>().sprite = idle;
        }

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
	
		if (Input.GetAxis ("RightStickH") != 0 || Input.GetAxis ("RightStickV") != 0) {
			offset = new Vector2 (Input.GetAxis ("RightStickH"), Input.GetAxis ("RightStickV"));
			var angle = Mathf.Atan2 (offset.y, offset.x) * Mathf.Rad2Deg;
			transform.rotation = Quaternion.Euler (0, 0, angle);
			body.transform.rotation = Quaternion.Euler (0, 0, 0);
		}

		if (Input.GetButtonDown("Fire1") && !Input.GetMouseButtonDown(0)){//when the left mouse button is clicked
			FireBullet(offset.normalized);//look for and use the fire bullet operation
			this.GetComponent<AudioSource>().Play();
		}
	}

	public void FireBullet(Vector2 v){
		//spawning the bullet at position
		GameObject Clone;
		Clone = (Instantiate(bulletPrefab, gunObject.transform.position+1f*transform.forward,this.transform.rotation));
		Destroy (Clone, 5f);

		//add force to the spawned objected
		Clone.GetComponent<Rigidbody2D>().AddForce(new Vector2(BULLET_FORCE * v.x, BULLET_FORCE * v.y));
	}

	void OnCollisionEnter2D (Collision2D col)
	{
		if(col.gameObject.tag == "bullet")
		{
			Destroy(col.gameObject);
			Destroy (this.gameObject);

			Debug.Log ("p2 hit");
		}
	}

}
