using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour {

	private const int minLines = 1;
	private const int maxLines = 4;

	[SerializeField]
	private Text linesText;
	[SerializeField]
	private Text levelText;
	[SerializeField]
	private Text scoreText;

	private int score;
	private int lines;
	private int level = 1;
	private int linesPerLevel;

	private void Start () {
		UpdateUIText();
	}

	public void ScoreLines (int lines) {
		lines = Mathf.Clamp(lines, minLines, maxLines);

		switch (lines) {
			case 1:
				score += 40 * level;
				break;
			case 2:
				score += 100 * level;
				break;
			case 3:
				score += 300 * level;
				break;
			case 4:
				score += 1200 * level;
				break;
		}
		UpdateUIText();
	}

	public void Reset () {
		level = 1;
		lines = linesPerLevel * level;
	}

	private void UpdateUIText () {
		if (!linesText || !levelText || !scoreText) {
			return;
		}
		linesText.text = lines.ToString();
		levelText.text = level.ToString();
		scoreText.text = PadZeros(score, 6);
	}

	private string PadZeros (int score, int padDigits) {
		string str = score.ToString();
		while (str.Length < padDigits) {
			str += "0";
		}
		return str;
	}

}
