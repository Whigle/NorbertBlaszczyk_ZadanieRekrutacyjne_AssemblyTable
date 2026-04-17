using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class SystemElementSpawner : SingletonMB<SystemElementSpawner>
{
	public event Action SpawnableElementsPrepared;

	[SerializeField]
	private string systemElementDataLabel = "SEData";
	[SerializeField]
	private Transform spawnPoint;

	private Dictionary<int, SystemElementSO> spawnableElements = new Dictionary<int, SystemElementSO>();
	private Dictionary<int, SystemElement> spawnedElements = new Dictionary<int, SystemElement>();

	private int idCounter = 0;

	public IReadOnlyList<SystemElement> SpawnedElements => spawnedElements.Values.ToList();
	public IReadOnlyList<SystemElementSO> SpawnableElements => spawnableElements.Values.ToList();

	protected override void Awake()
	{
		base.Awake();

		Addressables.LoadAssetsAsync<SystemElementSO>(systemElementDataLabel, addressable =>
		{
			if (addressable != null)
			{
				spawnableElements.Add(addressable.Id, addressable);
			}
		}).Completed += OnLoadDone;
	}

	private void OnLoadDone(AsyncOperationHandle<IList<SystemElementSO>> handle)
	{
		SpawnableElementsPrepared?.Invoke();
	}

	public async Task SpawnSystemElement(int dataId, int instanceId = -1, Vector3? position = null)
	{
		var data = spawnableElements[dataId];

		await SpawnSystemElement(data, instanceId, position);
	}

	public async Task SpawnSystemElement(SystemElementSO data, int instanceId = -1, Vector3? position = null)
	{
		int id = instanceId >= 0 ? instanceId : idCounter;

		if(instanceId != -1 && idCounter < instanceId) {
			idCounter = instanceId;
		}

		var spawnPosition = (position != null) ? position : spawnPoint.position;

		var handle = data.Prefab.InstantiateAsync(spawnPosition.Value, Quaternion.identity);

		GameObject gameObject = await handle.Task;

		if (handle.Status == AsyncOperationStatus.Succeeded)
		{
			var newSystemElement = gameObject.GetComponent<SystemElement>();
			newSystemElement.Init(id, data);
			newSystemElement.Destroyed += OnDestroyed;
			spawnedElements.Add(id, newSystemElement);
			idCounter++;
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

	internal void ClearTable()
	{
		while (spawnedElements.Count > 0)
		{
			spawnedElements[0].Delete();
			spawnedElements.Remove(0); 
		}

		idCounter = 0;
	}
}
