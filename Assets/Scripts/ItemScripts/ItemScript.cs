using Interfaces;
using Models.Items;
using Player;
using UnityEngine;

namespace ItemScripts
{
    public class ItemScript : MonoBehaviour, IInteractable
    {
        [SerializeField]
        private BaseItem itemData;

        public bool Interact(PlayerController playerController)
        {
            bool pickedUp = playerController.ReceiveItem(itemData);
            if(pickedUp) Destroy(gameObject);
            return pickedUp;
        }

        public Vector3 getHighlightButtonPos()
        {
            return transform.position + Vector3.up * 0.5f;
        }
    }
}