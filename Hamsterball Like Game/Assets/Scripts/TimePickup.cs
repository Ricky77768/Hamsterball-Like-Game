using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimePickup : MonoBehaviour {

    public AudioSource pickupSFX;
    public Canvas UI;
    public int rotateSpeed = 1;
    public int timeAmount = 5;

    void Update() {
        transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime, Space.Self);
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player")) {
            pickupSFX.Play();
            UI.GetComponent<TimerController>().bonusTime(timeAmount);
            gameObject.SetActive(false);
        }
    }
}
