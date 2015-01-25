﻿using UnityEngine;
using System.Collections;

public class PlayerStats : MonoBehaviour {

	public bool isAlive = true;
	public bool isWaiting = false;
	public bool isVictorious = false;
	public bool isPressed = false;

	public static int DieCount = 1;
	public static int DiceSize = 6;

	public static int hp=100;
	public static int mp=100;
	public static int atk=10;
	public static int def=5;
	public int damageCount;
	
	public static int currentHP=100;
	public static int currentMP=100;
	public static float dmgMult=0;

	public static int level=1;


	public GameObject enemyObject;
	Enemy enemy;

	public AudioSource[] sounds;
	public AudioSource hitsound;
	public AudioSource deathsound;
	
	// Use this for initialization
	void Start () {
		sounds = GetComponents<AudioSource> ();
		hitsound = sounds [0];
		deathsound = sounds[1];

		enemy = enemyObject.GetComponent<Enemy>();
	}
	
	// Update is called once per frame
	void Update () {

		if (!enemy.isAlive) {
			isVictorious = true;
		}
	
	}

	public void Roll(){
		if(!isWaiting){
			damageCount = 0;
			for (int diceNumber = 1; diceNumber <= DieCount; diceNumber++) {
				int roll = Random.Range (1, DiceSize + 1);
				damageCount += roll;
			}
		}

	}

	public void Attack(){
		if (!isWaiting && isAlive && !isPressed) {
			isPressed = true;
			Roll ();
			int damage = damageCount;
			damage*=(int)(1.0f+dmgMult);
			Debug.Log (string.Format ("HIT FOR {0}", damage));
			enemy.GetHit (damage);
			isWaiting = true;
			enemy.isWaiting = false;
		}
	}

	public void Escape(){
		if (!isWaiting && isAlive && !isPressed) {
			isPressed = true;
			Roll ();
			int escapism = damageCount;
			Debug.Log (string.Format ("ESCAPE CHECK: {0}", damageCount));
			if (escapism > enemy.atk){
				Debug.Log ("ESCAPE SUCCESS");
				Debug.Log ("Application.LoadLevel (0)");
			}
			else if(isAlive){
				Debug.Log ("COULDN'T ESCAPE");
				isWaiting = true;
				enemy.isWaiting = false;
			}

		}
	}


	public void GetHit(int damage){
		currentHP -= damage;
		if (currentHP <= 0 && isAlive) {
			deathsound.Play();
			isAlive = false;
			Debug.Log ("GAME OVER");
			Debug.Log ("ur bad");
		}
		else if(isAlive){
			hitsound.Play();
		}
	}

	//Lets put skills here :v
	public void LesserHeal(){
		if (!isWaiting && mp>=10) {
			mp-=10;
			int basePower = Random.Range(1,51);
			if (currentHP + basePower >= hp) {
				currentHP=hp;
				Debug.Log ("I'm really feeling it!");
			}else{
				currentHP +=basePower;
				if(currentHP / hp <=0.25){
					Debug.Log ("I'm as good as dead.");
				}else if(currentHP / hp >0.25 && currentHP / hp <= 0.8){
					Debug.Log ("I feel a little better.");
				}else if(currentHP / hp >0.8 && hp!=currentHP){
					Debug.Log ("A little battered, but still good to go!");
				}
			}
			isWaiting = true;
			enemy.isWaiting = false;
		}
	}

	//Skill does a random action
	public void Starfall(){
		if (!isWaiting && mp >= 5) {
			int selection = Random.Range (1,5);
			int basePower = Random.Range (1,DiceSize);
			if(selection==1){//Deal Damage
				int dmg = (int)basePower*basePower/3;
				Debug.Log (dmg);
				enemy.GetHit (dmg);
			}else if(selection==2){//Heal
				basePower*=10;
				if (currentHP + basePower >= hp) {
					currentHP=hp;
					Debug.Log ("I'm really feeling it!");
				}else{
					currentHP +=basePower;
					if(currentHP / hp <=0.25){
						Debug.Log ("I'm as good as dead.");
					}else if(currentHP / hp >0.25 && currentHP / hp <= 0.8){
						Debug.Log ("I feel a little better.");
					}else if(currentHP / hp >0.8 && hp!=currentHP){
						Debug.Log ("A little battered, but still good to go!");
					}
				}
			}else if(selection==3){
				for(int i =0;i<5;i++){
					int dmg = (int)basePower;
					Debug.Log (dmg);
					enemy.GetHit (dmg);
				}
			}else if(selection==4){
				Debug.Log ("...Okay. I guess nothing happened :/");
			}

			isWaiting = true;
			enemy.isWaiting = false;
		}
	}

	public void BlessingOfTheBasedGod(){
		dmgMult += 0.5f;
		Debug.Log ("Lil B hears your prayer and blesses you.");
		isWaiting = true;
		enemy.isWaiting = false;
	}
}
