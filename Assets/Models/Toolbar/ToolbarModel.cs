using System.Collections.Generic;
using Extensions;
using Models.Items;

namespace Models.Toolbar
{
    public class ToolbarModel
    {
        public BaseItem[] Items { get; private set; }
        private int _selectedIndex = -1;

        public void Initialize(int inventorySize)
        {
            Items = new BaseItem[inventorySize];
        }

        public void SetItems(IList<BaseItem> items)
        {
            for (int i = 0; i < Items.Length; i++)
            {
                Items[i] = items[i];
            }
        }

        public bool SelectSlot(int targetIndex)
        {
            if (!Items.IsIndexInBounds(targetIndex))
            {
                return false;
            }

            _selectedIndex = targetIndex;
            return true;
        }

        public bool AddItemAtSelectedSlot(BaseItem itemToAdd)
        {
            if (!Items.IsIndexInBounds(_selectedIndex))
            {
                return false;
            }

            Items[_selectedIndex] = itemToAdd;
            return true;
        }

        public bool RemoveItemAtSelectedSlot()
        {
            if (!Items.IsIndexInBounds(_selectedIndex))
            {
                return false;
            }

            Items[_selectedIndex] = null;
            return true;
        }

        public BaseItem GetSelectedItem()
        {
            return Items[_selectedIndex];
        }
    }
}