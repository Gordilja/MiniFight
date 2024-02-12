using System.Collections;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    [SerializeField] private PlayerController _PlayerController;
    public GameObject _enemyPlayer; // change later to get from manager enemy player

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 3 && !_PlayerController.isGrounded)
            _PlayerController.isGrounded = true;

        if (collision.gameObject.GetComponent<SwordCollider>())
        {
            _PlayerController.isHit = true;
            _PlayerController._Rb.AddForce(transform.right * 50, ForceMode.Impulse);
            StartCoroutine(StaggerTimer());
            Debug.Log($"{gameObject.name} got hit with sword");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
  
    }

    private IEnumerator StaggerTimer() 
    {
        yield return new WaitForSeconds( 2.0f );
        _PlayerController.isHit = false;
        Debug.Log("Stagger done");
    }
}
