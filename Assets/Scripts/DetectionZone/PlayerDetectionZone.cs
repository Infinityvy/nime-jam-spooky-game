using System.Collections.Generic;
using ResourceNode;
using UnityEngine;

namespace DetectionZone
{
    [RequireComponent(typeof(SphereCollider))]
    public class PlayerDetectionZone : MonoBehaviour
    {
        private readonly List<IMineable> _mineables = new List<IMineable>();

        private void OnTriggerEnter(Collider other)
        {
            if (!other)
            {
                return;
            }

            if (other.TryGetComponent(out IMineable mineable))
            {
                _mineables.Add(mineable);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other)
            {
                return;
            }

            if (other.TryGetComponent(out IMineable mineable))
            {
                _mineables.Remove(mineable);
            }
        }

        public IEnumerable<IMineable> GetMineablesNearby()
        {
            return _mineables;
        }
    }
}