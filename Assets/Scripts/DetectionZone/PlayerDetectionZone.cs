using System.Collections.Generic;
using System.Linq;
using Interfaces;
using UnityEngine;

namespace DetectionZone
{
    [RequireComponent(typeof(SphereCollider))]
    public class PlayerDetectionZone : MonoBehaviour
    {
        private readonly List<IInteractable> _interactables = new List<IInteractable>();

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
    }
}