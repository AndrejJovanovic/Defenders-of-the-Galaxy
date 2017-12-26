using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	//private float xPos = 0.0f;
	public float speed = 0.1f;
	public float padding = 1f;
	public float projectileSpeed;
	public float firingRate = 0.2f;
	public GameObject projectile;
	public float health = 250;
	

	float xmin;
	float xmax;

	// Use this for initialization
	void Start () {

		float distance = transform.position.z - Camera.main.transform.position.z;
		Vector3 leftmost = Camera.main.ViewportToWorldPoint(new Vector3(0,0,distance));
		Vector3 rightmost = Camera.main.ViewportToWorldPoint(new Vector3(1,0,distance));

		xmin = leftmost.x + padding;
		xmax = rightmost.x - padding;
		
	}

	void Fire()
	{
		Vector3 offsetPosition = transform.position + new Vector3(0,1,0);
		GameObject beam = Instantiate(projectile,offsetPosition,Quaternion.identity) as GameObject;
		beam.GetComponent<Rigidbody2D>().velocity = new Vector3(0,projectileSpeed,0);

	}

	
	void OnTriggerEnter2D(Collider2D collider)
	{
		Projectile missile = collider.gameObject.GetComponent<Projectile>();
		if(missile){
			health -= missile.GetDamage();
			missile.Hit();
			if (health<=0)
			{
				Destroy(gameObject);
			}
			Debug.Log("Hit by a projectile");
		}
	}
	
	// Update is called once per frame
	void Update () {

		if(Input.GetKey(KeyCode.LeftArrow))
		{
			transform.position += Vector3.left * speed * Time.deltaTime;
		}

		else if(Input.GetKey(KeyCode.RightArrow))
		{
			transform.position += Vector3.right * speed * Time.deltaTime;
		}

		// restrict the player to the gamespace
		float newX = Mathf.Clamp(transform.position.x,xmin,xmax);
		transform.position = new Vector3(newX,transform.position.y,transform.position.z);

		if(Input.GetKeyDown(KeyCode.Space))
		{
			InvokeRepeating("Fire",0.000001f,firingRate);
		}

		if(Input.GetKeyUp(KeyCode.Space))
		{
			CancelInvoke("Fire");
		}


		// Ovo je dobar nacin kodiranja ali nije po tutorialu pa je zato kom
		//Koristi uvijek deltaTime
		/*if (Input.GetKey(KeyCode.LeftArrow))
		{
			Debug.Log("Left key pressed");
			xPos = xPos - 1;
			Debug.Log(xPos);
			transform.position = new Vector3(xPos,0,0) * speed;
		}
		else if (Input.GetKey(KeyCode.RightArrow))
		{
			Debug.Log("right key pressed");
			xPos = xPos + 1;
			Debug.Log(xPos);
			transform.position = new Vector3(xPos,0,0) * speed;
		}*/
	}
}
