using AssemblyTable.Core.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace AssemblyTable.Core.SystemElements
{
	public class SystemElementSpawner : SingletonMB<SystemElementSpawner>, ISerializable<ElementsSaveData>
	{
		public event Action SpawnableElementsPrepared;
		public event Action SpawnedElement;
		public event Action DestroyedElement;

		[SerializeField]
		private string systemElementDataLabel = "SEData";
		[SerializeField]
		private Transform spawnPoint;

		private Dictionary<int, SystemElementSO> spawnableElements = new Dictionary<int, SystemElementSO>();
		private Dictionary<int, SystemElement> spawnedElements = new Dictionary<int, SystemElement>();

		private int idCounter = 0;

		public IReadOnlyDictionary<int, SystemElement> SpawnedElements => spawnedElements;
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

			if (instanceId != -1 && idCounter < instanceId)
			{
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
				SpawnedElement?.Invoke();
			}
		}

		private void OnDestroyed(int id)
		{
			spawnedElements.Remove(id);
			DestroyedElement?.Invoke();
		}

		private void OnDrawGizmosSelected()
		{
			if (spawnPoint != null)
			{
				Gizmos.color = Color.red;
				Gizmos.DrawWireSphere(spawnPoint.transform.position, .25f);
			}
		}

		public void ClearTable()
		{
			var ids = spawnedElements.Keys.ToArray();

			foreach (var id in ids)
			{
				spawnedElements[id].Delete();
				spawnedElements.Remove(id);
			}

			DestroyedElement?.Invoke();

			idCounter = 0;
		}

		public ElementsSaveData Serialize()
		{
			ElementsSaveData saveData = new ElementsSaveData() { Elements = new List<ElementSaveData>() };

			foreach (var element in spawnedElements.Values)
			{
				saveData.Elements.Add(new ElementSaveData()
				{
					InstanceId = element.Id,
					DataId = element.Data.Id,
					Position = element.transform.position
				});
			}

			return saveData;
		}

		public async Task Deserialize(ElementsSaveData saveData)
		{
			ClearTable();

			foreach (var element in saveData.Elements)
			{
				await SpawnSystemElement(element.DataId, element.InstanceId, element.Position);
			}
		}
	}

	[Serializable]
	public struct ElementsSaveData
	{
		public List<ElementSaveData> Elements;
	}


	[Serializable]
	public struct ElementSaveData
	{
		public int InstanceId;
		public int DataId;
		public Vector3Serializable Position;
	}
}