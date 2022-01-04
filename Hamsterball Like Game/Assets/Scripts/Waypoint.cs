using UnityEngine;

public class Waypoint : MonoBehaviour {
    private Vector3 position;
    public float moveDuration;
    public float stayDuration;

    private void Start() {
        position = transform.position;
    }

    public Vector3 GetPosition() {
        return position;
    }
}
