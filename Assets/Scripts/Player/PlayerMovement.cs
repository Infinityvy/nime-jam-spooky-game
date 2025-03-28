using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement instance;

    public bool frozen { get; private set; } = false;

    public Action playerIsMoving;

    [SerializeField]
    private Rigidbody playerRigid;
    [SerializeField]
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private ParticleSystem particles;
    
    public bool isMoving = false;

    private float maxSpeed = 4.5f;
    private float acceleration = 1f;
    private float sprintMultiplier = 1.5f;

    private enum AnimationState
    {
        IDLE, WALK, RUN, MINE, DIE
    }


    void Awake()
    {
        instance = this;
    }

    void Update()
    {
        if (Session.instance.paused) return;

        move();
    }

    private void move()
    {
        isMoving = false;

        if(frozen) return;

        Vector3 direction = Vector3.zero;

        if (Input.GetKey(GameInputs.keys["Forward"]))
        {
            direction += new Vector3(1, 0, 1);
            isMoving = true;
        }
        if (Input.GetKey(GameInputs.keys["Back"]))
        {
            direction += new Vector3(-1, 0, -1);
            isMoving = true;
        }
        if (Input.GetKey(GameInputs.keys["Left"]))
        {
            direction += new Vector3(-1, 0, 1);
            isMoving = true;
        }
        if (Input.GetKey(GameInputs.keys["Right"]))
        {
            direction += new Vector3(1, 0, -1);
            isMoving = true;
        }


        Vector3 horizontalVelocity = new Vector3(playerRigid.velocity.x, 0, playerRigid.velocity.z);

        bool isSprinting = Input.GetKey(GameInputs.keys["Sprint"]);
        float currentMaxSpeed = (isSprinting ? maxSpeed * sprintMultiplier : maxSpeed);

        if (direction != Vector3.zero
            && horizontalVelocity.magnitude < currentMaxSpeed)
        {
            direction = direction.normalized;
            playerRigid.AddForce(800 * acceleration * Time.deltaTime * direction);
        }
        else if (playerRigid.velocity.x != 0 || playerRigid.velocity.z != 0)
        {
            Vector3 reductionVector = 400.0f * -Time.deltaTime * new Vector3(playerRigid.velocity.x, 0, playerRigid.velocity.z);

            playerRigid.AddForce(reductionVector);
        }


        isMoving = (isMoving || playerRigid.velocity.magnitude > 0.3f);
        if (isMoving) playerIsMoving.Invoke();

        // animate
        if (!isMoving)
        {
            animator.SetInteger("state", (int)AnimationState.IDLE);
        }
        else if (isSprinting)
        {
            animator.SetInteger("state", (int)AnimationState.RUN);
        }
        else
        {
            animator.SetInteger("state", (int)AnimationState.WALK);
        }

        if(isMoving)
        {
            Vector3 rightVector = new Vector3(1, 0, -1).normalized;

            float dotProduct = Vector3.Dot(rightVector, playerRigid.velocity);


            if (dotProduct > 0)
            {
                spriteRenderer.flipX = true;
                particles.transform.localPosition = new Vector3(1, 0.8f, -1);
            }
            else
            {
                spriteRenderer.flipX = false;
                particles.transform.localPosition = new Vector3(-1, 0.8f, 1);
            }
        }
    }

    public void freezePlayer()
    {
        frozen = true;
        playerRigid.constraints = RigidbodyConstraints.FreezeAll;
        animator.SetInteger("state", (int)AnimationState.IDLE);
    }

    public void unfreezePlayer()
    {
        frozen = false;
        playerRigid.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;
    }

    public void freezePlayerForDuration(float duration)
    {
        freezePlayer();
        Invoke(nameof(unfreezePlayer), duration);
    }

    public void animateMiningCycle()
    {
        freezePlayer();
        animator.SetInteger("state", (int)AnimationState.MINE);
        Invoke(nameof(stopMiningCycle), GameUtility.mineCycleDuration);
    }

    private void stopMiningCycle()
    {
        unfreezePlayer();
        animator.SetInteger("state", (int)AnimationState.IDLE);
    }

    public void animateDeath()
    {
        freezePlayer();
        animator.SetInteger("state", (int)AnimationState.DIE);
    }
}
