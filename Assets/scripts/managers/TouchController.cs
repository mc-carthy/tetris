﻿using UnityEngine;
using UnityEngine.UI;

public class TouchController : MonoBehaviour {

	public delegate void TouchScreenEventHandler (Vector2 swipe);	
	public static event TouchScreenEventHandler DragEvent;
	public static event TouchScreenEventHandler SwipeEvent;
	public static event TouchScreenEventHandler TapEvent;

	[RangeAttribute(50, 150)]
	private float minDragDistance = 50;	
	public float MinDragDistance {
		get {
			return minDragDistance;
		}
		set {
			minDragDistance = value;
		}
	}

	[RangeAttribute(20, 250)]
	private float minSwipeDistance = 50;
	public float MinSwipeDistance {
		get {
			return minSwipeDistance;
		}
		set {
			minSwipeDistance = value;
		}
	}

	private bool isUsingDiagnostic;
	public bool IsUsingDiagnostic {
		get {
			return isUsingDiagnostic;
		}
		set {
			isUsingDiagnostic = value;
		}
	}

	[SerializeField]
	private Text diagnosticText1;
	[SerializeField]
	private Text diagnosticText2;
	private Vector2 touchMovement;
	private float tapTimeMax = 0;
	private float tapTimeWindow = 0.2f;

	private void Start () {
		Diagnostic("", "");
	}

	private void Update () {
		if (Input.touchCount > 0) {
			Touch touch = Input.touches[0];

			if (touch.phase == TouchPhase.Began) {
				touchMovement = Vector2.zero;
				tapTimeMax = Time.time + tapTimeWindow;
				Diagnostic("", "");
			} else if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary) {
				touchMovement += touch.deltaPosition;

				if (touchMovement.magnitude > minDragDistance) {
					OnDrag();
					Diagnostic("Drag Detected", touchMovement.ToString() + " " + SwipeDiagnostic(touchMovement));
				}
			} else if (touch.phase == TouchPhase.Ended) {
				if (touchMovement.magnitude > minSwipeDistance) {
					OnSwipeEnd();
					Diagnostic("Swipe Detected", touchMovement.ToString() + " " + SwipeDiagnostic(touchMovement));
				} else if (Time.time < tapTimeMax) {
					OnTap();
					Diagnostic("Tap Detected", touchMovement.ToString() + " " + SwipeDiagnostic(touchMovement));
				}
			}
		}
	}

	private void OnDrag () {
		if (DragEvent != null) {
			DragEvent(touchMovement);
		}
	}

	private void OnSwipeEnd () {
		if (SwipeEvent != null) {
			SwipeEvent(touchMovement);
		}	
	}

	private void OnTap () {
		if (TapEvent != null) {
			TapEvent(touchMovement);
		}
	}

	private void Diagnostic (string text1, string text2) {
		diagnosticText1.gameObject.SetActive(isUsingDiagnostic);
		diagnosticText2.gameObject.SetActive(isUsingDiagnostic);

		if (diagnosticText1 && diagnosticText2) {
			diagnosticText1.text = text1;
			diagnosticText2.text = text2;
		}
	}

	private string SwipeDiagnostic (Vector2 swipeMovement) {
		string direction = "";

		if (Mathf.Abs(swipeMovement.x) > Mathf.Abs(swipeMovement.y)) {
			direction = (swipeMovement.x >= 0) ? "right" : "left";
		} else {
			direction = (swipeMovement.y >= 0) ? "up" : "down";
		}

		return direction;
	}
}
