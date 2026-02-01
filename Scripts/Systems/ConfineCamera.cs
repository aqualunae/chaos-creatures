using System;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class ConfineCamera : MonoBehaviour
{
    [SerializeField, Tooltip("Collider that defines the area the camera should stay within.")]
    private Collider2D boundaries;

    private Camera boundCamera;

    private float minX;
    private float maxX;
    private float minY;
    private float maxY;

    private void Start()
    {
        // size of the camera in world space
        boundCamera = GetComponent<Camera>();
        float height = boundCamera.orthographicSize;
        float width = height * boundCamera.aspect;

        Bounds bounds = boundaries.bounds;

        // the minimum and maximum values that the camera's position (center) can be
        // and still have the edges contained within the boundaries
        minX = bounds.center.x - bounds.extents.x + width;
        maxX = bounds.center.x + bounds.extents.x - width;
        minY = bounds.center.y - bounds.extents.y + height;
        maxY = bounds.center.y + bounds.extents.y - height;
    }

    private void LateUpdate()
    {
        // late update so it happens after CenterCamera centers the camera
        // clamp the position to keep it within the boundaries
        Vector3 position = transform.position;
        position.x = Mathf.Clamp(position.x, minX, maxX);
        position.y = Mathf.Clamp(position.y, minY, maxY);
        transform.position = position;
    }

    // figured out with help from the forums
    // https://discussions.unity.com/t/calculating-2d-camera-bounds/77081
    // https://discussions.unity.com/t/how-to-get-the-cameras-size-in-world-units/245791
}
