﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class KartController : MonoBehaviour
{
	public static Dictionary <int, Dictionary<string, KeyCode>> playersMapping;
	public static List<bool> controllerEnabled;

	public bool j1enabled=false;
	public float coeffVitesse=2f;
	public float coeffManiabilite=4f;
	
	public Dictionary <string, KeyCode> keyMap;
	private Kart kart;
	private bool dansLesAirs = true;
	private Dictionary <string, string> axisMap;
	private float ky;

	
	// Use this for initialization
	void Start ()
	{
		if (controllerEnabled == null)
		{
			controllerEnabled = new List<bool>();
			controllerEnabled.Add(j1enabled);
			controllerEnabled.Add(false);
			controllerEnabled.Add(false);
			controllerEnabled.Add(false);
		}
		if (playersMapping == null)
			InitMapping ();
		InitSelfMapping ();
	}
	
	void InitSelfMapping()
	{
		Dictionary <string, string> ps1_axis = new Dictionary<string, string> {
			{"turn","J1_TurnAxis"}, {"stop","J1_StopAxis"}		};
		Dictionary <string, string> ps2_axis = new Dictionary<string, string> {
			{"turn","J2_TurnAxis"}, {"stop","J2_StopAxis"}		};
		Dictionary <string, string> ps3_axis = new Dictionary<string, string> {
			{"turn","J3_TurnAxis"}, {"stop","J3_StopAxis"}		};
		Dictionary <string, string> ps4_axis = new Dictionary<string, string> {
			{"turn","J4_TurnAxis"}, {"stop","J4_StopAxis"}		};
		List<Dictionary <string, string> > l_axis = new List<Dictionary<string, string>> {
			ps1_axis,ps2_axis,ps3_axis,ps4_axis	};

		if (controllerEnabled [kart.numeroJoueur-1])
			axisMap = l_axis[kart.numeroJoueur-1];

		keyMap = playersMapping [kart.numeroJoueur];
	}
	
	void Update()
	{
		//limiter les rotations a 60 degrés
		float limitz1 = 0f;
		float limitz2 = -limitz1;
		/*if(transform.localEulerAngles.x > limit)
			transform.localEulerAngles = new Vector3(limit, transform.localEulerAngles.y, transform.localEulerAngles.z);
		else if(transform.localEulerAngles.x < -limit)
			transform.localEulerAngles = new Vector3(-limit, transform.localEulerAngles.y, transform.localEulerAngles.z);*/
		if(rigidbody.transform.localEulerAngles.z > limitz1)
			rigidbody.transform.localEulerAngles = new Vector3(rigidbody.transform.localEulerAngles.x, rigidbody.transform.localEulerAngles.y, limitz1);
		else if(rigidbody.transform.localEulerAngles.z < limitz2)
			rigidbody.transform.localEulerAngles = new Vector3(rigidbody.transform.localEulerAngles.x, rigidbody.transform.localEulerAngles.y, limitz2);


		if (dansLesAirs)
			rigidbody.velocity = new Vector3(rigidbody.velocity.x,-9.81f,rigidbody.velocity.z);
		/*
		else
			rigidbody.velocity = new Vector3(rigidbody.velocity.x,rigidbody.velocity.y/2,rigidbody.velocity.z);
			*/

		// INDISPENSABLE : annule la possibilité de CONTROLER la rotation z
		rigidbody.angularVelocity = Vector3.zero;

	}
	
	// Update is called once per frame
	void FixedUpdate ()
	{
		if (keyMap == null)
			return;
		if (keyMap ["moveForward"] == KeyCode.Joystick3Button2 && !controllerEnabled [2])
			return;
		if (keyMap ["moveForward"] == KeyCode.Joystick4Button2 && !controllerEnabled [3])
			return;

		controlPosition ();
	}
	
	
	void OnCollisionStay(Collision collision)
	{
		if(collision.gameObject.name=="Ground")
			dansLesAirs = false;

		if(collision.gameObject.name=="accelerateur")
			rigidbody.velocity = new Vector3(rigidbody.velocity.x*3,rigidbody.velocity.y*3,rigidbody.velocity.z*3);

	}
	
	void OnCollisionEnter(Collision collision)
	{
		//rigidbody.velocity = Vector3.zero;
		//rigidbody.angularVelocity = Vector3.zero;
	}
	
	void OnCollisionExit(Collision collision)
	{
		if(collision.gameObject.name=="Ground")
		{
			dansLesAirs = true;
		}
	}
	
	public void SetKart (Kart k)
	{
		kart = k;
	}
	
	public Vector3 normalizeVector(Vector3 a)
	{
		float div = Mathf.Sqrt (a.x * a.x + a.y * a.y + a.z * a.z);
		a.x /= div;
		a.y /= div;
		a.z /= div;
		return a;
	}
	
	public void controlPosition()
	{
		Vector3 forwardNormal = rigidbody.transform.forward;
		forwardNormal.y = 0;
		forwardNormal = normalizeVector (forwardNormal);
		if(Input.GetKey(keyMap["moveBack"]))
		{
			if (!controllerEnabled[kart.numeroJoueur-1])
				rigidbody.position+=forwardNormal/4*coeffVitesse;
			if (controllerEnabled[kart.numeroJoueur-1])
				transform.Rotate(0,Input.GetAxis(axisMap["turn"])*coeffManiabilite,0);
			else
			{
				if(Input.GetKey(keyMap["turnLeft"]))
					transform.Rotate(0,-0.5f*coeffManiabilite,0);
				if(Input.GetKey(keyMap["turnRight"]))
					transform.Rotate(0,0.5f*coeffManiabilite,0);
			}
		}
		if(Input.GetKey(keyMap["moveForward"]))
		{
			rigidbody.position-=forwardNormal/4*coeffVitesse;
			if (controllerEnabled[kart.numeroJoueur-1])
				transform.Rotate(0,Input.GetAxis(axisMap["turn"])*coeffManiabilite,0);
			else
			{
				if(Input.GetKey(keyMap["turnLeft"]))
					transform.Rotate(0,0.5f*coeffManiabilite,0);
				if(Input.GetKey(keyMap["turnRight"]))
					transform.Rotate(0,-0.5f*coeffManiabilite,0);
			}
		}
		if(Input.GetKeyUp(keyMap["jump"]))
		{
			if(dansLesAirs==false)
			{
				rigidbody.AddForce(0,600000,0);
			}
		}
		if (controllerEnabled [kart.numeroJoueur - 1] && Input.GetAxis (axisMap ["stop"]) > 0) {
			rigidbody.position += Input.GetAxis (axisMap ["stop"]) * forwardNormal / 4 * coeffVitesse;
			transform.Rotate (0, -Input.GetAxis (axisMap ["turn"]) * coeffManiabilite, 0);
		}
	}

	void InitMapping()
	{
		// local variables : to be destroid at the end of function = memory'll be free.
		Dictionary <string, KeyCode> pc1 = new Dictionary<string, KeyCode> {
			{"moveForward",KeyCode.Z}, {"moveBack",KeyCode.S},
			{"turnRight",KeyCode.Q}, {"turnLeft",KeyCode.D},
			{"jump",KeyCode.Space}, {"action",KeyCode.A},
			{"start",KeyCode.Escape}, {"viewChange",KeyCode.F1},
			{"bip",KeyCode.F2}
		};
		Dictionary <string, KeyCode> pc2 = new Dictionary<string, KeyCode> {
			{"moveForward",KeyCode.I}, {"moveBack",KeyCode.K},
			{"turnRight",KeyCode.J}, {"turnLeft",KeyCode.L},
			{"jump",KeyCode.B}, {"action",KeyCode.U}
		};
		Dictionary <string, KeyCode> ps1 = new Dictionary<string, KeyCode> {
			{"moveForward",KeyCode.Joystick1Button2}, {"moveBack",KeyCode.Joystick1Button3},
			{"jump",KeyCode.Joystick1Button7}, {"jump2",KeyCode.Joystick1Button6},
			{"action",KeyCode.Joystick1Button1},{"start",KeyCode.Joystick1Button9},
			{"viewChange",KeyCode.Joystick1Button4}, {"viewInverse",KeyCode.Joystick1Button5},
			{"bip",KeyCode.Joystick1Button10}, {"bip2",KeyCode.Joystick1Button11}
		};
		Dictionary <string, KeyCode> ps2 = new Dictionary<string, KeyCode> {
			{"moveForward",KeyCode.Joystick2Button2}, {"moveBack",KeyCode.Joystick2Button3},
			{"jump",KeyCode.Joystick2Button7}, {"jump2",KeyCode.Joystick2Button6},
			{"action",KeyCode.Joystick2Button1},{"start",KeyCode.Joystick2Button9},
			{"viewChange",KeyCode.Joystick2Button4}, {"viewInverse",KeyCode.Joystick2Button5},
			{"bip",KeyCode.Joystick2Button10}, {"bip2",KeyCode.Joystick2Button11}
		};
		Dictionary <string, KeyCode> ps3 = new Dictionary<string, KeyCode> {
			{"moveForward",KeyCode.Joystick3Button2}, {"moveBack",KeyCode.Joystick3Button3},
			{"jump",KeyCode.Joystick3Button7}, {"jump2",KeyCode.Joystick3Button6},
			{"action",KeyCode.Joystick3Button1},{"start",KeyCode.Joystick3Button9},
			{"viewChange",KeyCode.Joystick3Button4}, {"viewInverse",KeyCode.Joystick3Button5},
			{"bip",KeyCode.Joystick3Button10}, {"bip2",KeyCode.Joystick3Button11}
		};
		Dictionary <string, KeyCode> ps4 = new Dictionary<string, KeyCode> {
			{"moveForward",KeyCode.Joystick4Button2}, {"moveBack",KeyCode.Joystick4Button3},
			{"jump",KeyCode.Joystick4Button7}, {"jump2",KeyCode.Joystick4Button6},
			{"action",KeyCode.Joystick4Button1},{"start",KeyCode.Joystick4Button9},
			{"viewChange",KeyCode.Joystick4Button4}, {"viewInverse",KeyCode.Joystick4Button5},
			{"bip",KeyCode.Joystick4Button10}, {"bip2",KeyCode.Joystick4Button11}
		};
		
		if (controllerEnabled [0])
			pc1 = ps1;
		if (controllerEnabled [1])
			pc2 = ps2;

		playersMapping = new Dictionary<int, Dictionary<string, KeyCode>> {{1,pc1},{2,pc2},{3,ps3},{4,ps4}};
	}
	
}
