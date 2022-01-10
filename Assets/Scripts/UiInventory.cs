using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UiInventory : MonoBehaviour
{
    public Inventory _inventory = null;
    public GameObject _panel = null;

    public void UpdateInventoryView()
    {
        int i = 0;

        foreach (ItemInstance itemInstance in _inventory._itemInstances)
        {
            Image image = transform.GetChild(i).GetChild(0).GetComponent<Image>();
            Text text = transform.GetChild(i).GetChild(1).GetComponent<Text>();
            image.sprite = itemInstance._item._InventoryRepresentation;
            image.color = Color.white;
            text.text = "" + itemInstance._quantity;
            i++;
        }

        for (; i < 20; i++)
        {
            Image image = transform.GetChild(i).GetChild(0).GetComponent<Image>();
            Text text = transform.GetChild(i).GetChild(1).GetComponent<Text>();
            image.color = new Color(1, 1, 1, 0);
            text.text = "";
        }
    }

    public void onClick(int index)
    {
        RectTransform transform = this.transform.GetChild(index).GetComponent<RectTransform>();
        Debug.Log(transform.position);
        _panel.GetComponent<RectTransform>().position = transform.position + new Vector3(transform.rect.width/3 , transform.rect.height/3, 0);

        _panel.SetActive(true);
    }


    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 20; ++i)
        {
            Button button = transform.GetChild(i).GetComponent<Button>();
            int x = new int();
            x = i;
            button.onClick.AddListener(delegate { onClick(x); });
            Debug.Log(i);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
