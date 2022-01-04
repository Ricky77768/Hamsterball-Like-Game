using UnityEngine;

public class CameraFollow : MonoBehaviour {
    public Transform target;
    public Vector3 offset;
    public float smoothSpeed = 4f;
    private bool disable = false;

    private void FixedUpdate() {
        if (disable || target.parent != null) { return; }
        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.position = smoothPosition;
    }

    private void LateUpdate() {
        if (disable || target.parent == null) { return; }
        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.position = smoothPosition;
    }

    public void disableCamera() {
        disable = true;
    }

    public void enableCamera() {
        disable = false;
    }
}
