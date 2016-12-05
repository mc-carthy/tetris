using UnityEngine;
using UnityEngine.Assertions;

public class GameController : MonoBehaviour {

	private Board board;
	private Spawner spawner;
	private Shape activeShape;
	private float dropInterval = 0.5f;
	private float timeToDrop;
	private float keyRepeatRate = 0.1f;
	private float timeToNextKey;
	
	private void Start () {
		board = GameObject.FindWithTag("board").GetComponent<Board>();
		spawner = GameObject.FindWithTag("spawner").GetComponent<Spawner>();
		Assert.IsNotNull(board);
		Assert.IsNotNull(spawner);

		spawner.transform.position = Vectorf.Round(spawner.transform.position);
		if (activeShape == null) {
			activeShape = spawner.SpawnShape();
		}
	}

	private void Update () {
		if (!board || !spawner || !activeShape) {
			return;
		}
		GetPlayerInput();
	}

	private void GetPlayerInput () {
		if (Input.GetButton("MoveRight") && Time.time > timeToNextKey || Input.GetButtonDown("MoveRight")) {
			activeShape.MoveRight();
			timeToNextKey = Time.time + keyRepeatRate;
			if (!board.IsValidPosition(activeShape)) {
				activeShape.MoveLeft();
			}
		} else if (Input.GetButton("MoveLeft") && Time.time > timeToNextKey || Input.GetButtonDown("MoveLeft")) {
			activeShape.MoveLeft();
			timeToNextKey = Time.time + keyRepeatRate;
			if (!board.IsValidPosition(activeShape)) {
				activeShape.MoveRight();
			}
		} else if (Input.GetButtonDown("Rotate")) {
			activeShape.RotateRight();
			timeToNextKey = Time.time + keyRepeatRate;
			if (!board.IsValidPosition(activeShape)) {
				activeShape.RotateLeft();
			}
		}
		if (Input.GetButton("MoveDown") && Time.time > timeToNextKey || Time.time > timeToDrop) {
			timeToDrop = Time.time + dropInterval;
			timeToNextKey = Time.time + keyRepeatRate;
			activeShape.MoveDown();
			if (!board.IsValidPosition(activeShape)) {
				LandShape();
			}
		}
	}

	private void LandShape () {
		timeToNextKey = Time.time;
		activeShape.MoveUp();
		board.StoreShapeInGrid(activeShape);
		activeShape = spawner.SpawnShape();
	}
}
