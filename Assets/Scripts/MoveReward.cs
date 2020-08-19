using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveReward : MonoBehaviour {

	//------TESTING-------------------
	public static float speed = 10.0f;
	public static bool move = false;
	public Transform defTextPos;


	// object reference required to access non-static member "MoveReward.defTextPos"
	public static MoveReward Instance;
		void Awake(){
			Instance = this;

	}

	void Start () {
//		transform.position = Settings.Instance.rewardtext.transform.position;
//		target = Settings.Instance.cross.transform;

		defTextPos.position = Scoring.Instance.rewardtext.transform.position;
	}
	

	void Update () {
//		transform.position = Vector3.MoveTowards(transform.position, target.position, speed*Time.deltaTime);

		if(Scoring.fireDisplay){
		//	rewardtext.transform.position = defTextPos.transform.position;
		
			// calculate direction vector
			Vector3 dir = Settings.Instance.cross.transform.position - Scoring.Instance.rewardtext.transform.position;

			// normalize resultant vector to unit vector
			dir = dir.normalized;

			// continuously move in direction of dir vector (to target)
			Scoring.Instance.rewardtext.transform.position += dir * speed * Time.deltaTime;

			if(Scoring.Instance.rewardtext.transform.position.x < 0.1f)
			{
				// hide reward during break
				Debug.Log("inside hide reward during break");
				Scoring.Instance.rewardtext.SetActive(false);
			}

	}
		//----------------------------------------------------------
		//cross position = (3.5f, 97.5f, 0f)
	}
}
