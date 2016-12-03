using UnityEngine;

public class Spawner : MonoBehaviour {

	[SerializeField]
	private Shape[] shapePrefabs;

	private Shape GetRandomShape () {
		return shapePrefabs[Random.Range(0, shapePrefabs.Length)];
	}

	public Shape SpawnShape () {
		Shape newShape = Instantiate(GetRandomShape(), transform.position, Quaternion.identity) as Shape;
		if (newShape) {
			return newShape;
		} else {
			Debug.LogError("Error! Shape could not be spawned");
			return null;
		}
	}
}
