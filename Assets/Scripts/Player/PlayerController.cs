using System.Collections.Generic;
using System.Linq;
using DetectionZone;
using Models.Items;
using Toolbar;
using UnityEngine;
using IInteractable = Interfaces.IInteractable;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField]
        private ToolbarController toolbarController;

        [SerializeField]
        private PlayerDetectionZone playerDetectionZone;

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

        private void Update()
        {
            InteractWithInventory();
            InteractWithNearbyObject();
        }

        private void DropItem()
        {
            BaseItem selectedItem = toolbarController.GetSelectedSlotItem();
            if (!selectedItem)
            {
                return;
            }

            Instantiate(selectedItem.DroppedItemPrefab, transform.position + transform.up, transform.rotation);
            toolbarController.RemoveItemAtSelectedSlot();
        }

        private void InteractWithInventory()
        {
            if (Session.instance.paused)
            {
                return;
            }

            for (int i = 0; i < toolbarController.inventorySize; i++)
            {
                if (Input.GetKey(GameInputs.keys["Slot " + (i + 1)]))
                {
                    toolbarController.SelectSlot(i);
                }
            }

            if (Input.mouseScrollDelta.y < 0)
            {
                toolbarController.SelectSlot(GameUtility.loopIndex(toolbarController.getSelectedSlotIndex() + 1,
                    toolbarController.inventorySize));
            }
            else if (Input.mouseScrollDelta.y > 0)
            {
                toolbarController.SelectSlot(GameUtility.loopIndex(toolbarController.getSelectedSlotIndex() - 1,
                    toolbarController.inventorySize));
            }

            if (Input.GetKeyDown(GameInputs.keys["Drop Item"]))
            {
                DropItem();
            }

            if (Input.GetKeyDown(GameInputs.keys["Use Item"]))
            {
                toolbarController.InteractWithSelectedItem();
            }
        }

        public void InteractWithNearbyObject()
        {
            if (!playerDetectionZone)
            {
                return;
            }

            if (!Input.GetKeyDown(GameInputs.keys["Interact"]))
            {
                return;
            }

            IEnumerable<IInteractable> interactables = playerDetectionZone.GetMineablesNearby(transform);
            IInteractable interactable = interactables.FirstOrDefault();
            if (interactable is null)
            {
                return;
            }

            bool mined = interactable.Interact(this);
            if (mined)
            {
                playerDetectionZone.RemoveIMineableFromList(interactable);
            }
        }

        public bool ReceiveItem(BaseItem item)
        {
            return toolbarController.AddItemToFreeSlot(item);
        }
    }
}
