using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnnoyingTutorialPopup : MonoBehaviour {

    private float timeSoFar;
    public Image darkness;
    private bool hasPressedConfirm;
    SpriteRenderer renderer;

    // Use this for initialization
    void Start () {
        renderer = this.GetComponent<SpriteRenderer>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        timeSoFar += Time.deltaTime;
        if (!hasPressedConfirm)
        {
            darkness.color = new Color(0, 0, 0, Mathf.Lerp(0, .5f, timeSoFar * 6));
            this.transform.localPosition = (new Vector3(8, Mathf.Lerp(260, 310, timeSoFar * 6), -20));
            renderer.color = new Color(1, 1, 1, Mathf.Lerp(0, 1, timeSoFar * 6));
        }
        else
        {
            darkness.color = new Color(0, 0, 0, Mathf.Lerp(.5f, 0, timeSoFar * 6));
            this.transform.localPosition = (new Vector3(8, Mathf.Lerp(310, 360, timeSoFar * 6), -20));
            renderer.color = new Color(1, 1, 1, Mathf.Lerp(1, 0, timeSoFar * 6));
            if (timeSoFar >= 1)
            {
                this.gameObject.SetActive(false);
            }
        }

        if (Input.GetButtonDown("Place_P1")&&!hasPressedConfirm)
        {
            timeSoFar = 0;
            hasPressedConfirm = true;
            MissionManager.instance.grossCallbackHack.enabled = true;
        }

	}
}
