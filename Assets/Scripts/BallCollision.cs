using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallCollision : MonoBehaviour
{
    private bool canHit = true;
    public GameObject game;
    private PongGameController gameController;
    // Start is called before the first frame update
    void Start()
    {
        gameController = game.GetComponent<PongGameController>();
    }

    // Update is called once per frame
    void Update()
    {
    }
    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("LeftGoal")){
            if (canHit){
                if (gameController != null){
                    gameController.rightGoal();
                }
                canHit = false;
            }
            

        }
        if (collision.gameObject.CompareTag("RightGoal")){

            if (canHit){
                if(gameController != null){
                    gameController.leftGoal();
                }
            canHit = false;
            }
        }
    }

    private void OnCollisionExit(Collision other) {
        canHit = true;
    }
}
