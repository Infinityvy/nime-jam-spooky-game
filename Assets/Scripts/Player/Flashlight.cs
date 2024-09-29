using UnityEngine;

namespace Player
{
    public class Flashlight : MonoBehaviour
    {
        public Camera mainCamera;
        public Transform test;

        private new Light light;

        private void Start()
        {
            light = GetComponent<Light>();
        }

        private void Update()
        {
            if(Session.instance.paused) return;

            //rotateFlashlightToCursor();
            lookAtCursor();
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

        private void lookAtCursor()
        {
            Ray ray = mainCamera.ScreenPointToRay(GameUtility.clampMousePosition(Input.mousePosition));
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                Vector3 direction = hit.point - transform.position;
                test.position = hit.point;
                //direction.y = 0; // Optional: keep the object level on the y-axis
                transform.forward = direction.normalized;
            }
        }
    }
}
