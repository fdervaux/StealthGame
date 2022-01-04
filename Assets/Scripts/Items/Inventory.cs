using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{

    public List<ItemInstance> _itemInstances = new List<ItemInstance>();

    public Hat _hat;
    public Weapon _weapon;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (_itemInstances.Count < 20)
        {
            Pickable pickable = other.GetComponent<Pickable>();

            if (pickable != null)
            {
                bool findObject = false;

                foreach (ItemInstance instance in _itemInstances)
                {
                    if (instance._item == pickable._itemInstance._item)
                    {
                        instance._quantity += pickable._itemInstance._quantity;
                        findObject = true;
                        break;
                    }
                }

                if (!findObject)
                {
                    _itemInstances.Add(pickable._itemInstance);
                }

                GameObject.Destroy(other.gameObject);
            }
        }
    }
}
