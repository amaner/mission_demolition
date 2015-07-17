using UnityEngine;
using System.Collections;

public class CloudCrafter : MonoBehaviour {

	// fields set in the Inspector pane
	public int numClouds = 40;			// the # of clouds to make
	public GameObject[] cloudPrefabs;   // the prefabs for the clouds
	public Vector3 cloudPosMin;			// min position of each cloud
	public Vector3 cloudPosMax;			// max position of each cloud
	public float cloudScaleMin = 1;		// min scale of each cloud
	public float cloudScaleMax = 5;		// max scale of each cloud
	public float cloudSpeedMult = 0.5f; // adjusts speed of clouds

	public bool __________; // separator

	// fields set dynamically
	public GameObject[] cloudInstances; // actual instances of clouds

	// Awake runs before Start
	void Awake() {
		// make an array large enough to hold all Cloud_# instances
		cloudInstances = new GameObject[numClouds];
		// find the CloudAnchor object
		GameObject anchor = GameObject.Find ("CloudAnchor");
		// iterate through and make Cloud_s
		GameObject cloud;
		for (int i=0; i<numClouds; i++) {
			// pick an int between 0 and cloudPrefabs.Length-1
			// Random.Range will not ever pick as high as the top number
			int prefabNum = Random.Range(0, cloudPrefabs.Length);
			// make an instance
			cloud = Instantiate(cloudPrefabs[prefabNum]) as GameObject;
			// position cloud
			Vector3 cPos = Vector3.zero;
			cPos.x = Random.Range (cloudPosMin.x,cloudPosMax.x);
			cPos.y = Random.Range (cloudPosMin.y,cloudPosMax.y);
			// scale the cloud
			float scaleU = Random.value;
			float scaleVal = Mathf.Lerp (cloudScaleMin, cloudScaleMax, scaleU);
			// smaller clouds with smaller scaleU should be nearer the ground
			cPos.y = Mathf.Lerp(cloudPosMin.y, cPos.y, scaleU);
			// smaller clouds should be further away
			cPos.z = 100 - 90*scaleU;
			// apply these to the cloud
			cloud.transform.position = cPos;
			cloud.transform.localScale = Vector3.one * scaleVal;
			// make cloud a child of the anchor
			cloud.transform.parent = anchor.transform;
			// add the cloud to cloudInstances
			cloudInstances[i] = cloud;

		}

	}
	// Update runs every frame
	void Update() {
		// iterate over each cloud that was created
		foreach (GameObject cloud in cloudInstances) {
			// get the cloud scale and position
			float scaleVal = cloud.transform.localScale.x;
			Vector3 cPos = cloud.transform.position;
			// move larger clouds faster
			cPos.x -= scaleVal * Time.deltaTime * cloudSpeedMult;
			// if a cloud has moved too far to the left...
			if (cPos.x <= cloudPosMin.x) {
				// move it to the far right
				cPos.x = cloudPosMax.x;
			}
			// apply the new position to the cloud
			cloud.transform.position = cPos;
		}
	}
}
