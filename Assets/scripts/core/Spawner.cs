using UnityEngine;
using UnityEngine.Assertions;

public class Spawner : MonoBehaviour {

	[SerializeField]
	private Shape[] shapePrefabs;
	[SerializeField]
	private Transform[] queueTransforms;
	[SerializeField]
	private Shape[] queuedShapes = new Shape[3];
	private float queueScale = 0.5f;

	private void Start () {
		Assert.IsNotNull(shapePrefabs);
		Assert.AreEqual(queueTransforms.Length, queuedShapes.Length);

		InitQueue();
	}

	public Shape SpawnShape () {
		Shape newShape = GetQueuedShape();
		newShape.transform.position = transform.position;
		newShape.transform.localScale = Vector3.one;
		if (newShape) {
			return newShape;
		} else {
			Debug.LogError("Error! Shape could not be spawned");
			return null;	
		}
	}

	private Shape GetRandomShape () {
		return shapePrefabs[Random.Range(0, shapePrefabs.Length)];
	}

	private void InitQueue () {
		for (int i = 0; i < queuedShapes.Length; i++) {
			queuedShapes[i] = null;
		}
		FillQueue();
	}

	private void FillQueue () {
		for (int i = 0; i < queuedShapes.Length; i++) {
			if (!queuedShapes[i]) {
				queuedShapes[i] = Instantiate(GetRandomShape(), transform.position, Quaternion.identity) as Shape;
				queuedShapes[i].transform.position = queueTransforms[i].position + queuedShapes[i].QueueOffset; 
				queuedShapes[i].transform.localScale = Vector3.one * queueScale;
			}
		}
	}

	private Shape GetQueuedShape () {
		Shape firstShape = null;

		if (queuedShapes[0]) {
			firstShape = queuedShapes[0];
		}

		for (int i = 1; i < queuedShapes.Length; i++) {
			queuedShapes[i - 1] = queuedShapes[i];
			queuedShapes[i - 1].transform.position = queueTransforms[i - 1].position + queuedShapes[i].QueueOffset;
		}

		queuedShapes[queuedShapes.Length - 1] = null;
		FillQueue();

		return firstShape;
	}
}
