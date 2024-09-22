using UnityEngine;
using UnityEngine.UI;

namespace UI.Toolbar
{
    public class ToolbarSlotUI : MonoBehaviour
    {
        [SerializeField]
        private Image toolBarSlotImage;

        [SerializeField]
        private Image selectedSlotImage;

        private void Awake()
        {
            if (toolBarSlotImage is null)
            {
                throw new UnityException("Toolbar Slot Image Reference Missing In Toolbar Slot UI");
            }

            if (selectedSlotImage is null)
            {
                throw new UnityException("Toolbar Selected Slot Image Reference Missing In Toolbar Slot UI");
            }
        }

        public void UpdateItemSprite(Sprite sprite)
        {
            toolBarSlotImage.sprite = sprite;
            bool imageActive = toolBarSlotImage.sprite;
            toolBarSlotImage.gameObject.SetActive(imageActive);
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