using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvisibleDelayedChargeGiver : MonoBehaviour
{
    public PowerupType type;
    public float delay;
    public int amount;
    public Combatant target;
	
    // Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        delay -= Time.deltaTime;
        if (delay<0)
        {
            switch (type)
            {
                case PowerupType.ENERGY:
                    target.AddFlatAmountToEnergy(amount);
                    break;
                case PowerupType.SHIELDS:
                    target.AddFlatAmountToShield(amount);
                    break;
                case PowerupType.ATTACK:
                    target.AddFlatAmountToAttackCharge(amount);
                    break;
            }
            GameObject.Destroy(this.gameObject);
        }
	}

    internal void SetAmountForOneCube(PowerupType type)
    {
        switch (type)
        {
            case PowerupType.ENERGY:
                amount = target.GetEnergyChargeAmount(1);
                break;
            case PowerupType.SHIELDS:
                amount = target.getCubeShieldCharge();
                break;
            case PowerupType.ATTACK:
                amount = target.getCubeAttackCharge();
                break;
        }
    }

    internal void SetAmountAsEnergyCostForOneCube(PowerupType type)
    {
        switch (type)
        {
            case PowerupType.ENERGY:
                Debug.LogError("Error! We're trying to get the energy cube cost for a cube of energy! That makes no sense!");
                break;
            case PowerupType.SHIELDS:
                amount = target.getCubeShieldEnergyCost()*-1;
                break;
            case PowerupType.ATTACK:
                amount = target.getCubeAttackEnergyCost()*-1;
                break;
        }
    }
}
