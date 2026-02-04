using UnityEngine;

[RequireComponent(typeof(Collider))]
public class GroundChecker : MonoBehaviour
{
    [SerializeField] private LayerMask _GroundMask;
    [SerializeField] private float extraDistance = 0.05f;   // how far below feet we still count as grounded
    [SerializeField, Range(0f, 1f)] private float minGroundNormalY = 0.7f; // ~45°

    [SerializeField] private PlayerController _Controller;
    [SerializeField] private Collider _Col;

    private void Awake()
    {
        if (_Col == null)
            _Col = GetComponent<Collider>();

        if (_Controller == null)
            _Controller = GetComponentInParent<PlayerController>();

        if (_Col == null || _Controller == null)
        {
            Debug.LogError("GroundChecker: Missing collider or player controller reference.", this);
            enabled = false;
        }
    }

    private void FixedUpdate()
    {
        Bounds b = _Col.bounds;

        // Start from collider center and cast straight down
        Vector3 origin = b.center;
        float distance = b.extents.y + extraDistance;

        bool grounded = false;

        if (Physics.Raycast(origin,
                            Vector3.down,
                            out RaycastHit hit,
                            distance,
                            _GroundMask,
                            QueryTriggerInteraction.Ignore))
        {
            if (hit.normal.y >= minGroundNormalY)
                grounded = true;
        }

        if (_Controller != null && _Controller._Rb != null && _Controller._Rb.linearVelocity.y > 0.05f)
            grounded = false;

        _Controller.SetGrounded(grounded);
    }

    private void OnDrawGizmosSelected()
    {
        if (_Col == null) _Col = GetComponent<Collider>();
        Bounds b = _Col.bounds;

        Vector3 origin = b.center;
        float distance = b.extents.y + extraDistance;

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(origin, origin + Vector3.down * distance);
    }
}