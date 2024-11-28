using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPaddle : MonoBehaviour
{
    public Transform ball;
    public float speed = 10f;
    public float yOffset = 0.5f;
    private Rigidbody rb;
    private int mode = 0;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        mode = PlayerPrefs.GetInt("mode");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void FixedUpdate() {
        if (ball != null && mode == 1) {
            // Debug.Log(ball.position);

            float targetZ = ball.position.z + yOffset;
            Vector3 targetPosition = new Vector3(transform.position.x, transform.position.y, targetZ);

            // Move the paddle towards the target position
            MovePaddle(targetPosition);
        } else {
            if (mode == 0) {
                // Debug.Log("PVP mode");
            }

        
        }
     }

     void MovePaddle(Vector3 targetPosition)
    {
        // Debug.Log("inside move paddle");

        // Calculate the new position for the paddle
        Vector3 newPosition = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
        
        // Update the paddle's position
        rb.MovePosition(newPosition);
    }
}
