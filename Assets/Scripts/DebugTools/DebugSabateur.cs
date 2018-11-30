using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugSabateur : MonoBehaviour
{

    public Combatant toSabotage;

    private float timePassed;

    private static int[] damageAmounts = { 10, 20, 40, 80 };
    private readonly bool[] damageTimes = new bool[damageAmounts.Length];

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        timePassed += Time.deltaTime;

        for(int x = 0; x < damageAmounts.Length; x++)
        {
            if (timePassed > 3.5f*(x+1) && !damageTimes[x])
            {
                toSabotage.TakeDamage(damageAmounts[x]);
                damageTimes[x] = true;
            }
        }
	}
}
