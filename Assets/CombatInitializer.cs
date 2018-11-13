using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatInitializer : MonoBehaviour {

    public GameGrid grid1;
    public GameGrid grid2;
	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        grid1.SetEnemy();
        grid2.SetEnemy();

        int randomSeed = UnityEngine.Random.Range(1, 65535);
        if (grid2.isPlayedByAI)
        {
            grid2.LoadAI();
            randomSeed = grid2.GetAISeed();
        }

        grid1.SetSeedAndStart(randomSeed);
        grid2.SetSeedAndStart(randomSeed);

        GameObject.Destroy(this);

    }
}
