using UnityEngine;

public class Holder : MonoBehaviour {

	private Shape holdShape;
	public Shape HoldShape {
		get {
			return holdShape;
		}
	}

	private bool isReleaseable;
	public bool IsReleaseable {
		get {
			return isReleaseable;
		}
		set {
			isReleaseable = value;
		}
	}

	[SerializeField]
	private Transform holdTransform;
	private float holdScale = 0.5f;

	public void Catch (Shape shape) {
		if (holdShape) {
			return;
		}

		if (!shape) {
			return;
		}

		if (holdTransform) {
			shape.transform.position = holdTransform.transform.position + shape.QueueOffset;
			shape.transform.localScale = Vector3.one * holdScale;
			holdShape = shape;
		}
	}

	public Shape Release () {
		holdShape.transform.localScale = Vector3.one;
		Shape shape = holdShape;
		holdShape = null;

		isReleaseable = false;
		return shape;
	}
}
