using UnityEngine;

namespace Player
{
    public class Flashlight : MonoBehaviour
    {
        public Camera mainCamera;

        public AudioSource audioSource;

        private Light lamp;

        private void Start()
        {
            lamp = GetComponent<Light>();
        }

        private void Update()
        {
            if(Session.instance.paused) return;

            lookAtCursor();
            toggleFlashlight();
        }

        private void toggleFlashlight()
        {
            if (Input.GetKeyDown(GameInputs.keys["Toggle Flashlight"]))
            {
                lamp.enabled = !lamp.enabled;
                audioSource.playSound("flashlight_" + (lamp.enabled ? "on" : "off"));
            }
        }

        //private void rotateFlashlightToCursor()
        //{
        //    float angle = GameUtility.getCursorAngleRelativeToPlayer();

        //    transform.rotation = Quaternion.Euler(30, angle, 0f);
        //}

        private void lookAtCursor()
        {
            Ray ray = mainCamera.ScreenPointToRay(GameUtility.clampMousePosition(Input.mousePosition));
            RaycastHit hit;

            int layerMask = LayerMask.GetMask("Default", "World", "Creature");

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
            {
                Vector3 direction = hit.point - transform.position;
                transform.forward = direction.normalized;
            }
        }
    }
}
