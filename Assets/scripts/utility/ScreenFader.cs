using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(MaskableGraphic))]
public class ScreenFader : MonoBehaviour {

	private float startAlpha = 1f;
	private float endAlpha = 0f;
	private float startDelay = 0f;
	private float timeToFade = 1f;
	private float increment;
	private float currentAlpha;
	private MaskableGraphic graphic;
	private Color originalColor;
	
	private void Start () {
		graphic = GetComponent<MaskableGraphic>();
		originalColor = graphic.color;
		currentAlpha = startAlpha;
		Color tempColor = new Color(originalColor.r, originalColor.g, originalColor.b, currentAlpha);
		graphic.color = tempColor;
		increment = (endAlpha - startAlpha) / timeToFade * Time.deltaTime;

		StartCoroutine("FadeRoutine");
	}

	private IEnumerator FadeRoutine () {
		yield return new WaitForSeconds(startDelay);

		while (Mathf.Abs(endAlpha - currentAlpha) > 0.01f) {
			yield return new WaitForEndOfFrame();

			currentAlpha += increment;
			Color tempColor = new Color(originalColor.r, originalColor.g, originalColor.b, currentAlpha);
			graphic.color = tempColor;
		}

	}
}
