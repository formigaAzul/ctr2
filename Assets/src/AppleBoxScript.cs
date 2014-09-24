﻿using UnityEngine;
using System.Collections;

public class AppleBoxScript : MonoBehaviour {
	private int nb = 0;
	
	// Use this for initialization
	void Start () {
	}
	
	void OnTriggerEnter(Collider other)
	{
		collider.enabled = false;
		Debug.Log ("collision"+nb);
		nb++;
		audio.Play ();
		
		StartCoroutine (Take());
		StartCoroutine(animPommes());
	}
	
	IEnumerator animPommes()
	{
		yield return new WaitForSeconds (0.1f);
	}
	
	IEnumerator Take()
	{
		animation.Play ("boxDisappear");
		yield return new WaitForSeconds (3f);
		animation.Play ("boxGrow");
		yield return new WaitForSeconds (2f);
		collider.enabled = true;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
