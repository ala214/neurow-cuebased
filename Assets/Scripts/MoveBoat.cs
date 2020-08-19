using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class MoveBoat : MonoBehaviour {

	public static float boatspeed = 5f;//default = 5
	public static float turnspeed = 10f;
	public static float stoppingAngle = 45f;

	// scoring
	public static int countR = 0;
	public static int countL = 0;
	public static int countW = 0;

	public Transform compass;
	public GameObject EndofSessionPanel;

	public Image crossui, leftarrow, rightarrow;
	public GameObject RewardText;
	public static bool case800, case786, cross, left, right, hidearrow = false;
	public static bool training;


	// Use this for initialization
	void Start () {

		Cursor.visible = true;
		//settings
		EndofSessionPanel.SetActive(false);
		Settings.reverseHands = true;
		boatspeed = 15f;

		crossui.enabled = false;
		leftarrow.enabled = false;
		rightarrow.enabled = false;

		RewardText = GameObject.Find("RewardText");
		RewardText.SetActive(false);


		// check scene name to enable training
		if (SceneManager.GetActiveScene().name == "boat_online") 
			training = false;
		else
			training = true;

		Debug.Log ("scene: " + SceneManager.GetActiveScene().name);
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKey("escape"))
		{
			Debug.Log ("Quit!");
			Application.Quit();
		}

		//Debug.Log (Assets.LSL4Unity.Scripts.Examples.ExampleFloatInlet.signal);

		// if simple ERD is selected, then only FWD, else FWD is automatic and L/R is through LDA input
		//if(Input.GetKey(KeyCode.UpArrow))

		//-------------------------------------------------------------------------------
		// COULD BE WHY BOAT MOVES IN BEGINNING OF GAME???????
		transform.Translate(Vector3.forward * boatspeed * Time.deltaTime);
		//-------------------------------------------------------------------------------

		//if((compass.localEulerAngles.y >= (360 - stoppingAngle) && compass.localEulerAngles.y <= 360) || (compass.localEulerAngles.y >= 0 && compass.localEulerAngles.y <= (0 + stoppingAngle)))
		//	transform.Translate(Vector3.forward * boatspeed * Time.deltaTime);

		if (Receivemarkers.markerint == 1010) //32770 experiment stop
			EndofSessionPanel.SetActive(true); //pop window
			//Debug.Log ("End of Session!");


		//Todo
		// on 32770 experiment stop
		// quit app
		if (Receivemarkers.markerint == 32770) //32770
			Debug.Log ("32770 experiment stop");
		


		if (training) {
			Debug.Log("inside MoveBoat training");

			getStim ();
			
			if (Input.GetKey (KeyCode.LeftArrow) || (left  && hidearrow)) {
				left = true;
				//			right = false;
				if (!Settings.reverseHands)
					transform.Rotate (Vector3.up * turnspeed * Time.deltaTime, Space.World);
				else
					transform.Rotate (Vector3.down * turnspeed * Time.deltaTime, Space.World);
				//	transform.Translate(Vector3.forward * boatspeed * Time.deltaTime);
			}
			if (Input.GetKey (KeyCode.RightArrow) || (right && hidearrow)) {
				//			left = false;
				right = true;
				if (!Settings.reverseHands)
					transform.Rotate (Vector3.down * turnspeed * Time.deltaTime, Space.World);
				else
					transform.Rotate (Vector3.up * turnspeed * Time.deltaTime, Space.World);
				//	transform.Translate(Vector3.forward * boatspeed * Time.deltaTime);
			}

			if (Input.GetKeyUp (KeyCode.LeftArrow))
				left = false;

			if (Input.GetKeyUp (KeyCode.RightArrow))
				right = false;

		} 

		else { // if Online
			Debug.Log("inside MoveBoat online"); 

			getStimOnline ();

//			if (left && hidearrow == true)
//				left = true;
//			else left = false;

			if (Input.GetKey (KeyCode.LeftArrow) ||(left && hidearrow)&& ldaSignal()>=0) {
				//left = true;
				Debug.Log("inside left arrow");
				countL += 1;	// scoring - left rows
				//			right = false;
				if(!Settings.reverseHands)
					transform.Rotate (Vector3.up * turnspeed * Time.deltaTime, Space.World);
				else
					transform.Rotate (Vector3.down * turnspeed * Time.deltaTime, Space.World);
				//	transform.Translate(Vector3.forward * boatspeed * Time.deltaTime);
			}
			else if (Input.GetKey (KeyCode.RightArrow) ||(right && hidearrow) && ldaSignal()<=0) {
				//			left = false;

				right = true;
				Debug.Log("inside right arrow");
				countR += 1;	// scoring - right rows
				if(!Settings.reverseHands)
					transform.Rotate (Vector3.down * turnspeed * Time.deltaTime, Space.World);
				else
					transform.Rotate (Vector3.up * turnspeed * Time.deltaTime, Space.World);
				//	transform.Translate(Vector3.forward * boatspeed * Time.deltaTime);
			}
			else {
				if(hidearrow)
					countW += 1;
			}

			if(Input.GetKeyUp(KeyCode.LeftArrow)){
				left = false;
			}

			if(Input.GetKeyUp(KeyCode.RightArrow)){
				right = false;
			}

		}

		// SSVEP FLASHING ARROWS (30 - 40 Hz) -- 30 = left, 40 = right (?)
//		if(leftarrow.enabled == true){
//			for(int mi = 0; mi < 30; mi++){
//				leftarrow.enabled = false;
//				leftarrow.enabled = true;
//			}			
//		}


//		if(rightarrow.enabled == true){
//			for(int mi = 0; mi < 40; mi++){
//				rightarrow.enabled = false;
//			}
//			rightarrow.enabled = true;
//		}


	}


	void getStim()
	{
		int stim = Receivemarkers.markerint;

		switch (stim)
		{
		case 800: //hide cross
			crossui.enabled = false;
			leftarrow.enabled = false;
			rightarrow.enabled = false;
			cross= false; 
			left = false;
			right = false;
			hidearrow = false;
			break;
		case 786: // show cross
			crossui.enabled = true;
			leftarrow.enabled = false;
			rightarrow.enabled = false;
			cross = true;
			left = false;
			right = false;
			hidearrow = false;
			break;
		case 770: // right arrow
			crossui.enabled = true;
			leftarrow.enabled = false;
			rightarrow.enabled = true;
			cross= true;
			left = false;
			right = true;
			hidearrow = false;
			break;
		case 769: // left arrow
			crossui.enabled = true;
			leftarrow.enabled = true;
			rightarrow.enabled = false;
			cross= true; 
			left = true;
			right = false;
			hidearrow = false;
			break;
		case 781: // hide arrow
			crossui.enabled = true;
			leftarrow.enabled = false;
			rightarrow.enabled = false;
			cross= true;
			hidearrow = true;
			//left= false;
			//right= false;
			break;
		}
	}

	public static float ldaSignal(){
		return Assets.LSL4Unity.Scripts.Examples.ExampleFloatInlet.signal;
	}


	void getStimOnline()
	{
		int stim = Receivemarkers.markerint;

		switch (stim)
		{
		case 800: //hide cross
			crossui.enabled = false;
			leftarrow.enabled = false;
			rightarrow.enabled = false;
			cross= false;
			left = false;
			right = false;
			hidearrow = false;
			case800 = true;
			case786 = false;
			break;
		case 786: // show cross
			crossui.enabled = true;
			leftarrow.enabled = false;
			rightarrow.enabled = false;
			cross = true;
			left = false;
			right = false;
			hidearrow = false;
			case800 = false;
			case786 = true;
			break;
		case 770: // right arrow
			crossui.enabled = true;
			leftarrow.enabled = false;
			rightarrow.enabled = true;
			cross= true;
			left = false;
			right = true;
			hidearrow = false;
			case800 = false;
			case786 = false;
			break;
		case 769: // left arrow
			crossui.enabled = true;
			leftarrow.enabled = true;
			rightarrow.enabled = false;
			cross= true;
			left = true;
			right = false;
			hidearrow = false;
			case800 = false;
			case786 = false;
			break;
		case 781: // hide arrow
			crossui.enabled = true;
			leftarrow.enabled = false;
			rightarrow.enabled = false;
			cross= true;
			hidearrow = true;
			case800 = false;
			case786 = false;
			break;
			//		default:
			//			cross.enabled = false;
			//			leftarrow.enabled = false;
			//			rightarrow.enabled = false;
			//			break;
		}
	}

}
