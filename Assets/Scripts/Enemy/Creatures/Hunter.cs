using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hunter : Creature
{
    private float maxIdleDurationInSeconds = 5f;

    public Transform player;

    private void Start()
    {

    }

    private void FixedUpdate()
    {
        if (agent.pathPending) return;

        if (agent.velocity.magnitude > 0)
        {
            transform.LookAt(transform.position + agent.velocity);
            state = CreatureState.MOVING;
        }
        else if(agent.remainingDistance <= agent.stoppingDistance)
        {
            state = CreatureState.STOPPED;
        }

        if (player != null)
        {
            huntPlayer();
        }
        else if (state != CreatureState.IDLE)
        {
            patrol();
        }
    }

    private void huntPlayer()
    {
        agent.SetDestination(player.position);
    }

    private void patrol()
    {
        switch(state)
        {
            case CreatureState.MOVING:
                break;
            case CreatureState.STOPPED:
                state = CreatureState.IDLE;
                StartCoroutine(nameof(idleForRandomDuration));
                break;
            default:
                break;
        }
    }

    private IEnumerator idleForRandomDuration()
    {
        yield return new WaitForSeconds(Random.Range(0, maxIdleDurationInSeconds));

        setRandomDestination();

        state = CreatureState.MOVING;
    }

    private void setRandomDestination()
    {
        agent.SetDestination(WorldGenerator.instance.getRandomNodePosition());
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer != LayerMask.NameToLayer("Player"))
        {
            player = other.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            player = null;
        }
    }
}
