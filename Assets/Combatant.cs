using System;
using System.Collections.Generic;
using UnityEngine;

internal class Combatant
{
    private float HP;
    private float energy;
    private float shields;
    private float psi;


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
}