using UnityEngine;
using UnityEngine.Assertions;

public class GameController : MonoBehaviour {

	private Board board;
	private Spawner spawner;
	
	private void Start () {
		board = GameObject.FindWithTag("board").GetComponent<Board>();
		spawner = GameObject.FindWithTag("spawner").GetComponent<Spawner>();
		Assert.IsNotNull(board);
		Assert.IsNotNull(spawner);
	}
}
