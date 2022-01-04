using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class Item : ScriptableObject
{
    public GameObject _physicalRepresentation;
    public Sprite _InventoryRepresentation;

    public string _name = "default";

}
