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

        public void Mine(PlayerController playerController)
        {
            health--;
            if (health > 0)
            {
                return;
            }

            playerController.ReceiveItem(itemGiven);
            Explode();
        }

        private void Explode()
        {
            Destroy(gameObject);
        }
    }
}