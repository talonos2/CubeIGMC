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
    private Combatant enemy;
    private float queuedDamage;

    public Transform energyBar;
    public Transform healthBar;
    public Transform shieldBar;

    public void Start()
    {
        health = MaxHealth();
        energy = MaxEnergy() / 2;
    }

    internal CubeType GetRandomCubeType()
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
        return randomBag[UnityEngine.Random.Range(0, randomBag.Count - 1)];
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
            attackCharge -= (1f / 60f);
            if (attackCharge < 0)
            {
                attackCharge = 0;
                Fire();
            }
         }

        energyBar.localScale = new Vector3(1, (energy / MaxEnergy()), 1);
        shieldBar.localScale = new Vector3(1, Math.Min(shields / MaxHealth(), 1), 1);
        healthBar.localScale = new Vector3(1, (health / MaxHealth()), 1);
    }

    private float MaxHealth()
    {
        return 250;
    }

    private float MaxEnergy()
    {
        return 1000;
    }

    private void Fire()
    {
        enemy.TakeDamage(DamageAmount());
        this.queuedDamage = 0;
    }

    private void TakeDamage(float damage)
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
    }

    private float DamageAmount()
    {
        //queuedDamage is the number of AttackCubes that have been submitted since attack charging began.
        return queuedDamage*(energy/500f);
    }

    private bool AttackIsQueued()
    {
        return attackCharge > 0;
    }

    private float ShieldDecayFactor()
    {
        return .98f;
    }

    private float EnergyDecayFactor()
    {
        return .99f;
    }

    public void ChargeAttack(int attackCubes)
    {
        queuedDamage += attackCubes;
        if (attackCharge == 0)
        {
            attackCharge = AttackChargeTime();
        }
    }

    public void ChargeShields(int shieldCubes)
    {
        shields += getShieldChargeAmount(shieldCubes);
    }

    public void ChargeEnergy(int energyCubes)
    {
        shields += GetEnergyChargeAmount(energyCubes);
    }

    private float getShieldChargeAmount(int shieldCubes)
    {
        return shieldCubes * 2;
    }

    private float GetEnergyChargeAmount(int energyCubes)
    {
        return energyCubes * 2;
    }

    private float AttackChargeTime()
    {
        return 5;
    }
}