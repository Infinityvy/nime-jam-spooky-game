using Models.Items;
using Models.Toolbar;
using UI.Toolbar;
using UnityEngine;

namespace Toolbar
{
    public class ToolbarController : MonoBehaviour
    {
        [SerializeField]
        private int inventorySize;

        [SerializeField]
        private ToolbarUI toolbarUI;

        private ToolbarModel _toolbar;
        public BaseItem[] testItems;

        private void Awake()
        {
            _toolbar = new ToolbarModel();
            _toolbar.Initialize(inventorySize);
            _toolbar.SetItems(testItems);
            toolbarUI.InitializeToolbar(inventorySize);
            toolbarUI.UpdateToolbarSlots(_toolbar.Items);
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
    }
}