﻿using UnityEngine;
using System.Collections;

public class RammerAI : MonoBehaviour, ICollisionDamage, IKillable, IDamageable<float>, IGivesExp<float>, ISpeed
{
    [SerializeField]
    private float collisionDamage;
    public float CollisionDamage
    {
        get { return collisionDamage; }
        set { collisionDamage = value; }
    }
    [SerializeField]
    private float speed;
    public float Speed
    {
        get { return speed; }
        set { speed = value; }
    }
    [SerializeField]
    private float health;
    public float Health
    {
        get { return health; }
        set { health = value; }
    }
    [SerializeField]
    private float expGiven;
    public float ExpGiven
    {
        get { return expGiven; }
        set { expGiven = value; }
    }
    public float superEffectiveMod = 2;
    public GameObject superEffectiveParticleSystem;
    GameObject player;

	void Start () 
    {
        player = GameObject.FindGameObjectWithTag("Player");
	}
	
	void Update () 
    {
        if (player == null)
            return;
        if (player.transform.position.x < transform.position.x)
            transform.LookAt(player.transform.position);

        transform.position += transform.forward * (speed) * Time.deltaTime;

        if (health <= 0)
        {
            Kill();
            GiveExp(ExpGiven);
        }
	}

    public void DamageProjectile(float damageTaken)
    {
        health -= damageTaken * superEffectiveMod;
        SuperEffectiveSystem();
    }

    public void DamageExplosive(float damageTaken)
    {
        Damage(damageTaken);
    }

    public void DamageEnergy(float damageTaken)
    {
        Damage(damageTaken);
    }

    public void SuperEffectiveSystem()
    {
        GameObject inst = Instantiate(superEffectiveParticleSystem, transform.position, transform.rotation) as GameObject;
        Destroy(inst, 5);
        
    }

    public void Damage(float damageTaken)
    {
        health -= damageTaken;
    }

    void OnCollisionEnter(Collision other)
    {
        other.gameObject.SendMessage("Damage", CollisionDamage, SendMessageOptions.DontRequireReceiver);
        //print("HIT");
    }

    public void GiveExp (float exp)
    {
        player.SendMessage("GiveExp", exp);
    }

    public void Kill()
    {
        Destroy(gameObject);
    }
}