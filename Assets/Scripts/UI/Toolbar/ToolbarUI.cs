using System.Collections.Generic;
using Extensions;
using Models.Items;
using UnityEngine;

namespace UI.Toolbar
{
    public class ToolbarUI : MonoBehaviour
    {
        [SerializeField] private ToolbarSlotUI toolbarSlotUIPrefab;
        [SerializeField] private RectTransform toolbarPanel;
        private ToolbarSlotUI[] _toolbarSlots;
        private int _selectedSlot = -1;

        public void InitializeToolbar(int toolbarSize)
        {
            _toolbarSlots = new ToolbarSlotUI[toolbarSize];
            for (int i = 0; i < toolbarSize; i++)
            {
                _toolbarSlots[i] = Instantiate(toolbarSlotUIPrefab, Vector3.zero, Quaternion.identity);
                _toolbarSlots[i].transform.SetParent(toolbarPanel);
            }
        }

        public void UpdateToolbarSlots(IList<BaseItem> items)
        {
            for (int i = 0; i < _toolbarSlots.Length; i++)
            {
                if (items[i] is null)
                {
                    _toolbarSlots[i].UpdateItemSprite(null);
                    continue;
                }

                _toolbarSlots[i].UpdateItemSprite(items[i].Sprite);
            }
        }

        public void UpdateSelectedSlot(int targetIndex)
        {
            if (!_toolbarSlots.IsIndexInBounds(targetIndex))
            {
                return;
            }

            if (_toolbarSlots.IsIndexInBounds(_selectedSlot))
            {
                _toolbarSlots[_selectedSlot].DeselectItem();
            }

            _selectedSlot = targetIndex;
            _toolbarSlots[_selectedSlot].SelectItem();
        }
    }
}