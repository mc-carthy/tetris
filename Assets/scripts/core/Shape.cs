using UnityEngine;

public class Shape : MonoBehaviour {

	[SerializeField]
	private bool isRotatable;
	[SerializeField]
	private Vector3 queueOffset;
	public Vector3 QueueOffset {
		get {
			return queueOffset;
		}
	}

	private void Start () {

	}

	private void Move (Vector3 moveDirection) {
		transform.position += moveDirection;
	}

	public void MoveUp () {
		Move(new Vector3(0, 1, 0));
	}

	public void MoveDown () {
		Move(new Vector3(0, -1, 0));
	}

	public void MoveLeft () {
		Move(new Vector3(-1, 0, 0));
	}

	public void MoveRight () {
		Move(new Vector3(1, 0, 0));
	}

	public void RotateLeft () {
		if (isRotatable) {
			transform.Rotate(0, 0, 90);
		}
	}

	public void RotateRight () {
		if (isRotatable) {
			transform.Rotate(0, 0, -90);
		}
	}

	public void RotateClockwise (bool isRotClockwise) {
		if (isRotClockwise) {
			RotateRight();
		} else {
			RotateLeft();
		}
	} 
}
