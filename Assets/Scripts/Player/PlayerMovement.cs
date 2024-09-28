using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement instance;

    public bool frozen = false;

    [SerializeField]
    private new Rigidbody rigidbody;
    [SerializeField]
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private Animator animator;
    
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


        Vector3 horizontalVelocity = new Vector3(rigidbody.velocity.x, 0, rigidbody.velocity.z);

        bool isSprinting = Input.GetKey(GameInputs.keys["Sprint"]);
        float currentMaxSpeed = (isSprinting ? maxSpeed * sprintMultiplier : maxSpeed);

        if (direction != Vector3.zero
            && horizontalVelocity.magnitude < currentMaxSpeed)
        {
            direction = direction.normalized;
            rigidbody.AddForce(800 * acceleration * Time.deltaTime * direction);
        }
        else if (rigidbody.velocity.x != 0 || rigidbody.velocity.z != 0)
        {
            Vector3 reductionVector = 400.0f * -Time.deltaTime * new Vector3(rigidbody.velocity.x, 0, rigidbody.velocity.z);

            rigidbody.AddForce(reductionVector);
        }


        isMoving = (isMoving || rigidbody.velocity.magnitude > 0.3f);

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

            float dotProduct = Vector3.Dot(rightVector, rigidbody.velocity);

            if (dotProduct > 0) spriteRenderer.flipX = true;
            else spriteRenderer.flipX = false;
        }
    }

    public void animateMiningCycle()
    {
        frozen = true;
        animator.SetInteger("state", (int)AnimationState.MINE);
        Invoke(nameof(stopMiningCycle), GameUtility.mineCycleDuration);
    }

    private void stopMiningCycle()
    {
        frozen = false;
        animator.SetInteger("state", (int)AnimationState.IDLE);
    }
}
