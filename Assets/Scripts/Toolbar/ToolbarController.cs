using Models.Items;
using Models.Toolbar;
using UI.Toolbar;
using UnityEngine;

namespace Toolbar
{
    public class ToolbarController : MonoBehaviour
    {
        public int inventorySize { private set; get; } = 4;

        [SerializeField]
        private ToolbarUI toolbarUI;

        private ToolbarModel _toolbar;

        private void Awake()
        {
            if (toolbarUI is null)
            {
                throw new UnityException("Toolbar UI reference missing");
            }

            _toolbar = new ToolbarModel(inventorySize);
            toolbarUI.InitializeToolbar(inventorySize);
            toolbarUI.UpdateToolbarSlots(_toolbar.Items);
            toolbarUI.UpdateSelectedSlot(_toolbar.SelectedIndex);
        }

        public void SelectSlot(int targetIndex)
        {
            if (!_toolbar.SelectSlot(targetIndex))
            {
                return;
            }

            toolbarUI.UpdateSelectedSlot(targetIndex);
        }

        public void RemoveItemAtSelectedSlot()
        {
            if (!_toolbar.RemoveItemAtSelectedSlot())
            {
                return;
            }

            toolbarUI.UpdateToolbarSlots(_toolbar.Items);
        }

        public void AddItemAtSelectedSlot(BaseItem item)
        {
            if (!_toolbar.AddItemAtSelectedSlot(item))
            {
                return;
            }

            toolbarUI.UpdateToolbarSlots(_toolbar.Items);
        }

        public void InteractWithSelectedItem()
        {
            BaseItem selectedItem = _toolbar.GetSelectedItem();
            switch (selectedItem)
            {
                case null:
                    return;
                case IInteractable toggleableItem:
                    toggleableItem.Toggle();
                    break;
            }
        }

        public int getSelectedSlotIndex()
        {
            return _toolbar.SelectedIndex;
        }
    }
}