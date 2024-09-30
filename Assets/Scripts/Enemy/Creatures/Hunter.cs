using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hunter : Creature
{
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private Light mouthLight;
    [SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    private AudioSource secondaryAudioSource;

    private float viewDistance = 25f;
    private float maxIdleDurationInSeconds = 5f;

    private Transform player;

    private float patrolSpeed = 2.5f;
    private float huntSpeed = 4.8f;

    private void Start()
    {
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
        agent.speed = huntSpeed;
        agent.SetDestination(player.position);
        animator.SetBool("mouthOpen", true);
        mouthLight.enabled = true;
        audioSource.playSoundIfReady("monster_scream0", 1.5f);
    }

    private void patrol()
    {
        agent.speed = patrolSpeed;
        animator.SetBool("mouthOpen", false);
        mouthLight.enabled = false;
        audioSource.Stop();

        switch (state)
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

    private void OnTriggerStay(Collider other)
    {
        PlayerController player;

        if (other.TryGetComponent<PlayerController>(out player))
        {
            player.dealDamage(0.8f);
            player.GetComponent<Rigidbody>().AddForce((player.transform.position - transform.position).normalized * 35f, ForceMode.Impulse);
            secondaryAudioSource.playSoundIfReady("monster_bite");
        }
    }
}
