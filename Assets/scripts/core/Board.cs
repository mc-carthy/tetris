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

	private bool IsWithinBoard (int x, int y) {
		return (x >= 0 && x < boardWidth && y >= 0);
	}

	private bool IsOccupied (int x, int y, Shape shape) {
		return (grid[x, y] != null && grid[x, y].parent != shape.transform);
	}

	public bool IsValidPosition (Shape shape) {
		foreach (Transform child in shape.transform) {
			Vector2 pos = Vectorf.Round(child.position);

			if (!IsWithinBoard((int)pos.x, (int)pos.y)) {
				return false;
			}
			if (IsOccupied((int)pos.x, (int)pos.y, shape)) {
				return false;
			}
		}
		return true;
	}

	

	public void StoreShapeInGrid (Shape shape) {
		if (shape == null) {
			return;
		}

		foreach (Transform child in shape.transform) {
			Vector2 pos = Vectorf.Round(child.position);
			grid[(int)pos.x, (int)pos.y] = child;
		}
	}

}
