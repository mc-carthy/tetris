using UnityEngine;
using UnityEngine.Assertions;

public class GameController : MonoBehaviour {

	private Board board;
	private Spawner spawner;
	private Shape activeShape;
	private float dropInterval = 1f;
	private float timeToDrop;
	
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
		if (Time.time > timeToDrop) {
			timeToDrop = Time.time + dropInterval;
			if (activeShape) {
				activeShape.MoveDown();
			}
		}
	}
}
