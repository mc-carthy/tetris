using UnityEngine;
using UnityEngine.Assertions;

public class Board : MonoBehaviour {

	[SerializeField]
	private Transform emptyCellPrefab;
	[SerializeField]
	private int boardWidth, boardHeight, header;

	private Transform[,] grid;

	private void Awake () {
		Assert.IsNotNull(emptyCellPrefab);
		grid = new Transform[boardWidth, boardHeight];
	}

	private void Start () {
		DrawEmptyCells();
	}

	private void DrawEmptyCells () {
		for (int y = 0; y < boardHeight - header; y++) {
			for (int x = 0; x < boardWidth; x++) {
				Transform clone;
				clone = Instantiate (emptyCellPrefab, new Vector3(x, y, 0), Quaternion.identity) as Transform;
				clone.name = "Board space (x = " + x.ToString() + ", y = " + y.ToString() + ")";
				clone.transform.parent = transform;
			}
		}
	}

}
