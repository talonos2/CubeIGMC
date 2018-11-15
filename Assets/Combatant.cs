using System;
using System.Collections.Generic;
using UnityEngine;

public class Combatant : MonoBehaviour
{
    private float health;
    private float energy;
    private float shields;
    private float psi;
    private float attackCharge;
    public Combatant enemy;
    private float queuedDamage;


    public Transform energyBar;
    public Transform healthBar;
    public Transform shieldBar;
    public Transform attackChargeBar;

    public SpaceshipPawn pawn;

    public void Start()
    {
        health = MaxHealth();
        energy = MaxEnergy() / 2;
        shields = 0;
    }

    internal CubeType GetRandomCubeType(SeededRandom dice)
    {
        List<CubeType> randomBag = new List<CubeType>();
        for (int x = 0; x < 100; x++)
        {
            randomBag.Add(CubeType.ENERGY);
        }
        for (int x = 0; x < AttackCubes(); x++)
        {
            randomBag.Add(CubeType.ATTACK);
        }
        for (int x = 0; x < ShieldCubes(); x++)
        {
            randomBag.Add(CubeType.SHIELDS);
        }
        for (int x = 0; x < PsiCubes(); x++)
        {
            randomBag.Add(CubeType.PSI);
        }
        return randomBag[dice.NextInt(0, randomBag.Count)];
    }

    private int PsiCubes()
    {
        return 0;
    }

    private int ShieldCubes()
    {
        return 10;
    }

    private int AttackCubes()
    {
        return 10;
    }

    public void Update()
    {
        energy *= EnergyDecayFactor();
        shields *= ShieldDecayFactor();
        if (AttackIsQueued())
        {
            attackCharge -= (Time.deltaTime);
            if (attackCharge < 0)
            {
                attackCharge = 0;
                Fire();
            }
         }

        energyBar.localScale = new Vector3(.2f, (energy / MaxEnergy()), .2f);
        shieldBar.localScale = new Vector3(.2f, Math.Min(shields / MaxHealth(), 1), .2f);
        healthBar.localScale = new Vector3(.2f, (health / MaxHealth()), .2f);
        attackChargeBar.localScale = new Vector3(.2f, (attackCharge / AttackChargeTime()), .2f);
    }

    private float MaxHealth()
    {
        return 100;
    }

    private float MaxEnergy()
    {
        return 100;
    }

    private void Fire()
    { 

        float damage = DamageAmount();

        pawn.FireBullet(damage, enemy, 2);

        pawn.chargeSound.Stop();
        pawn.fireSound.Stop();
        pawn.getHitLightSound.pitch = Math.Max(2.0f - (damage / DamageNeededForLargeSFX()),1);
        pawn.getHitLightSound.volume = Math.Min(damage / DamageNeededForLargeSFX(),1);
        pawn.fireSound.Play();
        this.queuedDamage = 0;
    }

    internal void TakeDamage(float damage)
    {
        if (shields > damage)
        {
            shields -= damage;
        }
        else
        {
            damage -= shields;
            shields = 0;
            health -= damage;
        }

        if (damage < 0)
        {
            //No damage left; shield absorbed it all.
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
                Debug.Log("Hit Light: "+pawn.getHitLightSound.pitch + ", "+pawn.getHitLightSound.volume);
                pawn.getHitLightSound.Play();
            }
        }
    }

    private float DamageNeededForLargeSFX()
    {
        return MaxHealth() * .3f;
    }

    private float DamageAmount()
    {
        //queuedDamage is the number of AttackCubes that have been submitted since attack charging began.
        return queuedDamage*5*EnergyMultiplier();
    }

    private bool AttackIsQueued()
    {
        return attackCharge > 0;
    }

    private float ShieldDecayFactor()
    {
        return .998f;
    }

    private float EnergyDecayFactor()
    {
        return .999f;
    }

    public void ChargeAttack(int attackCubes)
    {
        if (attackCubes == 0)
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
    }

    public void ChargeEnergy(int energyCubes)
    {
        energy += GetEnergyChargeAmount(energyCubes);
        energy = Math.Min(energy, MaxEnergy());
    }

    private float GetShieldChargeAmount(float shieldCubes)
    {
        return shieldCubes * 10 * EnergyMultiplier();
    }

    private float EnergyMultiplier()
    {
        return 2 * energy / MaxEnergy();
    }

    private float GetEnergyChargeAmount(float energyCubes)
    {
        return energyCubes * 2;
    }

    private float AttackChargeTime()
    {
        return 5;
    }

    public float lengthOfBar = 15;
    private float pretendEnergy;
    private float pretendShields;

    internal Vector3 GetTargetOfParticle(CubeType type)
    {

        switch (type)
        {
            case CubeType.ATTACK:
                return attackChargeBar.transform.position;
            case CubeType.SHIELDS:
                Vector3 toReturn = shieldBar.transform.position + new Vector3(-lengthOfBar * pretendShields / MaxHealth(), 0, 0);
                pretendShields += GetShieldChargeAmount(1);
                return toReturn;
            case CubeType.ENERGY:
                Vector3 toReturn2 = energyBar.transform.position + new Vector3(-lengthOfBar * pretendEnergy / MaxEnergy(), 0, 0);
                pretendEnergy+= GetEnergyChargeAmount(1);
                return toReturn2;
            default:
                return Vector3.zero; //We're not even handling Psi right now.
        }
    }

    internal void StartNewParticleBarrage()
    {
        pretendEnergy = energy;
        pretendShields = shields;
    }
}