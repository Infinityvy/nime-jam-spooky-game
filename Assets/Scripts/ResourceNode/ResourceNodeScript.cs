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

        public bool Interact(PlayerController playerController)
        {
            health--;
            if (health > 0)
            {
                return false;
            }

            if (!playerController.ReceiveItem(itemGiven))
            {
                return false;
            }
            
            Explode();
            return true;
        }

        private void Explode()
        {
            Destroy(gameObject);
        }
    }
}