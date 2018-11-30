using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ItemSelectionManager : MonoBehaviour {

    public static List<GameObject> ItemObjectList = new List<GameObject>();
    public Text DescriptionBox;
    public Transform ScrollBar;
    public Transform TempPlayerStorage;
    private int ListLocation = 0;
    private int ListSize = 0;
    private int TimeSinceLastInput = 7;
    private int Scrolling = 0;
    private int ScrollingDn = 0;


    //public bool ListCreationFinished = false;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (TimeSinceLastInput < 40)
            TimeSinceLastInput++;
        if (TimeSinceLastInput > 7) {
            Scrolling = 0;
            ScrollingDn = 0;
        }
            
        

        if (Input.GetAxis("Vertical") > .1f && ListLocation>0 && TimeSinceLastInput > 5 ) {
            if (Scrolling == 0 || Scrolling > 2)
            {
                ItemObjectList[ListLocation].GetComponent<InventoryItemDisplay>().UnShadeItem();
                ListLocation--;
                ItemObjectList[ListLocation].GetComponent<InventoryItemDisplay>().ShadeItem();
                DescriptionBox.text = ItemObjectList[ListLocation].GetComponent<InventoryItemDisplay>().Item.GetItemDesc();
                if (ListLocation> 6 && ListLocation < 29)
                    {
                    ScrollBar.GetComponent<Scrollbar>().value+=.05f; }
                if (ListLocation <= 6) ScrollBar.GetComponent<Scrollbar>().value = 1;
            }
            TimeSinceLastInput = 0;
            Scrolling++;
        }

        if (Input.GetAxis("Vertical") < -.1f && ListLocation < (ListSize-1) && TimeSinceLastInput > 5)
        {
            if (ScrollingDn == 0 || ScrollingDn > 2)
            {
                ItemObjectList[ListLocation].GetComponent<InventoryItemDisplay>().UnShadeItem();
                ListLocation++;
                ItemObjectList[ListLocation].GetComponent<InventoryItemDisplay>().ShadeItem();
                DescriptionBox.text = ItemObjectList[ListLocation].GetComponent<InventoryItemDisplay>().Item.GetItemDesc();
                if (ListLocation < 29 && ListLocation>6)
                {
                    ScrollBar.GetComponent<Scrollbar>().value -= .05f;
                }
                if (ListLocation >=28) ScrollBar.GetComponent<Scrollbar>().value = 0;
            }
            TimeSinceLastInput = 0;
            ScrollingDn++;
        }
        
    }

    public void goToSampleScene()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void ClickedItem(int locationInArray) {
        ItemObjectList[ListLocation].GetComponent<InventoryItemDisplay>().UnShadeItem();
        ListLocation = locationInArray;
        ItemObjectList[ListLocation].GetComponent<InventoryItemDisplay>().ShadeItem();
        DescriptionBox.text = ItemObjectList[ListLocation].GetComponent<InventoryItemDisplay>().Item.GetItemDesc();

    }

   public void PushSelectedItemToPurchase() {
        TempPlayerStorage.GetComponent<PurchaseManager>().AttemptToPurchase(ItemObjectList[ListLocation].GetComponent<InventoryItemDisplay>().Item);


    }

    public int AddToList(GameObject itemPrefObj) {
        ItemObjectList.Add(itemPrefObj);

        if (ListSize == 0) { 
            ItemObjectList[0].GetComponent<InventoryItemDisplay>().ShadeItem();
            DescriptionBox.text = ItemObjectList[0].GetComponent<InventoryItemDisplay>().Item.GetItemDesc();
            }
        ListSize++;
        return (ListSize - 1);
    }

}
