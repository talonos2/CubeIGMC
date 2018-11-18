using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Combatant : MonoBehaviour
{
    private float health;
    private int energy;
    private float shields;
    private float psi;
    private float attackCharge;
    public Combatant enemy;
    private float queuedDamage;
    public CameraShake cameraToShake;

    public Transform healthBar;
    public Transform shieldParticleTarget;
    public Transform attackChargeBar;

    public SpaceshipPawn pawn;
    public PowerLights lights;

    public DamageManager damageManager;
    public PowerMultiplierTextChanger multiplierText;

    public void Start()
    {
        health = MaxHealth();
        energy = MaxEnergy() / 4; 
        shields = 0;
        this.initializer = GameObject.Find("Initializer").GetComponent<CombatInitializer>();
    }

    //In our current formula, there are no reasons to have cubes other than the "standard" cube.
    internal PowerupType GetRandomCubeType(SeededRandom dice)
    {
        List<PowerupType> randomBag = new List<PowerupType>();
        for (int x = 0; x < 100; x++)
        {
            randomBag.Add(PowerupType.ENERGY);
        }
        for (int x = 0; x < AttackCubes(); x++)
        {
            randomBag.Add(PowerupType.ATTACK);
        }
        for (int x = 0; x < ShieldCubes(); x++)
        {
            randomBag.Add(PowerupType.SHIELDS);
        }
        for (int x = 0; x < PsiCubes(); x++)
        {
            randomBag.Add(PowerupType.PSI);
        }
        return randomBag[dice.NextInt(0, randomBag.Count)];
    }

    internal float getShieldPercent()
    {
        return shields / MaxShields();
    }

    private float MaxShields()
    {
        return MaxHealth() * 2;
    }

    internal bool isAlive()
    {
        return health > 0;
    }

    private int PsiCubes()
    {
        return 0;
    }

    private int ShieldCubes()
    {
        return 0;
    }

    private int AttackCubes()
    {
        return 0;
    }

    public void Update()
    {
        shields = shields * Mathf.Exp(Mathf.Log(ShieldDecayFactor()) * Time.deltaTime);
        if (AttackIsQueued())
        {
            attackCharge -= (Time.deltaTime);
            if (attackCharge < 0)
            {
                attackCharge = 0;
                Fire();
            }
         }

        shieldParticleTarget.localScale = new Vector3(.2f, Math.Min(shields / MaxHealth(), 1), .2f);
        healthBar.localScale = new Vector3(.2f, Math.Max((health / MaxHealth()),0), .2f);
        attackChargeBar.localScale = new Vector3(.2f, (attackCharge / AttackChargeTime()), .2f);
    }

    //Modify this to change what a player's grid looks like when they start playing.
    internal void SetGridcellsStartingState(CellType[,] cellTypes)
    {
        for (int x = 0; x < 4; x++)
        {
            for (int y = 0; y < 4; y++)
            {
                cellTypes[x + 2, y + 10] = CellType.ATTACK;
                cellTypes[x + 9, y + 10] = CellType.SHIELD;
            }
        }
    }

    private float MaxHealth()
    {
        return 100;
    }

    private int MaxEnergy()
    {
        return 100;
    }

    private void Fire()
    { 

        float damage = DamageAmount();
        PayEnergyCostsForFiringAttack();

        pawn.FireBullet(damage, enemy, 2);

        pawn.chargeSound.Stop();
        pawn.fireSound.Stop();
        pawn.getHitLightSound.pitch = Math.Max(2.0f - (damage / DamageNeededForLargeSFX()),1);
        pawn.getHitLightSound.volume = Math.Min(damage / DamageNeededForLargeSFX(),1);
        pawn.fireSound.Play();
        this.queuedDamage = 0;
    }

    private void PayEnergyCostsForFiringAttack()
    {
        energy -= (int)(queuedDamage * EnergyMultiplier());
        energy = Math.Max(0, energy);
        RefreshEnergyBars();
    }

    internal void TakeDamage(float damage)
    {
        if (shields > damage)
        {
            shields -= damage;
            damage = 0;
        }
        else
        {
            damage -= shields;
            shields = 0;
            health -= damage;
        }

        if (damage <= 0)
        {
            //No damage left; shield absorbed it all.
            Debug.Log("Shield Sound");
            pawn.shieldSound.Stop();
            pawn.shieldSound.Play();
        }
        else
        {
            //Make sound based on modified damage
            if (damage > DamageNeededForLargeSFX())
            {
                pawn.getHitHeavySound.Stop();
                pawn.getHitHeavySound.Play();
            }
            else
            {
                pawn.getHitLightSound.Stop();
                pawn.getHitLightSound.pitch = 2.0f-(damage / DamageNeededForLargeSFX());
                pawn.getHitLightSound.volume = damage / DamageNeededForLargeSFX();
                pawn.getHitLightSound.Play();
            }
        }



        //if dead...
        if (!isAlive())
        {
            initializer.StartDeathSequence();
        }
        else
        {
            pawn.Damage(damage);
            damageManager.SetNewDamageProportion(this.health / this.MaxHealth());
            cameraToShake.ShakeCamera(damage / MaxHealth(), .5f);
        }
    }

    private float DamageNeededForLargeSFX()
    {
        return MaxHealth() * .3f;
    }

    private float DamageAmount()
    {
        //queuedDamage is the number of squares worth of attack tiles that have been submitted since attack charging began.
        return queuedDamage*.8f*EnergyMultiplier(); //Min Damage is .8, max damage is 51.2, average is about 20.
    }

    private bool AttackIsQueued()
    {
        return attackCharge > 0;
    }

    private float ShieldDecayFactor()
    {
        return .8f;
    }

    private float EnergyDecayFactor()
    {
        return .999f;
    }

    public void ChargeAttack(int attackCubes)
    {
        if (attackCubes == 0||EnergyMultiplier()==0)
        {
            return;
        }
        queuedDamage += attackCubes;
        if (attackCharge == 0)
        {
            attackCharge = AttackChargeTime();
            pawn.chargeSound.Play();
        }
    }

    public void ChargeShields(float shieldCubes)
    {
        shields += GetShieldChargeAmount(shieldCubes);
        shields = Math.Min(shields, MaxShields());
    }

    public void ChargeEnergy(int energyCubes)
    {
        energy += GetEnergyChargeAmount(energyCubes);
        energy = Math.Min(energy, MaxEnergy());
        RefreshEnergyBars();
    }

    private void RefreshEnergyBars()
    {
        lights.SetAllLights((float)energy / (float)MaxEnergy());
        int energyMulter = Mathf.RoundToInt(EnergyMultiplier());
        multiplierText.ChangeText(energyMulter);
    }

    private float GetShieldChargeAmount(float shieldCubes)
    {
        return shieldCubes * 10 * EnergyMultiplier();
    }

    private float EnergyMultiplier()
    {
        float energyRatio = (float)energy / (float)MaxEnergy();
        if (energyRatio > .8f) return 4;
        if (energyRatio > .6f) return 3;
        if (energyRatio > .4f) return 2;
        if (energyRatio > .2f) return 1;
        return 0;
    }

    private int GetEnergyChargeAmount(int energyCubes)
    {
        return energyCubes;
    }

    private float AttackChargeTime()
    {
        return 5;
    }

    public float lengthOfBar = 15;
    private float pretendEnergy;
    private CombatInitializer initializer;

    internal Vector3 GetTargetOfParticle(PowerupType type)
    {

        switch (type)
        {
            case PowerupType.ATTACK:
                return attackChargeBar.transform.position;
            case PowerupType.SHIELDS:
                Vector3 toReturn = shieldParticleTarget.transform.position;
                return toReturn;
            case PowerupType.ENERGY:
                Vector3 toReturn2 = lights.GetTargetAtPercent((float)pretendEnergy / (float)MaxEnergy());
                pretendEnergy+= GetEnergyChargeAmount(1);
                return toReturn2;
            default:
                return Vector3.zero; //We're not even handling Psi right now.
        }
    }

    internal void StartNewParticleBarrage()
    {
        pretendEnergy = energy;
    }

    public bool HasRoomForMoreEnergy()
    {
        return pretendEnergy < MaxEnergy();
    }
}