using UnityEngine;

public class SoundManager : MonoBehaviour {

	[SerializeField]
	private AudioClip[] musicClips;
	[SerializeField]
	private AudioClip clearRowSound;
	[SerializeField]
	private AudioClip moveSound;
	[SerializeField]
	private AudioClip dropSound;
	[SerializeField]
	private AudioClip gameOverSound;
	[SerializeField]
	private AudioSource backgroundMusicSource;
	private AudioClip backgroundMusic;
	private AudioClip randomMusicClip;
	private bool isMusicEnabled = true;
	private bool isSfxEnabled = true;
	[Range(0, 1)]
	private float musicVolume = 1.0f;
	[Range(0, 1)]
	private float sfxVolume = 1.0f;

	private void Start () {
		backgroundMusic = GetRandomClip(musicClips);
		PlayBackgroundMusic(backgroundMusic);
	}

	public void ToggleMusic () {
		isMusicEnabled = !isMusicEnabled;
		UpdateMusic();
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
