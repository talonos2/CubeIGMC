using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearGridOnDeath : DeathEffect
{
    private GameGrid grid;

    public override void FinalityExplosion()
    {
        grid.DeathClear();
    }

    // Use this for initialization
    void Start () {
        this.grid = this.gameObject.GetComponent<GameGrid>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
