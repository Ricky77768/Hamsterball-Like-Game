using UnityEngine;

public class MovingPlatform : MonoBehaviour {
    public Waypoint[] waypoints;
    private Vector3 originalPosition;
    private Vector3 targetPosition;
    private float waitTime = 0f;
    private float moveProgress = 1; // 0 = start, 1 = end
    private int currentIndex = -1;

    // Start is called before the first frame update
    void Start() {
        currentIndex++;
    }

    // Update is called once per frame
    void Update() {
        if (waitTime > 0) {
            waitTime -= Time.deltaTime;
            return;
        }

        if (moveProgress == 1) {
            moveProgress = 0;
            currentIndex++;
            if (currentIndex == waypoints.Length)
            {
                currentIndex = 0;
            }
            originalPosition = transform.position;
            targetPosition = waypoints[currentIndex].GetPosition();
        } else {
            moveProgress += 1 / waypoints[currentIndex].moveDuration * Time.deltaTime;
            moveProgress = Mathf.Clamp(moveProgress, 0, 1);
            if (moveProgress == 1) {
                waitTime = waypoints[currentIndex].stayDuration;
            }
            transform.SetPositionAndRotation(originalPosition + (targetPosition - originalPosition) * moveProgress, transform.rotation);
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player")) {
            other.transform.parent = transform;
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.CompareTag("Player")) {
            other.transform.parent = null;
        }
    }

}