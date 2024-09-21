using UnityEngine;
using UnityEngine.UI;

namespace UI.Toolbar
{
    public class ToolbarSlotUI : MonoBehaviour
    {
        [SerializeField] private Image toolBarSlotImage;
        [SerializeField] private Image selectedSlotImage;

        public void UpdateItemSprite(Sprite sprite)
        {
            toolBarSlotImage.sprite = sprite;
            toolBarSlotImage.gameObject.SetActive(!Equals(toolBarSlotImage.sprite, null));
        }

        public void SelectItem()
        {
            selectedSlotImage.gameObject.SetActive(true);
        }

        public void DeselectItem()
        {
            selectedSlotImage.gameObject.SetActive(false);
        }
    }
}