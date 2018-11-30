using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagableFireEvent : DamagableDisplay
{
    public GameObject firesPrefab;
    private GameObject fires;

    public override void Damage()
    {
        fires.gameObject.SetActive(true);
        this.isDamaged = true;
    }

    public override void Fix()
    {
        throw new System.NotImplementedException();
    }

    // Use this for initialization
    void Start()
    {
            fires = GameObject.Instantiate(firesPrefab);
            fires.transform.parent = this.transform;
            fires.transform.localPosition = new Vector3(0, 0, 0);
        fires.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        fires.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
