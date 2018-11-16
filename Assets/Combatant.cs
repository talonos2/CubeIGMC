using System;
using System.Collections.Generic;
using UnityEngine;

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
    public Transform shieldBar;
    public Transform attackChargeBar;

    public SpaceshipPawn pawn;
    public PowerLights lights;

    public DamageManager damageManager;

    public void Start()
    {
        health = MaxHealth();
        //energy = MaxEnergy() / 2; New system, energy starts at 0, but no decay.
        shields = 50;
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

        //energyBar.localScale = new Vector3(.2f, ((float)energy / (float)MaxEnergy()), .2f);
        shieldBar.localScale = new Vector3(.2f, Math.Min(shields / MaxHealth(), 1), .2f);
        healthBar.localScale = new Vector3(.2f, (health / MaxHealth()), .2f);
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
                pawn.getHitLightSound.Play();
            }
        }
        pawn.Damage(damage);
        damageManager.SetNewDamageProportion(this.health / this.MaxHealth());
        //cameraToShake.ShakeCamera(damage / MaxHealth(), 1.5f);
    }

    private float DamageNeededForLargeSFX()
    {
        return MaxHealth() * .3f;
    }

    private float DamageAmount()
    {
        //queuedDamage is the number of AttackCubes that have been submitted since attack charging began.
        return queuedDamage*5f*EnergyMultiplier();
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
        lights.SetAllLights((float)energy / (float)MaxEnergy());
    }

    private float GetShieldChargeAmount(float shieldCubes)
    {
        return shieldCubes * 10 * EnergyMultiplier();
    }

    private float EnergyMultiplier()
    {
        return 2f * (float)energy / (float)MaxEnergy();
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
    private float pretendShields;

    internal Vector3 GetTargetOfParticle(PowerupType type)
    {

        switch (type)
        {
            case PowerupType.ATTACK:
                return attackChargeBar.transform.position;
            case PowerupType.SHIELDS:
                Vector3 toReturn = shieldBar.transform.position + new Vector3(-lengthOfBar * pretendShields / MaxHealth(), 0, 0);
                pretendShields += GetShieldChargeAmount(1);
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
        pretendShields = shields;
    }
}