using System.Collections.Generic;
using System.Linq;
using DetectionZone;
using Models.Items;
using ResourceNode;
using Toolbar;
using UnityEngine;
using UnityEngine.SceneManagement;
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

        private float maxHealth = 1f;
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

        private float timeWhenLastInteracted = 0;
        public void InteractWithNearbyObject()
        {
            if (!playerDetectionZone || PlayerMovement.instance.frozen || Time.time - timeWhenLastInteracted < GameUtility.mineCycleDuration)
            {
                return;
            }

            if (!Input.GetKey(GameInputs.keys["Interact"]))
            {
                return;
            }

            IInteractable interactable = playerDetectionZone.getClosestInteractable(transform.position);
            if (interactable is null)
            {
                return;
            }


            timeWhenLastInteracted = Time.time;

            if (interactable is ResourceNodeScript) PlayerMovement.instance.animateMiningCycle();
            if (interactable is ItemScripts.ItemScript) audioSource.playSound("pickup", 0.5f); 

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

            audioSource.playSound("hurt", 1f);

            if (health < 0) die();
        }

        private void regenHealth()
        {
            health += healthRegen * Time.deltaTime;

            health =  Mathf.Clamp(health, 0, maxHealth);

            float alpha = 1f - ((health - 0.2f) / 0.8f);
            bloodScreen.color = new Color(1, 1, 1, alpha * 0.8f);
        }

        private void die()
        {
            //PlayerMovement.instance.animateDeath();
            Session.instance.finalizeLevel(true);
        }
    }
}
