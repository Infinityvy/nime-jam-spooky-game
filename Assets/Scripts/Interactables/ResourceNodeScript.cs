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

        private AudioSource audioSource;
        private Material[] mats;

        private void Start()
        {
            audioSource = GameObject.Find("GlobalSFXSource").GetComponent<AudioSource>();
            mats = GetComponent<MeshRenderer>().materials;
        }

        public Vector3 getHighlightButtonPos()
        {
            return transform.position + Vector3.up * 1.5f;
        }

        public bool Interact(PlayerController playerController)
        {
            health--;

            foreach (Material mat in mats)
            {
                mat.SetFloat("_Health", health * 0.2f);
            }

            if (health > 0)
            {
                return false;
            }

            Invoke(nameof(destroy), GameUtility.mineCycleDuration);
            return true;
        }


        private void destroy()
        {
            if (itemGiven.DroppedItemPrefab)
            {
                Instantiate(itemGiven.DroppedItemPrefab, transform.position + transform.up, transform.rotation);
            }

            audioSource.playSound("rock_breaking");

            Destroy(gameObject);
        }
    }
}