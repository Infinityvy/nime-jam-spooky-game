using Models.Items;
using Player;
using UnityEngine;
using IInteractable = Interfaces.IInteractable;

namespace ResourceNode
{
    public class ResourceNodeScript : MonoBehaviour, IInteractable
    {
        [SerializeField]
        private int health = 5;

        [SerializeField]
        private BaseItem itemGiven;

        public Vector3 getHighlightButtonPos()
        {
            return transform.position + Vector3.up * 1.5f;
        }

        public bool Interact(PlayerController playerController)
        {
            health--;
            if (health > 0)
            {
                return false;
            }

            if (itemGiven.DroppedItemPrefab)
            {
                Instantiate(itemGiven.DroppedItemPrefab, transform.position + transform.up, transform.rotation);
            }

            Destroy(gameObject);
            return true;
        }

    }
}