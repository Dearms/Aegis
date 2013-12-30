﻿#pragma strict
@script AddComponentMenu("Player Weapons/Fusion Beam")
var beam : GameObject;
var beamCharging : GameObject;
private var heatCost : float = 0;	//for basic bullet
private var cooldown : float = 0;
private var damage : float = 0;
var blast : AudioClip;
var playerStats : PlayerStats;
//var size : Vector2;
var chargeTime : float = 2;
var duration : float = 2;
private var input : String;
var currentChargeTime : float = 0; //private
var currentShootTime : float = 0; //private
private var instantiateBeamOnce : boolean = false;
private var instantiateChargingOnce : boolean = false;
private var instantiateChargingOnceTime : float = 0;
private var nozzle : Transform;

function Start () {
	beam = Resources.Load("Prefabs/FusionBeam", GameObject);
	playerStats = gameObject.GetComponent(PlayerStats);
	heatCost = playerStats.FusionBeamStats.currentLevel.heatPerShot;
	damage = playerStats.FusionBeamStats.currentLevel.damage;
	cooldown = playerStats.FusionBeamStats.currentLevel.cooldown;
	input = playerStats.FusionBeamStats.input;
	nozzle = transform.FindChild("nozzle");
}

private var thisBeam : GameObject;
private var thisBeamCharging : GameObject;
function Update () {
	
	if (((Input.GetButton(input) && playerStats.usingMouseAndKeyboard) || (playerStats.usingXboxController && Input.GetAxis(input) < 0)) 
			&& !gameObject.GetComponent(PlayerStats).overheat //make sure we're not overheating
			&& Time.time - playerStats.FusionBeamStats.currentLevel.lastShot > cooldown)
	{
		if (currentChargeTime  < chargeTime){
			if (instantiateChargingOnce == false){
				thisBeamCharging = Instantiate(beamCharging, nozzle.position, Quaternion.Euler(90,0,0));
				instantiateChargingOnce = true;
				instantiateChargingOnceTime = Time.time;
			}
			currentChargeTime += Time.deltaTime;
			thisBeamCharging.transform.position = nozzle.position;
			var color : Color = thisBeamCharging.renderer.material.GetColor("_TintColor");
			color.a += Time.deltaTime;
			thisBeamCharging.renderer.material.SetColor("_TintColor",color); 

		} else if (currentChargeTime > 0 && instantiateChargingOnce == true && Input.GetButton(input) == false){ //if button is let go before fully charged, empty
	 		Destroy(thisBeamCharging);
	 		instantiateChargingOnce = false;
	 	}
	}

	if (currentChargeTime >= chargeTime || instantiateBeamOnce == true)
	{ //charge maxed
		if (currentShootTime < duration && Input.GetButton(input) && instantiateBeamOnce == false){ //start beam
			thisBeam = Instantiate(beam, transform.FindChild("nozzle").position, Quaternion.identity);	
			instantiateBeamOnce = true;
			Debug.Log("Beam Instantiated");
			gameObject.GetComponent(PlayerStats).heat += heatCost;	
		} else if (Input.GetButton(input) && instantiateBeamOnce == true){ //tick time shot while held
			Debug.Log("tick");
			currentShootTime += Time.deltaTime;
			currentChargeTime = 0;
		} else if (instantiateBeamOnce == true && Input.GetButton(input) == false){ //let Go
			Debug.Log("let go during shoot."+"Destroy "+thisBeam.name);
			Destroy (thisBeam);
			playerStats.FusionBeamStats.currentLevel.lastShot = Time.time;
			instantiateBeamOnce = false;
			currentShootTime = 0;
		} else if (currentShootTime >= duration){ //held through hold duration
			if(thisBeam != null)Debug.Log("end shoot"+"Destroy "+thisBeam.name);
			Destroy (thisBeam);
			playerStats.FusionBeamStats.currentLevel.lastShot = Time.time;
			instantiateBeamOnce = false;
			currentShootTime = 0;
		}	
	}
}