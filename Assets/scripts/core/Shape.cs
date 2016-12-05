using UnityEngine;

public class Shape : MonoBehaviour {

	[SerializeField]
	private bool isRotatable;

	private void Start () {

	}

	private void Move (Vector3 moveDirection) {
		transform.position += moveDirection;
	}

	private void MoveLeft () {
		Move(new Vector3(-1, 0, 0));
	}

	private void MoveRight () {
		Move(new Vector3(1, 0, 0));
	}

	private void RotateLeft () {
		if (isRotatable) {
			transform.Rotate(0, 0, 90);
		}
	}

	private void RotateRight () {
		if (isRotatable) {
			transform.Rotate(0, 0, -90);
		}
	}

	public void MoveUp () {
		Move(new Vector3(0, 1, 0));
	}

	public void MoveDown () {
		Move(new Vector3(0, -1, 0));
	}
	
}
