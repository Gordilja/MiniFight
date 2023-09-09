using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.layer == 3 && !playerController.isGrounded)
    //    {
    //        playerController.isGrounded = true;
    //        playerController.canDoubleJump = true;
    //        Debug.Log("Col");
    //    }
    //}
}
