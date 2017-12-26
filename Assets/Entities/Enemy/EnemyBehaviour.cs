using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour {

	public float health = 150f;
	public float projectileSpeed;
	public GameObject enemyProjectile;
	public float shootsPerSecond = 0.5f;
	public int scoreValue = 100;
	private ScoreKeeper scoreKeeper;

	
	void Start()
	{
		scoreKeeper = GameObject.Find("Score").GetComponent<ScoreKeeper>();
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
				scoreKeeper.Score(scoreValue);
			}
			Debug.Log("Hit by a projectile");
		}
	}

	void Fire()
	{
		Vector3 offsetPosition = transform.position + new Vector3(0,-1,0);
		GameObject missile = Instantiate(enemyProjectile,offsetPosition,Quaternion.identity) as GameObject;
		missile.GetComponent<Rigidbody2D>().velocity = new Vector3(0,-projectileSpeed,0);
	}

	void Update()
	{
		float probability = shootsPerSecond * Time.deltaTime;
		if(Random.value < probability){
			Fire();
		}
	}


}
