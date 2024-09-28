using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hunter : Creature
{
    private float viewDistance = 25f;
    private float maxIdleDurationInSeconds = 5f;

    private Transform player;

    private Material mat;

    private void Start()
    {
        mat = transform.GetChild(0).GetComponent<MeshRenderer>().material;
        player = GameObject.Find("Player").transform;
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

        if (detectPlayer())
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
        mat.color = Color.red;
        agent.SetDestination(player.position);
    }

    private void patrol()
    {
        mat.color = Color.white;

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

    private bool detectPlayer()
    {
        Vector3 vectorToPlayer = player.position - transform.position;

        if (vectorToPlayer.magnitude > viewDistance) return false;

        if (Physics.Raycast(transform.position + Vector3.up, vectorToPlayer.normalized, vectorToPlayer.magnitude, LayerMask.GetMask("World"))) return false;

        return true;
    }
}
