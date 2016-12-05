using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class IconToggle : MonoBehaviour {

	[SerializeField]
	private Sprite iconTrue;
	public Sprite IconTrue {
		get {
			return iconTrue;
		}
	}

	[SerializeField]
	private Sprite iconFalse;
	public Sprite IconFalse {
		get {
			return iconFalse;
		}
	}

	[SerializeField]
	private bool isOnByDefault = true;
	private Image image;

	private void Start () {
		image = GetComponent<Image>();
		image.sprite = isOnByDefault ? iconTrue : iconFalse;
	}

	public void ToggleIcon (bool state) {
		if (!image || !iconTrue || !iconFalse) {
			return;
		}
		image.sprite = (state) ? iconTrue : iconFalse;
	}
}
