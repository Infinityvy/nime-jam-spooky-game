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
            Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 0);

            Vector3 mousePosition = clampMousePosition(Input.mousePosition);

            float xOffset = mousePosition.x - screenCenter.x;
            float zOffset = mousePosition.y - screenCenter.y;

            float angle = Vector3.Angle(Vector3.forward, new Vector3(xOffset, 0, zOffset));

            angle *= xOffset > 0 ? 1 : -1;

            transform.rotation = Quaternion.Euler(30, angle + 45, 0f);
        }

        private Vector3 clampMousePosition(Vector3 mousePosition)
        {
            float x = Mathf.Clamp(mousePosition.x, 0, Screen.width);
            float y = Mathf.Clamp(mousePosition.y, 0, Screen.height);

            return new Vector3(x, y, 0);
        }
    }
}
