using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SystemElementSpawner : SingletonMB<SystemElementSpawner>
{
	[SerializeField]
	private Transform spawnPoint;

	private Dictionary<int, SystemElement> spawnedElements = new Dictionary<int, SystemElement>();

	private int idCounter = 0;

	public IReadOnlyList<SystemElement> SpawnedElements => spawnedElements.Values.ToList();

	public async void SpawnSystemElement(SystemElementSO data)
	{
		var handle = data.Prefab.InstantiateAsync(spawnPoint.position, Quaternion.identity);

		GameObject gameObject = await handle.Task;

		if (handle.Status == UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationStatus.Succeeded)
		{
			var newSystemElement = gameObject.GetComponent<SystemElement>();
			newSystemElement.Init(idCounter, data);
			newSystemElement.Destroyed += OnDestroyed;
			spawnedElements.Add(idCounter++, newSystemElement);
		}
	}

	private void OnDestroyed(int id)
	{
		spawnedElements.Remove(id);
	}

	private void OnDrawGizmosSelected()
	{
		if (spawnPoint != null)
		{
			Gizmos.color = Color.red;
			Gizmos.DrawWireSphere(spawnPoint.transform.position, .25f);
		}
	}
}
