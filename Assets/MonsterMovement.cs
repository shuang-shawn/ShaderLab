using UnityEngine;

public class MonsterAI : MonoBehaviour
{
    public float moveSpeed = 2f;               // Speed of the monster
    public float detectionDistance = 0.5f;     // Distance to detect walls in front and sides
    public float turnCooldown = 1f;            // Cooldown time between turns
    public float turnDelay = 0.5f;             // Delay time before turning after detecting a turn

    private Vector3 moveDirection;
    private float turnTimer;                    // Timer to manage turning cooldown
    private float delayTimer;                   // Timer to manage the delay before turning
    private bool isTurning;                     // Indicates if the monster is currently turning
    private Vector3 newDirection;               // The new direction to turn to
    // private bool turnRight = true;

    private void Start()
    {
        moveDirection = transform.forward; // Set initial direction
        turnTimer = 0f;                    // Initialize turn timer
        delayTimer = 0f;                   // Initialize delay timer
        isTurning = false;                  // Initially not turning
    }

    private void Update()
    {
        MoveMonster();
    }

    private void MoveMonster()
    {
        // Move the monster in the current direction
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime, Space.World);

        // Reduce the turn cooldown timer
        turnTimer -= Time.deltaTime;
        if (!isTurning) {
            // Check for available turns and obstacles in front
            if (ShouldTurn())
            {
                // Start the delay timer before turning
                delayTimer = turnDelay;
                isTurning = true; // Indicate that turning is initiated
            }
        }


        // If turning, manage the delay timer
        if (isTurning)
        {
            delayTimer -= Time.deltaTime; // Count down the delay timer
            if (delayTimer <= 0f)
            {
                // After the delay, perform the turn instantly
                Turn();
                isTurning = false; // Reset turning state
            }
        }
    }

    private bool ShouldTurn()
    {
        // Detect walls on the left, right, and directly in front
        bool wallAhead = Physics.Raycast(transform.position, moveDirection, detectionDistance);
        bool openRight = !Physics.Raycast(transform.position, transform.right, detectionDistance);
        bool openLeft = !Physics.Raycast(transform.position, -transform.right, detectionDistance);

        // Turn if there is an open side or if a wall is ahead and cooldown has expired
        return (wallAhead || openRight || openLeft) && turnTimer <= 0f && !isTurning;
    }

    private void Turn()
    {
        bool openRight = !Physics.Raycast(transform.position, transform.right, detectionDistance);
        bool openLeft = !Physics.Raycast(transform.position, -transform.right, detectionDistance);

        // Prioritize turning right if available, otherwise turn left
        if (openRight && openLeft) {
            if (Random.value < 0.5f) {
                newDirection = transform.right;
            } else {
                newDirection = -transform.right;
            }
            
        }
        else if (openRight)
        {
            newDirection = transform.right;
        }
        else if (openLeft)
        {
            newDirection = -transform.right;
        }
        else
        {
            // If neither left nor right is available, turn around
            newDirection = -moveDirection;
        }

        // Set the new direction instantly
        moveDirection = newDirection;
        transform.rotation = Quaternion.LookRotation(moveDirection);

        // Reset the turn cooldown timer
        turnTimer = turnCooldown;
    }
}
