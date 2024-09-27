using UnityEngine;

namespace Models.ResourceNodes
{
    [CreateAssetMenu(menuName = "Create ResourceNodeSpawnData", fileName = "ResourceNodeSpawnData", order = 0)]
    public class ResourceNodeSpawnData : ScriptableObject
    {
        [SerializeField]
        private GameObject[] resourceNodePrefabs;
    }
}