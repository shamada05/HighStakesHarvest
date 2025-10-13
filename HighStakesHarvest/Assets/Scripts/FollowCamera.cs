using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform cameraTransform;
    private Vector3 offset;

    void Start()
    {
        if (cameraTransform == null)
            cameraTransform = Camera.main.transform;

        // Record the offset between canvas and camera
        offset = transform.position - cameraTransform.position;
    }

    void LateUpdate()
    {
        // Keep the same offset from the camera as it moves
        transform.position = cameraTransform.position + offset;
        transform.rotation = cameraTransform.rotation;
    }
}
