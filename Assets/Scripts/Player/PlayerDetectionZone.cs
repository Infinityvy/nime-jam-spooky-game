using System.Collections.Generic;
using System.Linq;
using Interfaces;
using UnityEngine;

namespace DetectionZone
{
    [RequireComponent(typeof(SphereCollider))]
    public class PlayerDetectionZone : MonoBehaviour
    {
        public Camera mainCamera;
        public RectTransform interactionKey;

        private readonly List<IInteractable> _interactables = new List<IInteractable>();

        private IInteractable closestInteractable = null;

        private void OnTriggerEnter(Collider other)
        {
            if (!other)
            {
                return;
            }

            if (other.TryGetComponent(out IInteractable interactable))
            {
                _interactables.Add(interactable);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other)
            {
                return;
            }

            if (other.TryGetComponent(out IInteractable interactable))
            {
                _interactables.Remove(interactable);
            }
        }

        public void RemoveIMineableFromList(IInteractable interactable)
        {
            _interactables.Remove(interactable);
        }

        public IEnumerable<IInteractable> GetMineablesNearby(Transform position)
        {
            if (_interactables is null)
            {
                return Enumerable.Empty<IInteractable>();
            }

            return _interactables.OrderBy(interactable =>
            {
                MonoBehaviour mono = interactable as MonoBehaviour;
                if (!mono)
                {
                    return float.MaxValue;
                }

                return Vector3.Distance(position.position, mono.transform.position);
            });
        }

        private void Update()
        {
            closestInteractable = getClosestInteractable(transform.position);

            if (closestInteractable == null || PlayerMovement.instance.frozen)
            {
                interactionKey.gameObject.SetActive(false);
            }
            else
            {
                interactionKey.gameObject.SetActive(true);
                interactionKey.position = mainCamera.WorldToScreenPoint(closestInteractable.getHighlightButtonPos());
            }
        }

        public IInteractable getClosestInteractable(Vector3 position)
        {
            if (_interactables.Count == 0) return null;

            (IInteractable, float) closest = (null, float.MaxValue);

            foreach(IInteractable interactable in _interactables)
            {
                MonoBehaviour mono = interactable as MonoBehaviour;

                float distance = Vector3.Distance(position, mono.transform.position);

                if(closest.Item2 >  distance) closest = (interactable, distance);
            }

            return closest.Item1;
        }
    }
}