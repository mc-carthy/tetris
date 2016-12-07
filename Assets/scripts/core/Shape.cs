using UnityEngine;

public class Shape : MonoBehaviour {

	[SerializeField]
	private Vector3 queueOffset;
	public Vector3 QueueOffset {
		get {
			return queueOffset;
		}
	}

	[SerializeField]
	private bool isRotatable;
	[SerializeField]
	private string glowSquareFxTag;
	private GameObject[] glowSquareFx;

	private void Start () {
		if (glowSquareFxTag != null && glowSquareFxTag != "") {
			glowSquareFx = GameObject.FindGameObjectsWithTag(glowSquareFxTag);
		}
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

	public void LandShapeFx () {
		int i = 0;

		foreach (Transform child in gameObject.transform) {
			if (glowSquareFx[i]) {
				glowSquareFx[i].transform.position = child.position - Vector3.forward;
				ParticlePlayer particlePlayer = glowSquareFx[i].GetComponent<ParticlePlayer>();
				if (particlePlayer) {
					particlePlayer.Play();
				}
				i++;
			}
		}
	}
}
