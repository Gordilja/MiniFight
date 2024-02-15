using System.Collections;
using UnityEngine;

public class PlayerDistanceControl : MonoBehaviour
{
    [SerializeField] private PlayerController _PlayerController;
    [SerializeField] private PlayerHP _PlayerHP;
    public GameObject _enemyPlayer; // change later to get from manager enemy player

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 3 && !_PlayerController.isGrounded)
        {
            _PlayerController.isGrounded = true;
        }
    }
    public void CheckPlayerDistance() 
    {
        var distanceToEnemy = Vector3.Distance(transform.position, _enemyPlayer.transform.position);
        var heightMatching = transform.position.y - _enemyPlayer.transform.position.y;

        if (distanceToEnemy < 3.25f && heightMatching <= 1.2f && heightMatching >= 0)
        {
            _enemyPlayer.GetComponent<PlayerHP>().DealDmg(_PlayerController._PlayerData.attack);
            Debug.Log("Hit enemy");
        }
        else Debug.Log("Too far to hit");
    }
}