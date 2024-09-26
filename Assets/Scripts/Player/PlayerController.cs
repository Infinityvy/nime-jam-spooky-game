using Models.Items;
using System.Collections;
using System.Collections.Generic;
using Toolbar;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private ToolbarController toolbarController;

    public BaseItem[] testItems;

    void Update()
    {
        InteractWithInventory();
    }

    private void InteractWithInventory()
    {
        if (Session.instance.paused) return;

        for (int i = 0; i < toolbarController.inventorySize; i++)
        {
            if (Input.GetKey(GameInputs.keys["Slot " + (i + 1)]))
            {
                toolbarController.SelectSlot(i);
            }
        }

        if(Input.mouseScrollDelta.y < 0)
        {
            toolbarController.SelectSlot(GameUtility.loopIndex(toolbarController.getSelectedSlotIndex() + 1, toolbarController.inventorySize));
        }
        else if(Input.mouseScrollDelta.y > 0)
        {
            toolbarController.SelectSlot(GameUtility.loopIndex(toolbarController.getSelectedSlotIndex() - 1, toolbarController.inventorySize));
        }

        if (Input.GetKeyDown(GameInputs.keys["Drop Item"]))
        {
            toolbarController.RemoveItemAtSelectedSlot();
        }

        if (Input.GetKeyDown(GameInputs.keys["Interact"]))
        {
            int randomIndex = Random.Range(0, 2);
            toolbarController.AddItemAtSelectedSlot(testItems[randomIndex]);
        }

        if (Input.GetKeyDown(GameInputs.keys["Use Item"]))
        {
            toolbarController.InteractWithSelectedItem();
        }
    }
}
