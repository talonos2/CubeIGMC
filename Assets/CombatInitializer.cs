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
        grid1.SetEnemy(grid2);
        grid2.SetEnemy(grid1);
        GameObject.Destroy(this);
	}
}
