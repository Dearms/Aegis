﻿using UnityEngine;
using System.Collections;

public class FusionBeam : MonoBehaviour 
{

    GameObject beamObj;
    public Weapon fusionBeam = new Weapon();
    public AudioClip blast;
    PlayerStats playerStats;
    //var size : Vector2;
    public float duration;
    float currentShootTime = 0; //private
    bool instantiateBeamOnce = false;
    public GameObject nozzle;
    Vector3 level0Size = new Vector3(1.5f, 4, 2);
    Vector3 level1Size = new Vector3(1.75f, 4, 3);
    Vector3 level2Size = new Vector3(2, 4, 4);
    Vector3 level3Size = new Vector3(2.25f, 4, 5);

    void Awake()
    {
        fusionBeam.Awake();
    }

    void Start () 
    {
	    beamObj = Resources.Load<GameObject>("Prefabs/FusionBeam");
	    playerStats = GetComponent<PlayerStats>();
	    nozzle = GameObject.Find("nozzle");
    }

    private GameObject thisBeam;
    void Update ()
    {
	    if (((Input.GetButton("Fire2") && InputCoordinator.usingMouseAndKeyboard) || (InputCoordinator.usingController && Input.GetButton("joystick button 5"))) 
			    && !GetComponent<PlayerStats>().overheat //make sure we're not overheating
			    && Time.time - fusionBeam.currentLevel.ltShot > fusionBeam.currentLevel.cooldown
                && !instantiateBeamOnce)
	    {
            //print("fusion beam firing");
			thisBeam = Instantiate(beamObj, nozzle.transform.position, Quaternion.Euler(Vector3.zero)) as GameObject;	
			instantiateBeamOnce = true;
			thisBeam.GetComponent<FusionBeamInstance>().CollisionDamage = fusionBeam.currentLevel.damage;
			if (fusionBeam.level == 0)thisBeam.transform.localScale = level0Size; 
			if (fusionBeam.level == 1)thisBeam.transform.localScale = level1Size;
			if (fusionBeam.level == 2)thisBeam.transform.localScale = level2Size;
			if (fusionBeam.level == 3)thisBeam.transform.localScale = level3Size;
			//Debug.Log("Beam Instantiated");
			fusionBeam.currentLevel.ltShot = Time.time;
			GetComponent<PlayerStats>().heat += fusionBeam.currentLevel.heatCost;	
		} 
        else if (((Input.GetButton("Fire2") && InputCoordinator.usingMouseAndKeyboard) || (InputCoordinator.usingController && Input.GetButton("joystick button 5"))) && instantiateBeamOnce == true)
        { //tick time shot while held
			//Debug.Log("tick");
			currentShootTime += Time.deltaTime;
            fusionBeam.currentLevel.ltShot = Time.time;
		} 
        else if (instantiateBeamOnce == true && !((Input.GetButton("Fire2") && InputCoordinator.usingMouseAndKeyboard) || (InputCoordinator.usingController && Input.GetButton("joystick button 5"))))
        { //let Go
			//Debug.Log("let go during shoot."+"Destroy "+thisBeam.name);
			// Destroy (thisBeam);
			instantiateBeamOnce = false;
			currentShootTime = 0;
		}
		if (currentShootTime >= duration && instantiateBeamOnce == true)
        { //held through hold duration
			//if(thisBeam != null)Debug.Log("end shoot"+"Destroy "+thisBeam.name);
			// Destroy (thisBeam);
			fusionBeam.currentLevel.ltShot = Time.time;
			instantiateBeamOnce = false;
			currentShootTime = 0;
		}	
    }
}
