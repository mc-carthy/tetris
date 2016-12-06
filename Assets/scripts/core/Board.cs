using UnityEngine;
using UnityEngine.Assertions;
using System.Collections;

public class Board : MonoBehaviour {

	private int completedRows;
	public int CompletedRows {
		get {
			return completedRows;
		}
	}

	[SerializeField]
	private Transform emptyCellPrefab;
	[SerializeField]
	private ParticlePlayer[] rowGlowFx = new ParticlePlayer[4];
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

	public IEnumerator ClearFullRows () {
		completedRows = 0;
		for (int y = 0; y < boardHeight; ++y) {
			if (IsCompleteRow(y)) {
				ClearRowFx(completedRows, y);
				completedRows++;
			}
		}

		yield return new WaitForSeconds(0.5f);

		for (int y = 0; y < boardHeight; ++y) {
			if (IsCompleteRow(y)) {
				ClearRow(y);
				ShiftRowsDown(y + 1);
				yield return new WaitForSeconds(0.2f);
				y--;
			}
		}
	}

	public bool IsOverLimit (Shape shape) {
		foreach (Transform child in shape.transform) {
			if (child.transform.position.y >= (boardHeight - header - 1)) {
				return true;
			}
		}
		return false;
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

	private bool IsCompleteRow (int y) {
		for (int x = 0; x < boardWidth; x++) {
			if (grid[x, y] == null) {
				return false;
			}
		}
		return true;
	}

	private void ClearRow (int y) {
		for (int x = 0; x < boardWidth; x++) {
			if (grid[x, y] != null) {
				Destroy(grid[x, y].gameObject);
				grid[x, y] = null;
			}
		}
	}

	private void ShiftOneRowDown (int y) {
		for (int x = 0; x < boardWidth; x++) {
			if (grid[x, y] != null) {
				grid[x, y - 1] = grid[x, y];
				grid[x, y - 1].position += new Vector3(0, -1, 0);
				grid[x, y] = null;
			}
		}
	}

	private void ShiftRowsDown (int StartY) {
		for (int y = StartY; y < boardHeight; y++) {
			ShiftOneRowDown(y);
		}
	}

	private void ClearRowFx (int id, int y) {
		if (rowGlowFx[id]) {
			rowGlowFx[id].transform.position = new Vector3(0, y, -1);
			rowGlowFx[id].Play();
		}
	}
}
