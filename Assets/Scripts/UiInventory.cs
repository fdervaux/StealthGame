using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UiInventory : MonoBehaviour
{
    public Inventory _inventory = null;

    public void UpdateInventoryView()
    {
        int i = 0;

        foreach (ItemInstance itemInstance in _inventory._itemInstances)
        {
            Image image =transform.GetChild(i).GetChild(0).GetComponent<Image>();
            Text text =transform.GetChild(i).GetChild(1).GetComponent<Text>();
            image.sprite = itemInstance._item._InventoryRepresentation;
            image.color = Color.white;
            text.text = "" + itemInstance._quantity;
            i++;
        }

        for ( ;i < 20; i++)
        {
            Image image =transform.GetChild(i).GetChild(0).GetComponent<Image>();
            Text text =transform.GetChild(i).GetChild(1).GetComponent<Text>();
            image.color = new Color(1,1,1,0);
            text.text = "";
        }
    }


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
