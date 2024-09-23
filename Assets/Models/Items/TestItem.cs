using UnityEngine;

namespace Models.Items
{
    [CreateAssetMenu(menuName = "Create TestItem", fileName = "TestItem", order = 0)]
    public class TestItem : BaseItem
    {
        [field: SerializeField]
        public string TestProperty { get; private set; }
    }
}