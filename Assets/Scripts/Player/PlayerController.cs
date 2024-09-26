using System.Collections.Generic;
using DetectionZone;
using Models.Items;
using ResourceNode;
using Toolbar;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private ToolbarController toolbarController;

    [SerializeField]
    private PlayerDetectionZone playerDetectionZone;
    
    public BaseItem[] testItems;

    private void Awake()
    {
        if (!toolbarController)
        {
            throw new UnityException("Toolbar controller not attached to player controller");
        }

        if (!playerDetectionZone)
        {
            throw new UnityException("Player detection zone not attached to player controller");
        }
    }

    void Update()
    {
        InteractWithInventory();
        InteractWithNearbyObject();
    }

    private void InteractWithInventory()
    {
        for(int i = 0; i < ToolbarController.inventorySize; i++)
        {
            if (Input.GetKey(GameInputs.keys["Slot " + (i + 1)]))
            {
                toolbarController.SelectSlot(i);
            }
        }

        if(Input.mouseScrollDelta.y < 0)
        {
            toolbarController.SelectSlot(GameUtility.loopIndex(toolbarController.getSelectedSlotIndex() + 1, ToolbarController.inventorySize));
        }
        else if(Input.mouseScrollDelta.y > 0)
        {
            toolbarController.SelectSlot(GameUtility.loopIndex(toolbarController.getSelectedSlotIndex() - 1, ToolbarController.inventorySize));
        }

        if (Input.GetKeyDown(GameInputs.keys["Drop Item"]))
        {
            toolbarController.RemoveItemAtSelectedSlot();
        }
        
        if (Input.GetKeyDown(GameInputs.keys["Use Item"]))
        {
            toolbarController.InteractWithSelectedItem();
        }
    }

    public void InteractWithNearbyObject()
    {
        if (!Input.GetKeyDown(GameInputs.keys["Interact"]))
        {
            return;
        }

        IEnumerable<IMineable> mineables = playerDetectionZone.GetMineablesNearby();
        foreach (IMineable mineable in mineables)
        {
            mineable.Mine(this);
        }
    }

    public void ReceiveItem(BaseItem item)
    {
        if (toolbarController.AddItemToFreeSlot(item))
        {
            Debug.Log("Item added");
        }
        else
        {
            Debug.Log("Item not added no free slots");
        }
    }
}
