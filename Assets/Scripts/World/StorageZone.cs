using Models.Items;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ItemScript = ItemScripts.ItemScript;

public class StorageZone : MonoBehaviour
{
    public static StorageZone instance;

    public List<ItemScript> storedItems = new List<ItemScript>();

    [SerializeField]
    private InfoDisplay display;

    private void Start()
    {
        instance = this;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<ItemScript>(out ItemScript script))
        {
            storedItems.Add(script);
            script.onPickup += onItemRemoved;
            display.updateText();
        }
    }

    private void onItemRemoved(ItemScript item)
    {
        storedItems.Remove(item);
        display.updateText();
    }

    public int getStoredValue()
    {
        int value = 0;

        foreach (ItemScript item in storedItems)
        {
            value += item.getItemData().value;
        }

        return value;
    }

    public List<(BaseItem, Vector3)> getStoredItemsAndPositions()
    {
        List<(BaseItem, Vector3)> items = new List<(BaseItem, Vector3)>();

        foreach (ItemScript item in storedItems)
        {
            items.Add((item.getItemData(), item.transform.position));
        }

        return items;
    }
}
