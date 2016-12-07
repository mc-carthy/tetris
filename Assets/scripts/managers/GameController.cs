using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameController : MonoBehaviour {

	private enum Direction {
		none,
		left,
		right,
		up,
		down
	}

	[SerializeField]
	private GameObject gameOverPanel;
	[SerializeField]
	private IconToggle rotIconToggle;
	[SerializeField]
	private GameObject pausePanel;
	[SerializeField]
	private ParticlePlayer gameOverFx;

	private Board board;
	private Spawner spawner;
	private Shape activeShape;
	private SoundManager soundManager;
	private ScoreManager scoreManager;
	private Ghost ghost;
	private Holder holder;
	private Direction dragDirection = Direction.none;
	private Direction swipeDirection = Direction.none;

	[Range(0.05f, 1f)]
	private float minTimeToDrag = 0.15f;
	[Range(0.05f, 1f)]
	private float minTimeToSwipe = 0.3f;

	private bool isGameOver;
	private float timeToDrop;
	private float dropInterval = 0.5f;
	private float dropIntervalMod;
	private float timeToNextKeyLeftRight;
	private float keyRepeatRateLeftRight = 0.1f;
	private float timeToNextKeyDown;
	private float keyRepeatRateDown = 0.05f;
	private float timeToNextDrag;
	private float timeToNextSwipe;
	private bool didTap;
	private bool isRotClockwise = true;
	private bool isGamePaused;
	// private float timeToNextKeyRotate;
	// private float keyRepeatRateRotate = 0.1f;

	private void OnEnable () {
		TouchController.DragEvent += DragHandler;
		TouchController.SwipeEvent += SwipeHandler;
		TouchController.TapEvent += TapHandler;
	}

	private void OnDisable () {
		TouchController.DragEvent -= DragHandler;
		TouchController.SwipeEvent -= SwipeHandler;
		TouchController.TapEvent -= TapHandler;
	}
	
	private void Start () {
		gameOverPanel.SetActive(false);
		pausePanel.SetActive(false);

		GetReferences();
		MakeAssertions();

		spawner.transform.position = Vectorf.Round(spawner.transform.position);
		if (activeShape == null) {
			activeShape = spawner.SpawnShape();
		}

		dropIntervalMod = dropInterval;
	}

	private void Update () {
		if (!board || !spawner || !activeShape || isGameOver || !soundManager || !scoreManager) {
			return;
		}
		GetPlayerInput();
	}

	private void LateUpdate () {
		if (ghost) {
			ghost.DrawGhost(activeShape, board);
		}
	}

	public void Restart () {
		Time.timeScale = 1;
		SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
	}

	public void ToggleRotDirection () {
		isRotClockwise = !isRotClockwise;
		if (rotIconToggle) {
			rotIconToggle.ToggleIcon(isRotClockwise);
		}
	}

	public void TogglePause () {
		if (isGameOver) {
			return;
		}

		isGamePaused = !isGamePaused;

		if (pausePanel) {
			pausePanel.SetActive(isGamePaused);
			if (soundManager) {
				soundManager.BackgroundMusicSource.volume = isGamePaused ? soundManager.MusicVolume * 0.25f : soundManager.MusicVolume;
			}
			Time.timeScale = isGamePaused ? 0f : 1f;
		}
	}

	public void Hold () {
		if (!holder.HoldShape) {
			holder.Catch(activeShape);
			activeShape = spawner.SpawnShape();
			PlaySfxThroughGameController(soundManager.HoldSound);
		} else if (holder.IsReleaseable) {
			Shape shape = activeShape;
			activeShape = holder.Release();
			activeShape.transform.position = spawner.transform.position;
			holder.Catch(shape);
			PlaySfxThroughGameController(soundManager.HoldSound);
		} else {
			Debug.LogWarning("Holder Warning: Wait for cooldown");
			PlaySfxThroughGameController(soundManager.ErrorSound);
		}

		if (ghost) {
			ghost.Reset();
		}
	}

	private void MoveRight () {
		activeShape.MoveRight();
		timeToNextKeyLeftRight = Time.time + keyRepeatRateLeftRight;
		if (!board.IsValidPosition(activeShape)) {
			PlaySfxThroughGameController(soundManager.ErrorSound);
			activeShape.MoveLeft();
		} else {
			PlaySfxThroughGameController(soundManager.MoveSound, 0.5f);
		}
	}

	private void MoveLeft () {
		activeShape.MoveLeft();
		timeToNextKeyLeftRight = Time.time + keyRepeatRateLeftRight;
		if (!board.IsValidPosition(activeShape)) {
			PlaySfxThroughGameController(soundManager.ErrorSound);
			activeShape.MoveRight();
		} else {
			PlaySfxThroughGameController(soundManager.MoveSound, 0.5f);
		}
	}

	private void Rotate () {
		activeShape.RotateClockwise(isRotClockwise);
		//timeToNextKeyRotate = Time.time + keyRepeatRateRotate;
		if (!board.IsValidPosition(activeShape)) {
			PlaySfxThroughGameController(soundManager.ErrorSound);
			activeShape.RotateClockwise(!isRotClockwise);
		} else {
			PlaySfxThroughGameController(soundManager.MoveSound, 0.5f);
		}
	}

	private void MoveDown () {
		timeToDrop = Time.time + dropIntervalMod;
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

	private void GetPlayerInput () {
		if ((Input.GetButton("MoveRight") && Time.time > timeToNextKeyLeftRight) || Input.GetButtonDown("MoveRight")) {
			MoveRight();
		} else if ((Input.GetButton("MoveLeft") && Time.time > timeToNextKeyLeftRight) || Input.GetButtonDown("MoveLeft")) {
			MoveLeft();
		} else if (Input.GetButtonDown("Rotate")) { //&& Time.time > timeToNextKeyRotate) {
			Rotate();
		} else if ((Input.GetButton("MoveDown") && Time.time > timeToNextKeyDown) || Time.time > timeToDrop) {
			MoveDown();
		} else if (
			dragDirection == Direction.right && Time.time > timeToNextDrag || 
			swipeDirection == Direction.right && Time.time > timeToNextSwipe) {
			MoveRight();
			timeToNextDrag = Time.time + timeToNextDrag;
			timeToNextSwipe = Time.time + timeToNextSwipe;
		} else if (
			dragDirection == Direction.left && Time.time > timeToNextDrag || 
			swipeDirection == Direction.left && Time.time > timeToNextSwipe) {
			MoveLeft();
			timeToNextDrag = Time.time + timeToNextDrag;
			timeToNextSwipe = Time.time + timeToNextSwipe;
		} else if ((swipeDirection == Direction.up && Time.time > timeToNextSwipe) || (didTap)) {
			Rotate();
			didTap = false;
			timeToNextSwipe = Time.time + timeToNextSwipe;
		} else if (dragDirection == Direction.down && Time.time > timeToNextDrag) {
			MoveDown();
		} else if (Input.GetButtonDown("Hold")) {
			Hold();
		} else if (Input.GetButtonDown("ToggleRot")) {
			ToggleRotDirection();
		} else if (Input.GetButtonDown("TogglePause")) {
			TogglePause();
		}
		dragDirection = Direction.none;
		swipeDirection = Direction.none;
		didTap = false;
	}

	private void LandShape () {
		timeToNextKeyLeftRight = Time.time;
		timeToNextKeyDown = Time.time;
		//timeToNextKeyRotate = Time.time;
		activeShape.MoveUp();
		board.StoreShapeInGrid(activeShape);

		activeShape.LandShapeFx();

		if (ghost) {
			ghost.Reset();
		}

		if (holder) {
			holder.IsReleaseable = true;
		}

		activeShape = spawner.SpawnShape();
		board.StartCoroutine("ClearFullRows");

		PlaySfxThroughGameController(soundManager.DropSound, 0.25f);

		if (board.CompletedRows > 0) {
			scoreManager.ScoreLines(board.CompletedRows);
			if (scoreManager.IsLevelingUp) {
				PlaySfxThroughGameController(soundManager.LevelUpVocal);
				dropIntervalMod = Mathf.Clamp(dropInterval - (((float)scoreManager.Level - 1) * 0.05f), 0.05f, 1f);
			} else {
				if (board.CompletedRows > 1) {
					PlaySfxThroughGameController(soundManager.GetRandomClip(soundManager.VocalClips));
				}
			}
			PlaySfxThroughGameController(soundManager.ClearRowSound);
		}
	}

	private void GameOver() {
		activeShape.MoveUp();
		isGameOver = true;

		StartCoroutine(GameOverRoutine());

		PlaySfxThroughGameController(soundManager.GameOverSound, 5.0f);
		PlaySfxThroughGameController(soundManager.GameOverVocal, 5.0f);
	}

	private IEnumerator GameOverRoutine () {
		if (gameOverFx) {
			gameOverFx.Play();
		}

		yield return new WaitForSeconds(0.25f);

		gameOverPanel.SetActive(true);
	}

	private void PlaySfxThroughGameController (AudioClip clip, float volMultiplier = 1.0f) {
		if (soundManager.IsSfxEnabled && clip) {
			AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position, Mathf.Clamp(soundManager.SfxVolume * volMultiplier, 0.05f, 1f));
		}
	}

	private void GetReferences() {
		board = GameObject.FindObjectOfType<Board>();
		spawner = GameObject.FindObjectOfType<Spawner>();
		soundManager = GameObject.FindObjectOfType<SoundManager>();
		scoreManager = GameObject.FindObjectOfType<ScoreManager>();
		ghost = GameObject.FindObjectOfType<Ghost>();
		holder = GameObject.FindObjectOfType<Holder>();
	}

	private void MakeAssertions () {
		Assert.IsNotNull(gameOverPanel);
		Assert.IsNotNull(board);
		Assert.IsNotNull(spawner);
		Assert.IsNotNull(soundManager);
		Assert.IsNotNull(scoreManager);
		Assert.IsNotNull(holder);
	}

	private void DragHandler (Vector2 swipeMovement) {
		dragDirection = GetDirection(swipeMovement);
	}

	private void SwipeHandler (Vector2 swipeMovement) {
		swipeDirection = GetDirection(swipeMovement);
	}

	private void TapHandler (Vector2 swipeMovement) {
		didTap = true;
	}

	private Direction GetDirection (Vector2 swipeMovement) {
		Direction swipeDir = Direction.none;

		if (Mathf.Abs(swipeMovement.x) > Mathf.Abs(swipeMovement.y)) {
			swipeDir = (swipeMovement.x >= 0) ? Direction.right : Direction.left;
		} else {
			swipeDir = (swipeMovement.y >= 0) ? Direction.up : Direction.down;
		}

		return swipeDir;
	}
}
