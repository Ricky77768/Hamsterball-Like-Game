using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HoldButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {
	public float requiredHoldTime;
	public UnityEvent onLongClick;
	private bool pointerDown;
	private float pointerDownTimer;

	public void OnPointerDown(PointerEventData eventData) {
		pointerDown = true;
	}

	public void OnPointerUp(PointerEventData eventData) {
		Reset();
	}

	private void Update() {
		if (pointerDown) {
			pointerDownTimer += Time.deltaTime;
			if (pointerDownTimer >= requiredHoldTime) {
				if (onLongClick != null)
					onLongClick.Invoke();

				Reset();
			}
		}
	}

	private void Reset() {
		pointerDown = false;
		pointerDownTimer = 0;
	}
}