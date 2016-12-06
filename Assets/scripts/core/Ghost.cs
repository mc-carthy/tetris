using UnityEngine;

public class Ghost : MonoBehaviour {

	private Shape ghostShape;
	private bool hasHitBottom;
	private Color color = new Color(1f, 1f, 1f, 0.2f);

	public void DrawGhost (Shape origShape, Board board) {
		if (!ghostShape) {
			ghostShape = Instantiate(origShape, origShape.transform.position, origShape.transform.rotation) as Shape;
			ghostShape.gameObject.name = "GhostShape";
			ghostShape.transform.parent = this.transform;

			SpriteRenderer[] allRenderers = ghostShape.GetComponentsInChildren<SpriteRenderer>();
			foreach (SpriteRenderer sprRen in allRenderers) {
				sprRen.color = color;
			}
		} else {
			ghostShape.transform.position = origShape.transform.position;
			ghostShape.transform.rotation = origShape.transform.rotation;
		}
		hasHitBottom = false;

		while (!hasHitBottom) {
			ghostShape.MoveDown();
			if (!board.IsValidPosition(ghostShape)) {
				ghostShape.MoveUp();
				hasHitBottom = true;
			}
		}
	}

	public void Reset () {
		Destroy(ghostShape.gameObject);
	}

}
