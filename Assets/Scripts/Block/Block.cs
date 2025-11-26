using UnityEngine;

public class Block : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 3.0f;
    [SerializeField] private float tileTransitionSpeed = 2.5f;


    public bool IsMoving { get; set; } = false;

    // True if moving along the X axis, false if moving along the Z axis
    public bool IsMovingOnX { get; set; } = true;


    void Update()
    {
        if (!IsMoving) return;

        float moveMotion = Mathf.PingPong(Time.time * moveSpeed, tileTransitionSpeed * 2.0f) - tileTransitionSpeed;

        Vector3 currentPosition = transform.position;
        if (IsMovingOnX)
        {
            currentPosition.x = moveMotion;
            transform.position = currentPosition;
        }
        else
        {
            currentPosition.z = moveMotion;
            transform.position = currentPosition;
        }
    }
}
