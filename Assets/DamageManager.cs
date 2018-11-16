using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageManager : MonoBehaviour
{
    public List<DamagableDisplay> damagableComponents;

    private List<DamagableDisplay> damagedComponents = new List<DamagableDisplay>();

    private int numberOfDamagableComponents;


    // Use this for initialization
    void Start ()
    {
        numberOfDamagableComponents = damagableComponents.Count;
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    internal void SetNewDamageProportion(float percentHealthRemaining)
    {
        percentHealthRemaining = Mathf.Max(Mathf.Min(1, percentHealthRemaining), 0);
        int numberOfComponentsThatShouldBeDamaged = Mathf.RoundToInt((1-percentHealthRemaining) * numberOfDamagableComponents);
        Debug.Log("Here: " + percentHealthRemaining+", "+numberOfComponentsThatShouldBeDamaged);
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
                    bool canBeDamaged = true;
                    foreach (DamagableDisplay prereq in component.thingsThatMustBeDamagedFirst)
                    {
                        if (!damagedComponents.Contains(prereq))
                        {
                            canBeDamaged = false;
                        }
                    }
                    if (canBeDamaged)
                    {
                        componentsThatCanBeDamagedRightNow.Add(component);
                    }
                }
            }

            //From this list, pick a random component
            DamagableDisplay toDamage = componentsThatCanBeDamagedRightNow[UnityEngine.Random.Range(0, componentsThatCanBeDamagedRightNow.Count)];
            toDamage.Damage();
            damagedComponents.Add(toDamage);
            
            //Do that the specified number of times.
        }
    }
}
