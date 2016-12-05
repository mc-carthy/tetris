using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

	[SerializeField]
	private GameObject gameOverPanel;
	private Board board;
	private Spawner spawner;
	private Shape activeShape;
	private SoundManager soundManager;
	private bool isGameOver;
	private float timeToDrop;
	private float dropInterval = 0.5f;
	private float timeToNextKey;
	private float keyRepeatRate = 0.1f;
	private float timeToNextKeyLeftRight;
	private float keyRepeatRateLeftRight = 0.1f;
	private float timeToNextKeyDown;
	private float keyRepeatRateDown = 0.05f;
	// private float timeToNextKeyRotate;
	// private float keyRepeatRateRotate = 0.1f;
	
	private void Start () {
		gameOverPanel.SetActive(false);
		board = GameObject.FindObjectOfType<Board>();
		spawner = GameObject.FindObjectOfType<Spawner>();
		soundManager = GameObject.FindObjectOfType<SoundManager>();
		Assert.IsNotNull(gameOverPanel);
		Assert.IsNotNull(board);
		Assert.IsNotNull(spawner);
		Assert.IsNotNull(soundManager);

		spawner.transform.position = Vectorf.Round(spawner.transform.position);
		if (activeShape == null) {
			activeShape = spawner.SpawnShape();
		}
	}

	private void Update () {
		if (!board || !spawner || !activeShape || isGameOver || !soundManager) {
			return;
		}
		GetPlayerInput();
	}

	public void Restart () {
		SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
	}

	private void GetPlayerInput () {
		if (Input.GetButton("MoveRight") && Time.time > timeToNextKeyLeftRight || Input.GetButtonDown("MoveRight")) {
			activeShape.MoveRight();
			timeToNextKeyLeftRight = Time.time + keyRepeatRateLeftRight;
			if (!board.IsValidPosition(activeShape)) {
				activeShape.MoveLeft();
			} else {
				PlaySfxThroughGameController(soundManager.MoveSound);
			}
		} else if (Input.GetButton("MoveLeft") && Time.time > timeToNextKeyLeftRight || Input.GetButtonDown("MoveLeft")) {
			PlaySfxThroughGameController(soundManager.MoveSound);
			activeShape.MoveLeft();
			timeToNextKeyLeftRight = Time.time + keyRepeatRateLeftRight;
			if (!board.IsValidPosition(activeShape)) {
				activeShape.MoveRight();
			} else {
				PlaySfxThroughGameController(soundManager.MoveSound);
			}
		} else if (Input.GetButtonDown("Rotate")) { //&& Time.time > timeToNextKeyRotate) {
			activeShape.RotateRight();
			//timeToNextKeyRotate = Time.time + keyRepeatRateRotate;
			if (!board.IsValidPosition(activeShape)) {
				activeShape.RotateLeft();
			} else {
				PlaySfxThroughGameController(soundManager.MoveSound);
			}
		}
		if (Input.GetButton("MoveDown") && Time.time > timeToNextKeyDown || Time.time > timeToDrop) {
			timeToDrop = Time.time + dropInterval;
			timeToNextKeyDown = Time.time + keyRepeatRateDown;
			activeShape.MoveDown();
			if (!board.IsValidPosition(activeShape)) {
				if (board.IsOverLimit(activeShape)) {
					GameOver();
				} else {
					LandShape();
				}
			}
		}
	}

	private void LandShape () {
		timeToNextKeyLeftRight = Time.time;
		timeToNextKeyDown = Time.time;
		//timeToNextKeyRotate = Time.time;
		activeShape.MoveUp();
		board.StoreShapeInGrid(activeShape);
		activeShape = spawner.SpawnShape();
		board.ClearFullRows();

		PlaySfxThroughGameController(soundManager.DropSound);
	}

	private void GameOver() {
		activeShape.MoveUp();
		isGameOver = true;
		gameOverPanel.SetActive(true);
		PlaySfxThroughGameController(soundManager.GameOverSound);
	}

	private void PlaySfxThroughGameController (AudioClip clip, float volMultiplier = 1.0f) {
		if (soundManager.IsSfxEnabled && clip) {
			AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position, Mathf.Clamp(soundManager.SfxVolume * volMultiplier, 0.05f, 1f));
		}
	}
}
