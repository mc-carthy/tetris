using UnityEngine;

public class SoundManager : MonoBehaviour {

	[SerializeField]
	private AudioClip clearRowSound;
	public AudioClip ClearRowSound {
		get {
			return clearRowSound;
		}
	}

	[SerializeField]
	private AudioClip moveSound;
	public AudioClip MoveSound {
		get {
			return moveSound;
		}
	}

	[SerializeField]
	private AudioClip dropSound;
	public AudioClip DropSound {
		get {
			return dropSound;
		}
	}

	[SerializeField]
	private AudioClip gameOverSound;
	public AudioClip GameOverSound {
		get {
			return gameOverSound;
		}
	}

	[SerializeField]
	private AudioClip errorSound;
	public AudioClip ErrorSound {
		get {
			return errorSound;
		}
	}

	[Range(0, 1)]
	private float musicVolume = 1.0f;
	public float MusicVolume {
		get {
			return musicVolume;
		}
	}

	[Range(0, 1)]
	private float sfxVolume = 1.0f;
	public float SfxVolume {
		get {
			return sfxVolume;
		}
	}

	private bool isMusicEnabled = true;
	public bool IsMusicEnabled {
		get {
			return isMusicEnabled;
		}
	}

	private bool isSfxEnabled = true;
	public bool IsSfxEnabled {
		get {
			return isSfxEnabled;
		}
	}

	[SerializeField]
	private AudioClip[] musicClips;
	[SerializeField]
	private AudioSource backgroundMusicSource;
	private AudioClip backgroundMusic;
	private AudioClip randomMusicClip;

	private void Start () {
		backgroundMusic = GetRandomClip(musicClips);
		PlayBackgroundMusic(backgroundMusic);
	}

	public void ToggleMusic () {
		isMusicEnabled = !isMusicEnabled;
		UpdateMusic();
	}

	public void ToggleSfx () {
		isSfxEnabled = !isSfxEnabled;
	}

	private AudioClip GetRandomClip (AudioClip[] clips) {
		AudioClip randomClip = clips[Random.Range(0, clips.Length)];
		return randomClip;
	}

	private void PlayBackgroundMusic (AudioClip musicClip) {
		if (!isMusicEnabled || !musicClip || !backgroundMusicSource) {
			return;
		}

		backgroundMusicSource.Stop();
		backgroundMusicSource.clip = musicClip;
		backgroundMusicSource.volume = musicVolume;
		backgroundMusicSource.loop = true;
		backgroundMusicSource.Play();
	}

	private void UpdateMusic () {
		if (backgroundMusicSource.isPlaying != isMusicEnabled) {
			if (isMusicEnabled) {
				PlayBackgroundMusic(backgroundMusic);
			} else {
				backgroundMusicSource.Stop();
			}
		}
	}
}
