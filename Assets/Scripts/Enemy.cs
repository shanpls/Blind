﻿using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {
	public int hp=15;
	public int atk=10;
	public int def=5;
	
	public int currentHP=15;

	public AudioSource[] sounds;
	public AudioSource hitsound;
	public AudioSource deathsound;

	bool isAlive = true;


	// Use this for initialization
	void Start () {
		int currentHP=15;
		sounds = GetComponents<AudioSource> ();
		hitsound = sounds [0];
		deathsound = sounds[1];
	}

	
	// Update is called once per frame
	void Update () {
	
	}

	public void GetHit(int damage){
		currentHP -= damage;
		if (currentHP <= 0 && isAlive) {
			this.renderer.enabled = false;
			deathsound.Play();
			isAlive= false;
		}
		else {
			hitsound.Play();
		}
	}
}
