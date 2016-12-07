using UnityEngine;
using UnityEngine.UI;

public class SettingsPanel : MonoBehaviour {

	[SerializeField]
	private Slider swipeDistanceSlider;
	[SerializeField]
	private Slider dragDistanceSlider;
	[SerializeField]
	private Toggle toggleDiagnostic;

	private GameController gameController;
	private TouchController touchController;

	private void Start () {
		gameController = GameObject.FindObjectOfType<GameController>().GetComponent<GameController>();
		touchController = GameObject.FindObjectOfType<TouchController>().GetComponent<TouchController>();

		if (dragDistanceSlider) {
			dragDistanceSlider.value = 100;
			dragDistanceSlider.minValue = 50;
			dragDistanceSlider.maxValue = 150;
		}

		if (swipeDistanceSlider) {
			swipeDistanceSlider.value = 50;
			swipeDistanceSlider.minValue = 20;
			swipeDistanceSlider.maxValue = 250;
		}

		if (toggleDiagnostic && touchController) {
			touchController.IsUsingDiagnostic = toggleDiagnostic.isOn;
		}
	}

	public void UpdatePanel () {
		if (touchController) {
			if (dragDistanceSlider) {
				touchController.MinDragDistance = dragDistanceSlider.value;
			}
			if (swipeDistanceSlider) {
				touchController.MinSwipeDistance = swipeDistanceSlider.value;
			}
			if (toggleDiagnostic) {
				touchController.IsUsingDiagnostic = toggleDiagnostic.isOn;
			}
		}
	}
}
