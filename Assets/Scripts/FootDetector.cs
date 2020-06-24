using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootDetector : MonoBehaviour
{
	public Vector3 enterPoint;
	// Start is called before the first frame update
	void Start()
	{
		
	}

	// Update is called once per frame
	void Update()
	{
		
	}

	void OnTriggerEnter(Collider collider)
	{
		if (collider.gameObject.CompareTag("Ground"))
		{
			enterPoint = gameObject.transform.position;
		}
	}

	void OnTriggerExit(Collider collider)
	{
		if (collider.gameObject.CompareTag("Ground"))
		{
			gameObject.transform.root.position = new Vector3((gameObject.transform.root.position.x + (enterPoint.x - gameObject.transform.position.x)), gameObject.transform.root.position.y, (gameObject.transform.root.position.z + (enterPoint.z - gameObject.transform.position.z)));
		}
	}
}
