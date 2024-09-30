using System.Collections.Generic;
using System.Linq;
using DetectionZone;
using Models.Items;
using ResourceNode;
using Toolbar;
using UnityEngine;
using UnityEngine.UI;
using IInteractable = Interfaces.IInteractable;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField]
        private ToolbarController toolbarController;

        [SerializeField]
        private PlayerDetectionZone playerDetectionZone;

        [SerializeField]
        private Image bloodScreen;

        [SerializeField]
        private AudioSource audioSource;

        private float health = 1f;
        private float healthRegen = 0.04f;

        private float timeWhenLastDamaged = 0;
        private float invincibilityTime = 0.5f;

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
            if (Input.GetKeyDown(KeyCode.H)) dealDamage(0.2f);

            InteractWithInventory();
            InteractWithNearbyObject();

            regenHealth();
        }

        private void DropItem()
        {
            BaseItem selectedItem = toolbarController.GetSelectedSlotItem();
            if (!selectedItem)
            {
                return;
            }

            float cursorAngle = GameUtility.getCursorAngleRelativeToPlayer();

            Vector3 cursorDirection = Quaternion.Euler(0, cursorAngle, 0) * Vector3.forward;

            Vector3 dropPos = transform.position + Vector3.up + cursorDirection;

            Instantiate(selectedItem.DroppedItemPrefab, dropPos, Quaternion.identity);
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
            if (!playerDetectionZone || PlayerMovement.instance.frozen)
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

            if (interactable is ResourceNodeScript) PlayerMovement.instance.animateMiningCycle();

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

        public void dealDamage(float damage)
        {
            if (Time.time - timeWhenLastDamaged < invincibilityTime) return;

            timeWhenLastDamaged = Time.time;

            health -= damage;

            audioSource.playSound("hurt", 1f, 2f);

            if (health < 0) die();
        }

        private void regenHealth()
        {
            health += healthRegen * Time.deltaTime;

            float alpha = 1f - ((health - 0.2f) / 0.8f);
            bloodScreen.color = new Color(1, 1, 1, alpha);
        }

        private void die()
        {
            PlayerMovement.instance.animateDeath();
        }
    }
}
