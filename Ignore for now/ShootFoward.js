﻿#pragma strict
/*
* Ideally attached to an empty object then parented to the unit. This is so the place
* where the bullet spawns is easily adjusted.
*/
var bullet : GameObject;	//gameObject to be used. should be a prefab
var cooldown : float;		//how long of a wait between shots? in seconds
private var lastShot : float = 0;
private var grabInitialTimeBool : boolean = false;
private var grabInitialTime : float = 1;
var wait : float = 0;		//wait 4 seconds after being activated to shoot, geared towards AI

function Update () {
	if (!grabInitialTimeBool){
		grabInitialTime = Time.time;
		grabInitialTimeBool = true;
	}
	if (Time.time - lastShot > cooldown && Time.time - grabInitialTime > wait){ //basic forward gun
		var instance : GameObject;
		instance = Instantiate (bullet, transform.position, Quaternion.Euler(90,0,0));
		Destroy (instance, 5);
		lastShot = Time.time;
	}

}