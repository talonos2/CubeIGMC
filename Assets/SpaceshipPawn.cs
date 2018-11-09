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

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void FireBullet(float damage, Combatant enemy, float flightTime)
    {
        LaserBullet bullet = GameObject.Instantiate(bulletPrefab);
        bullet.transform.SetPositionAndRotation(this.transform.position + new Vector3(distanceToMyNose, 0, 0), this.transform.rotation);
        bullet.transform.localScale = new Vector3(damage / 100.0f, damage / 100.0f, damage / 100.0f);
        bullet.GetComponent<Rigidbody>().velocity = new Vector3(distanceBetweenShips / flightTime, 0, 0);
        bullet.Initialize(enemy, flightTime, damage);
        
    }


}
