using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

	public GameObject enemyPrefab;
	public float width = 15f;
	public float height = 6f;
	public float spawnDelay = 0.5f;
	private bool movingRight = false;
	private float speed = 5f;
	private float xmax;
	private float xmin;

	// Use this for initialization
	void Start () {
		
		float distanceToCamera = transform.position.z - Camera.main.transform.position.z;
		Vector3 leftBoundary = Camera.main.ViewportToWorldPoint(new Vector3(0,0,distanceToCamera));
		Vector3 rightBoundary = Camera.main.ViewportToWorldPoint(new Vector3(1,0,distanceToCamera));

		xmax = rightBoundary.x;
		xmin = leftBoundary.x;

		SpawnUntilFull();

		
	}


	public void OnDrawGizmos()
	{
		Gizmos.DrawWireCube(transform.position,new Vector3(width,height,0));
	}

	void SpawnUntilFull()
	{
		Transform freePosition = NextFreePosition();
		if (freePosition){
			GameObject Enemy = Instantiate(enemyPrefab,freePosition.transform.position,Quaternion.identity) as GameObject;
			Enemy.transform.parent = freePosition;
		}

		Invoke("SpawnUntilFull", spawnDelay);
		/*if(NextFreePosition())
		{
			Debug.Log(NextFreePosition());
			Invoke("SpawnUntilFull", spawnDelay);
		}*/
	}
	
	// Update is called once per frame
	void Update () {

		if(movingRight)
		{
			transform.position+=Vector3.right * speed * Time.deltaTime;
		}
		else
		{
			transform.position+=Vector3.left * speed * Time.deltaTime;
		}

		 float rightEdgeOfFormation = transform.position.x + (0.5f * width);
		 float leftEdgeOfFormation = transform.position.x - (0.5f * width);
		

		if(leftEdgeOfFormation < xmin)
		{
			movingRight = true;
		} else if (rightEdgeOfFormation > xmax)
		{
			movingRight = false;
		}

		if(AllMembersDead())
		{
			Debug.Log("Empty Formation");
			SpawnUntilFull();
		}
	}

	void SpawnEnemies()
	{
		foreach(Transform child in transform)
		{
			GameObject Enemy = Instantiate(enemyPrefab,child.transform.position,Quaternion.identity) as GameObject;
			Enemy.transform.parent = child;
		}
	}

	Transform NextFreePosition()
	{
		foreach (Transform childPositionGameObject in transform)
		{
			if (childPositionGameObject.childCount == 0)
			{
				return childPositionGameObject;
			}
		}

		return null;
	}
	bool AllMembersDead()
	{
		foreach (Transform childPositionGameObject in transform)
		{
			if (childPositionGameObject.childCount > 0)
			{
				return false;
			}
		}

		return true;
	}
}
