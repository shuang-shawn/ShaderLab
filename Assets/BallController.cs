using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    private Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    /// <summary>
    /// OnCollisionEnter is called when this collider/rigidbody has begun
    /// touching another rigidbody/collider.
    /// </summary>
    /// <param name="other">The Collision data associated with this collision.</param>
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("wall") || other.gameObject.CompareTag("enemy")) {
            // Vector3 normal = other.contacts[0].normal; // Get collision normal
            // Vector3 reflectedVelocity = Vector3.Reflect(rb.velocity, normal); // Calculate reflection
            // rb.velocity = reflectedVelocity.normalized * rb.velocity.magnitude; // Apply reflection

            // // Apply a small force to separate from the wall
            // rb.AddForce(normal * 0.1f, ForceMode.Impulse);

            // Debug.Log("Bounced off: " + other.gameObject.name);

            if (other.gameObject.CompareTag("enemy")) {
                other.gameObject.GetComponent<MonsterAI>().GetHit();
                int currentScore = PlayerPrefs.GetInt("MazeScore");
                PlayerPrefs.SetInt("MazeScore", currentScore + 1);
                Destroy(gameObject);
            }
            if (AudioController.aCtrl != null) {
                AudioController.aCtrl.PlaySFX();
            }
        }
    }

    
}
