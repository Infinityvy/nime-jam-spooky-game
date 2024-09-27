using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class Creature : MonoBehaviour
{
    public NavMeshAgent agent;

    public float health { get; protected set; } = 100.0f;

    public enum CreatureState
    {
        IDLE, MOVING, STOPPED 
    }

    public CreatureState state = CreatureState.MOVING;
}
