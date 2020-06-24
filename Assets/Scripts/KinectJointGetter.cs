using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Valve.VR;
using Windows.Kinect;

public class KinectJointGetter : MonoBehaviour
{
	
	KinectSensor sensor;
	BodyFrameReader bodyFrame;

	public List<GameObject> skele = new List<GameObject>();

	public Vector3 offset = new Vector3(0, 0, 0);

	IList<Body> users;

	public int bodyID;



	// Start is called before the first frame update
	void Start()
	{
		//connect to Kinect 
		sensor = KinectSensor.GetDefault();

		if(sensor != null)
		{
			//start Kinect
			sensor.Open();

			//prepare for receiving Kinect body data
			bodyFrame = sensor.BodyFrameSource.OpenReader();

			//init list of all visible Bodies
			users = new List<Body>();



		}
		else
		{
			Debug.Log("No Kinect Connected!");
		}
	}

	// Update is called once per frame
	void FixedUpdate()
	{
	   ///*
		//check if body data is being captured
		if(bodyFrame != null)
		{
			//get current frame fron the Kinect for use
			using (var frame = bodyFrame.AcquireLatestFrame())
			{
				if(frame != null)
				{
					users = new Body[frame.BodyFrameSource.BodyCount];
					//get Body data from frame and apply to user bodies
					frame.GetAndRefreshBodyData(users);
					foreach(GameObject g in skele)
					{
						
						//get joint orientation and apply it to object
						JointOrientation orientation = users[bodyID].JointOrientations[(JointType)skele.IndexOf(g)];
						Quaternion quat = new Quaternion(orientation.Orientation.X, orientation.Orientation.Y, orientation.Orientation.Z, orientation.Orientation.W);
						g.GetComponent<Transform>().rotation = quat;

						//get joint position and apply it to object
						Windows.Kinect.Joint pos = users[bodyID].Joints[(JointType)skele.IndexOf(g)];
						Vector3 vecPos = new Vector3(pos.Position.X, pos.Position.Y, -pos.Position.Z + 0.1f);
						g.GetComponent<Transform>().position = (gameObject.transform.position + vecPos + offset);
					}
					//align Kinect skeleton with hmd position
					GameObject headJoint = skele[3];
					GameObject hmd = GameObject.FindGameObjectWithTag("MainCamera");
					Vector3 headHMDOffset = -headJoint.transform.position + hmd.transform.position;
					offset += headHMDOffset;
				}
			}
		}				
	}
	// example of getting joint angle data
	//JointOrientation orientation = users[0].JointOrientations[0];
	//Quaternion quaternion = new Quaternion(orientation.Orientation.X, orientation.Orientation.Y, orientation.Orientation.Z, orientation.Orientation.W);
	//quaternion.eulerAngles;
	void OnDestroy()
	{
		sensor.Close();
		bodyFrame.Dispose();
	}
}
