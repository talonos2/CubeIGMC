using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DamagableDisplay : MonoBehaviour
{

    public List<DamagableDisplay> thingsThatMustBeDamagedFirst;

    public bool isDamaged = false;

    public abstract void Damage();

    public abstract void Fix();
}