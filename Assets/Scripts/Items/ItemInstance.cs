using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class ItemInstance
{
    public Item _item;
    public int _quantity;

    public ItemInstance(Item item, int quantity)
    {
        _item = item;
        _quantity = quantity;
    }
}
