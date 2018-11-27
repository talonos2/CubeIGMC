using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Combatant : NetworkBehaviour
{
    internal float health;
    internal int energy;
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
    public bool isImmortal = false; //Used for recording so you don't blow up the enemy before you finish.

    public CubeHolder cubeHolder;

    public CombatInitializer initializer;
    private PlayerCharacterSheet ThisPlayer;
    private GameGrid ThisGameGrid;

    public void Start()
    {
        ThisPlayer = new PlayerCharacterSheet();
        ThisGameGrid = transform.parent.parent.parent.GetComponentInChildren<GameGrid>();

        //Example of making player 2 customly weaker or stronger. 
        //   if (transform.parent.parent.parent.name.Equals("Player2")) {
        //        ThisPlayer.BaseHealth = 30;
        //        ThisPlayer.WeaponEquippedID = 11;
        //        ThisPlayer.ShieldEquippedID = 0;
        //}

        health = ThisPlayer.GetMaxHealth();
        energy = ThisPlayer.GetMaxEnergy() / 4;
        shields = ThisPlayer.GetStartingShields();
//        Transform Go = Transform.Find("Initializer");
//        Debug.Log(Go);
//        this.initializer = Go.GetComponent<CombatInitializer>();
        this.RefreshEnergyBars();
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

        healthBar.localScale = new Vector3(.2f, Math.Max((health / MaxHealth()), 0), .2f);
        //attackChargeBar.localScale = new Vector3(.2f, (attackCharge / AttackChargeTime()), .2f);
    }

    //In our current formula, there are no reasons to have cubes other than the "standard" cube,
    //But this is here just in case we do something weird later.
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

    internal void RemoveDeathEffectFromDamageManager(TileFX tileFX)
    {
        damageManager.stuffThatHappensInTheFinalExplosion.Remove(tileFX);
    }

    //Methods that return combat statistics:

    private float MaxShields()
    {
        return ThisPlayer.GetMaxShields();
    }

    private float ShieldDecayFactor()
    {
        return ThisPlayer.GetShieldDecayFactor();
    }

    public float GetMovementSpeed()
    {
        return 1f/ThisPlayer.GetMovementSpeed();
    }

    private float MaxHealth()
    {
        return ThisPlayer.GetMaxHealth() * (isImmortal ? 100 : 1);
    }

    private int MaxEnergy()
    {
        return ThisPlayer.GetMaxEnergy();
    }

    private float DamageAmount()
    {
        //queuedDamage is the amount of attack charge passed into this function from "AddFlatAmountToAttackCharge" since last shot was fired.
        return queuedDamage * .6f;
    }

    /// <summary>
    /// How much attack charge does a single cube give?
    /// </summary>
    /// <returns></returns>
    public int getCubeAttackCharge()
    {
        return EnergyMultiplier();
    }

    /// <summary>
    /// How much shield charge does a single cube give?
    /// </summary>
    /// <returns></returns>
    public int getCubeShieldCharge()
    {
        return EnergyMultiplier();
    }

    internal int getCubeAttackEnergyCost()
    {
        return EnergyMultiplier();
    }

    internal int getCubeShieldEnergyCost()
    {
        return EnergyMultiplier();
    }

    private int EnergyMultiplier()
    {
        float energyRatio = (float)energy / (float)MaxEnergy();
        if (energyRatio > .75f) return 4;
        if (energyRatio > .5f) return 3;
        if (energyRatio > .25f) return 2;
        return 1;
    }

    /// <summary>
    /// Returns how much energy you get from a given number of 3x3 squares. This is multiplied by the number
    /// of squares made before being passed into this function: a 3x5 rectangle would pass "3" into this
    /// function, for instance.
    /// </summary>
    /// <param name="energySquares">The number of 3x3 squares made.</param>
    /// <returns>The amount of energy given. </returns>
    public int GetEnergyChargeAmount(int energySquares)
    {
        return energySquares * 9;
    }

    private float AttackChargeTime()
    {
        return 5;
    }

    //Modify this to change what a player's grid looks like when they start playing.
    internal void SetGridcellsStartingState(CellType[,] cellTypes)
    {
        ThisPlayer.GetWeaponPositions(cellTypes);
        ThisPlayer.GetShieldPositions(cellTypes);
        ThisPlayer.GetPsiPositions(cellTypes);
    }

    //Information methods:

    public float getShieldPercent()
    {
        return shields / MaxShields();
    }

    internal bool IsAlive()
    {
        return health > 0;
    }

    private bool AttackIsQueued()
    {
        return attackCharge > 0;
    }

    internal void AddDeathEffectToDamageManager(DeathEffect deathEffect)
    {
        damageManager.stuffThatHappensInTheFinalExplosion.Add(deathEffect);
    }

    public float bulletFlightTime = 2;

    private void Fire()
    {

        float damage = DamageAmount();
        //Energy costs for firing are now payed as the cubes are floated/converted into attack power.

        pawn.FireBullet(damage, enemy, bulletFlightTime);

        pawn.chargeSound.Stop();
        pawn.fireSound.Stop();
        pawn.fireSound.Play();
        this.queuedDamage = 0;
    }

    public void AddFlatAmountToAttackCharge(int amount)
    {
        //WARNING: You now must pay energy costs from the calling fuction!
        queuedDamage += amount;
        if (attackCharge == 0)
        {
            attackCharge = AttackChargeTime();
            pawn.chargeSound.Play();
            pawn.StartChargeParticleVFX(AttackChargeTime());
        }
    }

    public void AddFlatAmountToShield(float amount)
    {
        //Warning: You now must pay energy costs from the calling function!
        shields += amount;
        shields = Math.Min(Mathf.Max(0, shields), MaxShields());
    }

    public void AddFlatAmountToEnergy(int amount)
    {
        energy += amount;
        energy = Mathf.Max(Math.Min(energy, MaxEnergy()),0);
        RefreshEnergyBars();
    }

    //Sets the lights to the correct energy amount.
    private void RefreshEnergyBars()
    {
        lights.SetAllLights((float)energy / (float)MaxEnergy());
        int energyMulter = Mathf.RoundToInt(EnergyMultiplier());
        multiplierText.ChangeText(energyMulter);
    }

    /// <summary>
    /// Gets the amount of "Shield Charge" generated by a given number of shield cubes exploded by the screen.
    /// </summary>
    /// <param name="shieldCubes">The number of cubes exploded as shield cubes.</param>
    /// <returns>HP worth of shield given.</returns>
    /// #Deprecated
    public int GetShieldChargeAmount(int shieldCubes)
    {
        return (int)(shieldCubes * 10 * EnergyMultiplier());
    }

    internal void DeleteAllEnergy()
    {
        energy = 0;
        RefreshEnergyBars();
    }

    //Functions dealing with particle movement.

    private float pretendEnergy;
    private int heldAttackCubes;

    /// <summary>
    /// Returns the location a particle should go to. This also changes the location a particle
    /// will get sent to NEXT time you call this function based on how much this particle
    /// changed the values.
    /// </summary>
    /// <param name="type">The type of charge this particle contributes</param>
    /// <param name="changeInTarget">How much charge this particle contributes.</param>
    /// <returns></returns>
    internal Vector3 GetTargetOfParticle(PowerupType type, int changeInTarget)
    {
        switch (type)
        {
            case PowerupType.ENERGY:
                Vector3 toReturn2 = lights.GetTargetAtPercent((float)pretendEnergy / (float)MaxEnergy());
                pretendEnergy += GetEnergyChargeAmount(changeInTarget)/9f;
                return toReturn2;
            default:
                Debug.Log("I'm trying to get the target of a particle type that I shouldn't be sending!");
                return Vector3.zero; //We're not even handling Psi right now.
        }
    }

    internal Transform getNextAttackCubeHolderPosition()
    {
        heldAttackCubes = (heldAttackCubes + 1) % 30;
        return cubeHolder.cubesToHold[heldAttackCubes];
    }

    internal void StartNewParticleBarrage()
    {
        pretendEnergy = energy;
    }

    public bool HasRoomForMoreEnergy()
    {
        return pretendEnergy < MaxEnergy();
    }

    //Unused and vestigial.
    private int PsiCubes() { return 0; }
    private int ShieldCubes() { return 0; }
    private int AttackCubes() { return 0; }

    //Taking Damage and attendant SFX:
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
                pawn.getHitLightSound.pitch = 2.0f - (damage / DamageNeededForLargeSFX());
                pawn.getHitLightSound.volume = damage / DamageNeededForLargeSFX();
                pawn.getHitLightSound.Play();
            }
        }


        pawn.Damage(damage);
        damageManager.DamageComponentsBasedOnHealthLeft(this.health / this.MaxHealth());
        cameraToShake.ShakeCamera(damage / MaxHealth(), .5f);

        //if dead...
        if (!IsAlive())
        {
            initializer.StartDeathSequence();
            damageManager.ExplodeRemainingShip();
        }
    }

    private float DamageNeededForLargeSFX()
    {
        return MaxHealth() * .3f;
    }

    public PlayerCharacterSheet GetCharacterSheet() {
        return ThisPlayer;
    }

    public void SetCharacterSheet(int NPCReference) {
        ThisPlayer = PlayerCharacterSheet.GetNPC(NPCReference);
    }

    public void SetCharacterSheet(string SaveFileName) {
        ThisPlayer = PlayerCharacterSheet.LoadFromDisk(SaveFileName);
    }

    public void SaveCharacterToDisk(string SaveFileName) {
        PlayerCharacterSheet.SaveToDisk(ThisPlayer, SaveFileName);
    }

}