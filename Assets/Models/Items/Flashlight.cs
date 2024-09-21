using UnityEngine;

namespace Models.Items
{
    [CreateAssetMenu(menuName = "Create Flashlight", fileName = "Flashlight", order = 0)]
    public class Flashlight : BaseItem, IInteractable
    {
        [SerializeField]
        private Player.Flashlight flashlight;

        private bool _toggle;

        public void Toggle()
        {
            _toggle = !_toggle;
            flashlight.toggleFlashlight(_toggle);
        }
    }
}