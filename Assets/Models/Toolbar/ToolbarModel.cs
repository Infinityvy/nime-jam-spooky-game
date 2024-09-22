using Extensions;
using Models.Items;

namespace Models.Toolbar
{
    public class ToolbarModel
    {
        public BaseItem[] Items { get; }
        public int SelectedIndex { get; private set; }

        public ToolbarModel(int inventorySize)
        {
            Items = new BaseItem[inventorySize];
        }

        public bool SelectSlot(int targetIndex)
        {
            if (!Items.IsIndexInBounds(targetIndex))
            {
                return false;
            }

            SelectedIndex = targetIndex;
            return true;
        }

        public bool AddItemAtSelectedSlot(BaseItem itemToAdd)
        {
            if (!Items.IsIndexInBounds(SelectedIndex))
            {
                return false;
            }

            Items[SelectedIndex] = itemToAdd;
            return true;
        }

        public bool RemoveItemAtSelectedSlot()
        {
            if (!Items.IsIndexInBounds(SelectedIndex))
            {
                return false;
            }

            Items[SelectedIndex] = null;
            return true;
        }

        public BaseItem GetSelectedItem()
        {
            return Items.IsIndexInBounds(SelectedIndex) ? Items[SelectedIndex] : null;
        }
    }
}