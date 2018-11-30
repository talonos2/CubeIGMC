using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuCubeSpawner : MonoBehaviour
{
    public float maxRotation;
    public Vector2 highLow;
    public Vector2 inOut;
    public Vector2 slowFast;
    public Vector2 shortLong;
    public Rigidbody thingToClone;

    public Material[] possibleMats;

    // Use this for initialization
    void Start ()
    {
		
	}

    float timeTillNext;

	// Update is called once per frame
	void Update ()
    {
        timeTillNext -= Time.deltaTime;
        if (timeTillNext < 0)
        {
            timeTillNext = UnityEngine.Random.Range(shortLong.x, shortLong.y);
            Rigidbody toSpew = GameObject.Instantiate<Rigidbody>(thingToClone);
            toSpew.position = this.transform.position;
            toSpew.transform.Translate(new Vector3(0, UnityEngine.Random.Range(highLow.x, highLow.y), UnityEngine.Random.Range(inOut.x, inOut.y)));
            toSpew.AddForce(new Vector3(UnityEngine.Random.Range(slowFast.x, slowFast.y), 0, 0));
            toSpew.AddTorque(new Vector3(UnityEngine.Random.Range(0, maxRotation), UnityEngine.Random.Range(0, maxRotation), UnityEngine.Random.Range(0, maxRotation)));
            toSpew.gameObject.GetComponent<Renderer>().material = possibleMats[UnityEngine.Random.Range(0, possibleMats.Length)];
            GameObject.Destroy(toSpew.gameObject, 60);
        }
	}
}
