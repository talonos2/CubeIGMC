using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryDisplay : MonoBehaviour {

    public Transform targetTransform;
    public InventoryItemDisplay ItemDispalyprefab;
    public Text ItemDesc;

	// Use this for initialization
	void Start () {
        Prime(ItemClass.GetAllItems());


    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Prime(List<ItemClass> items) {
        foreach (ItemClass item in items) {
            InventoryItemDisplay display = (InventoryItemDisplay)Instantiate(ItemDispalyprefab);
            display.transform.SetParent(targetTransform, false);
            display.Prime(item);
        }
        targetTransform.GetComponent<RectTransform>().sizeDelta.Set(targetTransform.GetComponent<RectTransform>().sizeDelta.x,59* (float)items.Count);

    }

    public void SetDescription() {

    }

}
