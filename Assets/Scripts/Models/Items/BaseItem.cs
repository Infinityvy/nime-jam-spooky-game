using UnityEngine;

namespace Models.Items
{
    public abstract class BaseItem : ScriptableObject
    {
        public int Id => GetInstanceID();

        [field: SerializeField]
        public string Name { get; private set; }

        [field: SerializeField]
        public Sprite Sprite { get; private set; }

        [field: SerializeField]
        public GameObject DroppedItemPrefab { get; private set; }
    }
}