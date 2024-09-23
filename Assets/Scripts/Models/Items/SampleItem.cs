using UnityEngine;

namespace Models.Items
{
    [CreateAssetMenu(menuName = "Create SampleItem", fileName = "SampleItem", order = 0)]
    public class SampleItem : BaseItem
    {
        [field: SerializeField]
        public string SampleProperty { get; private set; }
    }
}