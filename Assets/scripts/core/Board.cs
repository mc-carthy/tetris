using UnityEngine;

public class Board : MonoBehaviour {

	[SerializeField]
	private Transform emptyCellPrefab;
	[SerializeField]
	private int boardWidth, boardHeight;

	private void Start () {
		DrawEmptyCells();
	}

	private void DrawEmptyCells () {
		for (int y = 0; y < boardHeight; y++) {
			for (int x = 0; x < boardWidth; x++) {
				Transform clone;
				clone = Instantiate (emptyCellPrefab, new Vector3(x, y, 0), Quaternion.identity) as Transform;
				clone.name = "Board space (x = " + x.ToString() + ", y = " + y.ToString() + ")";
				clone.transform.parent = transform;
			}
		}
	}

}
