using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandFollowMouse : MonoBehaviour
{
    public Camera playerCamera;   // Reference to the main camera
    public float followHeight = 1.0f; // Fixed Y position for the hand
    private Player player; // Reference to the Player script
    public Transform clampCenter;       // Center point to clamp around (assign in Inspector)
    public float maxRadius = 5f;        // Max distance allowed from center

    private void Awake()
    {
        player = GetComponentInParent<Player>();
    }

    void Update()
    {
        if (player.state == Player.State.ThrowingUp)
        {
            return; // Don't allow hand movement while throwing up
        }

        MoveHandToMouse();
    }

    void MoveHandToMouse()
    {
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        Plane plane = new Plane(Vector3.up, new Vector3(0, followHeight, 0));

        if (plane.Raycast(ray, out float enter))
        {
            Vector3 hitPoint = ray.GetPoint(enter);
            Vector3 offset = hitPoint - clampCenter.position;
            offset.y = 0; // Ignore Y axis

            if (offset.magnitude > maxRadius)
            {
                offset = offset.normalized * maxRadius;
            }

            Vector3 clampedPos = clampCenter.position + offset;
            transform.position = new Vector3(clampedPos.x, followHeight, clampedPos.z);
        }
    }
}
