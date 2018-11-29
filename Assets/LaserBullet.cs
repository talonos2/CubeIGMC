using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LaserBullet : NetworkBehaviour {

    private Combatant personImAttacking;
    private float flightTime;
    private float damage;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        flightTime -= Time.deltaTime;
        if (flightTime <= 0)
        {
            personImAttacking.TakeDamage(damage);
            GameObject.Destroy(this.gameObject);
            if (MissionManager.triggerCallbacksOnAttackHit)
            {
                MissionManager.instance.grossCallbackHack.enabled = true;
            }
        }
		
	}

    public void Initialize(Combatant personImAttacking, float flightTime, float damage)
    {
        this.personImAttacking = personImAttacking;
        this.flightTime = flightTime;
        this.damage = damage;
    }

    [Command]
    public void CmdInitialize(Combatant personImAttacking, float flightTime, float damage)
    {
        this.personImAttacking = personImAttacking;
        this.flightTime = flightTime;
        this.damage = damage;
    }

}
