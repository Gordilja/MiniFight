using System.Collections;
using UnityEngine;

public class FollowPlayerScript : MonoBehaviour
{
    public Transform objectToFollow;
    public RectTransform imageRect;
    public Material shockWave;
    public Camera Cam;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C)) 
        {
            StartCoroutine(ShockwaveEffect());
        }
    }

    private Vector2 FollowPlayer()
    {
        // Convert world position of object to screen point
        var playerPos = new Vector3(objectToFollow.position.x, objectToFollow.position.y + 1, objectToFollow.position.z);
        Vector2 screenPos = Cam.WorldToScreenPoint(playerPos).normalized;

        return screenPos;
    }

    private IEnumerator ShockwaveEffect() 
    {
        var vector2 = FollowPlayer();
        //shockWave.SetVector("_FocalPoint", vector2);
        shockWave.SetFloat("_Speed", 0.6f);
        yield return new WaitForSeconds(1);
        shockWave.SetFloat("_Speed", 0f);
    }
}