using UnityEngine;

namespace Player
{
    public class Flashlight : MonoBehaviour
    {
        public static Flashlight instance;

        private new Light light;

        private void Start()
        {
            light = GetComponent<Light>();
            instance = this;
        }

        private void Update()
        {
            if(Session.instance.paused) return;

            rotateFlashlightToCursor();
            toggleFlashlight();
        }

        private void toggleFlashlight()
        {
            if (Input.GetKeyDown(GameInputs.keys["Toggle Flashlight"]))
            {
                light.enabled = !light.enabled;
            }
        }

        private void rotateFlashlightToCursor()
        {
            float angle = GameUtility.getCursorAngleRelativeToPlayer();

            transform.rotation = Quaternion.Euler(30, angle, 0f);
        }
    }
}
