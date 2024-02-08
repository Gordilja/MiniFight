using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialMover : MonoBehaviour
{

    public float speed = 0.3f;
    Renderer _rnd;
    Material _waterReference;

    void Start()
    {
        _rnd = GetComponent<Renderer>();
        _waterReference = _rnd.material;
    }

    private void Update()
    {
        var currentSpeed = Time.time * speed;
        _waterReference.mainTextureOffset = new Vector2(0, currentSpeed);
    }
    private void LateUpdate()
    {
        _rnd.material = _waterReference;
    }
}