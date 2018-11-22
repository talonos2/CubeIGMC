﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceshipPawn : MonoBehaviour {

    public LaserBullet bulletPrefab;
    public float distanceBetweenShips;
    public float distanceToMyNose;

    public AudioSource chargeSound;
    public AudioSource getHitHeavySound;
    public AudioSource getHitLightSound;
    public AudioSource shieldSound;
    public AudioSource fireSound;

    public float transpositionalWaggle = .4f;
    public float positionalWaggle = 1;
    public float altitudinalWaggle = .2f;
    public float rollWaggle = 30;
    public float pitchWaggle = 15;
    public float phaseOffset;
    public float waggleSpeed = 1;

    public float damageDampeningAmount = .99f;
    public float damageEffectOnWaggle = .1f;
    public float damageEffectOnWaggleSpeed = .1f;

    private float damage;

    private float phase;
    private Vector3 rootPosition;

    private float partialDerivativeSampleDistance = .001f;

    // Use this for initialization
    void Start ()
    {
        rootPosition = this.transform.position;
        this.phase = phaseOffset;
	}
	
	// Update is called once per frame
	void Update ()
    {
        float damMult = (damage*damageEffectOnWaggle) + 1;
        phase += Time.deltaTime * waggleSpeed * ((damage+1)*damageEffectOnWaggleSpeed);
        this.transform.position = rootPosition + new Vector3((Mathf.PerlinNoise(0, phase + 100) - .5f) * positionalWaggle*damMult, (Mathf.PerlinNoise(0, phase)-.5f)*transpositionalWaggle * damMult, (Mathf.PerlinNoise(phase, 0) - .5f) * altitudinalWaggle * damMult);

        //Calculus? Calculus. Get partial derivative of the noise field to get velocity for roll.

        float dx = Mathf.PerlinNoise(0, phase + partialDerivativeSampleDistance) - Mathf.PerlinNoise(0, phase - partialDerivativeSampleDistance);
        float dy = Mathf.PerlinNoise(phase + partialDerivativeSampleDistance, 0) - Mathf.PerlinNoise(phase - partialDerivativeSampleDistance, 0);
        this.transform.rotation = Quaternion.Euler(new Vector3(0, (dy/partialDerivativeSampleDistance*pitchWaggle * damMult + 90)*(distanceToMyNose > 1 ? 1 : -1), (dx/partialDerivativeSampleDistance*rollWaggle*damMult-70) * (distanceToMyNose > 1 ? 1 : -1)));

        damage *= damageDampeningAmount;
    }

    internal void takeoff(int v, Vector3 position, Quaternion rotation)
    {
        
    }

    public void FireBullet(float damage, Combatant enemy, float flightTime)
    {
        LaserBullet bullet = GameObject.Instantiate(bulletPrefab);
        bullet.transform.SetPositionAndRotation(this.transform.position + new Vector3(distanceToMyNose, 0, 0), this.transform.rotation);
        bullet.transform.localScale = new Vector3(damage / 100.0f, damage / 100.0f, damage / 100.0f);
        bullet.GetComponent<Rigidbody>().velocity = new Vector3(distanceBetweenShips / flightTime, 0, 0);
        bullet.Initialize(enemy, flightTime, damage);
        
    }

    public void Damage(float damage)
    {
        this.damage += damage;
        //can also explode stuff, damage the ship, etc. Later.
    }


}
