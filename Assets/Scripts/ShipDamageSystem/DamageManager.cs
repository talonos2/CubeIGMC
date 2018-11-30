using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageManager : MonoBehaviour
{
    public List<DamagableDisplay> damagableComponents;
    public List<DeathEffect> stuffThatHappensInTheFinalExplosion;

    private List<DamagableDisplay> damagedComponents = new List<DamagableDisplay>();


    // Use this for initialization
    void Start ()
    {
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    internal void DamageComponentsBasedOnHealthLeft(float percentHealthRemaining)
    {
        percentHealthRemaining = Mathf.Max(Mathf.Min(1, percentHealthRemaining), 0);
        int numberOfComponentsThatShouldBeDamaged = Mathf.RoundToInt((1-percentHealthRemaining) * damagableComponents.Count);
        if (numberOfComponentsThatShouldBeDamaged > damagedComponents.Count)
        {
            DamageSomeComponents(numberOfComponentsThatShouldBeDamaged - damagedComponents.Count);
        }
        else if (numberOfComponentsThatShouldBeDamaged < damagedComponents.Count)
        {
            //TODO: Ship repair or whatever.
            //DamageSomeComponents(damagedComponents.Count-numberOfComponentsThatShouldBeDamaged);
        }
    }

    public void ExplodeRemainingShip()
    {
        foreach (DeathEffect piece in stuffThatHappensInTheFinalExplosion)
        {
            piece.FinalityExplosion();
        }
    }

    private void DamageSomeComponents(int numberOfComponents)
    {
        for (int x = 0; x < numberOfComponents; x++)
        {

            //Get all components whose prerequisite components have all been damaged already.
            List<DamagableDisplay> componentsThatCanBeDamagedRightNow = new List<DamagableDisplay>();
            foreach (DamagableDisplay component in damagableComponents)
            {
                if (!component.isDamaged)
                {
                    componentsThatCanBeDamagedRightNow.Add(component);
                }
            }

            bool canItBeDamaged = false;
            DamagableDisplay toDamage = null;
            while (!canItBeDamaged)
            {
                canItBeDamaged = true;
                toDamage = componentsThatCanBeDamagedRightNow[UnityEngine.Random.Range(0, componentsThatCanBeDamagedRightNow.Count)];
                foreach (DamagableDisplay prereq in toDamage.thingsThatMustBeDamagedFirst)
                {
                    if (!damagedComponents.Contains(prereq))
                    {
                        canItBeDamaged = false;
                        componentsThatCanBeDamagedRightNow.Remove(toDamage);
                        if (componentsThatCanBeDamagedRightNow.Count == 0)
                        {
                            Debug.LogError("Ran out of damagable components somehow!");
                            break;
                        }
                    }
                }
            }


            //From this list, pick a random component
            toDamage.Damage();
            damagedComponents.Add(toDamage);
            
            //Do that the specified number of times.
        }
    }
}
