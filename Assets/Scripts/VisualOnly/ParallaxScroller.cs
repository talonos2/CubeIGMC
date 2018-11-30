using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxScroller : MonoBehaviour
{
    public float closeStarsScrollSpeed;
    public float farStarsScrollSpeed;
    public float nebulaMinScrollSpeed;
    public float nebulaMaxScrollSpeed;
    public float disappearLocation;

    public Transform closeStars1;
    public Transform closeStars2;
    public Transform farStars1;
    public Transform farStars2;
    public Transform[] nebulae;

    public float nebulaAppearChance = 0;
    public float nebulaMaxSize = 60;
    public float nebulaMinSize = 20;

    private float[] nebulaeScrollSpeed;
    private bool[] nebulaeActive;

    // Use this for initialization
    void Start ()
    {
        nebulaeScrollSpeed = new float[nebulae.Length];
        nebulaeActive = new bool[nebulae.Length];
    }
	
	// Update is called once per frame
	void Update ()
    {

        closeStars1.Translate(-closeStarsScrollSpeed, 0, 0);
        closeStars2.Translate(-closeStarsScrollSpeed, 0, 0);
        farStars1.Translate(-farStarsScrollSpeed, 0, 0);
        farStars2.Translate(-farStarsScrollSpeed, 0, 0);
        if (closeStars1.position.x < -disappearLocation) { closeStars1.Translate(disappearLocation * 2, 0, 0); }
        if (closeStars2.position.x < -disappearLocation) { closeStars2.Translate(disappearLocation * 2, 0, 0); }
        if (farStars1.position.x < -disappearLocation) { farStars1.Translate(disappearLocation * 2, 0, 0); }
        if (farStars2.position.x < -disappearLocation) { farStars2.Translate(disappearLocation * 2, 0, 0); }
        for (int nebulaNum = 0; nebulaNum < nebulae.Length; nebulaNum++)
        {
            if(nebulaeActive[nebulaNum])
            {
                nebulae[nebulaNum].Translate(-nebulaeScrollSpeed[nebulaNum], 0, 0);
                if (nebulae[nebulaNum].position.x < -disappearLocation)
                {
                    nebulaeActive[nebulaNum] = false;
                }
            }
            else
            {
                if (UnityEngine.Random.Range(0, 1f) < nebulaAppearChance)
                {
                    nebulae[nebulaNum].localPosition = new Vector3(disappearLocation, UnityEngine.Random.Range(-disappearLocation, disappearLocation), 0);
                    float nebulaSize = UnityEngine.Random.Range(nebulaMinSize, nebulaMaxSize);
                    nebulae[nebulaNum].localScale = new Vector3(nebulaSize, nebulaSize, 0);
                    nebulaeScrollSpeed[nebulaNum] = UnityEngine.Random.Range(nebulaMinScrollSpeed, nebulaMaxScrollSpeed);
                    nebulaeActive[nebulaNum] = true;
                }
            }
        }
    }
}
