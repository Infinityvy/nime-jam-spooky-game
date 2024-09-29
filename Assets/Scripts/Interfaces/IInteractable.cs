using Player;
using UnityEngine;

namespace Interfaces
{
    public interface IInteractable
    {
        bool Interact(PlayerController playerController);
        Vector3 getHighlightButtonPos();
    }
}