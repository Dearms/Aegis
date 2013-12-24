﻿#pragma strict
@script AddComponentMenu("Player Weapons/Vulcan Cannon")
var sight : GameObject;
var bullet : GameObject;
private var heatCost : float = 0;	//for basic bullet
private var cooldown : float = 0;
private var damage : float = 0;
var laser : AudioClip;
var lastShot : float = 0;
var playerStats : PlayerStats;

function Start () {
	bullet = Resources.Load("Prefabs/GreenLaser", GameObject);
	playerStats = gameObject.GetComponent(PlayerStats);
	sight = transform.FindChild("gun").gameObject;
	heatCost = playerStats.VulcanCannonStats.currentLevel.heatPerShot;
	damage = playerStats.VulcanCannonStats.currentLevel.damage;
	cooldown = playerStats.VulcanCannonStats.currentLevel.cooldown;
}

function Update () {
	
	if (((Input.GetButtonDown("Fire1") && playerStats.usingMouseAndKeyboard) || (playerStats.usingXboxController && Input.GetAxis("Fire1") < 0)) 
			&& !gameObject.GetComponent(PlayerStats).overheat && //make sure we're not overheating
			Time.time - playerStats.VulcanCannonStats.currentLevel.lastShot > cooldown){ //basic forward gun
		gameObject.GetComponent(PlayerStats).heat += heatCost; //up the heat
		var instance : GameObject;
		instance = Instantiate (bullet, sight.transform.position,sight.transform.rotation);
		bullet.GetComponent(Stats).damage = damage;
		audio.PlayOneShot(laser);
		instance.transform.localRotation.eulerAngles.x += 90; //because XZ plane
		Destroy (instance, 5); //after 5 seconds the bullet is way off the screen. This is for clean up
		playerStats.VulcanCannonStats.currentLevel.lastShot = Time.time;
	}
}