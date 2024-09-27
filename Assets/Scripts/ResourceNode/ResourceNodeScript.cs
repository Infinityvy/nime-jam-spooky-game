using Models.Items;
using UnityEngine;

namespace ResourceNode
{
    public class ResourceNodeScript : MonoBehaviour, IMineable
    {
        [SerializeField]
        private int health = 5;

        [SerializeField]
        private BaseItem itemGiven;

        public bool Mine(PlayerController playerController)
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