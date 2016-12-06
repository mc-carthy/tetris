using UnityEngine;

public class ParticlePlayer : MonoBehaviour {

	[SerializeField]
	private ParticleSystem[] allParticles;

	private void Start () {
		allParticles = GetComponentsInChildren<ParticleSystem>();
	}

	public void Play () {
		foreach(ParticleSystem particles in allParticles) {
			particles.Stop();
			particles.Play();
		}
	}
}
