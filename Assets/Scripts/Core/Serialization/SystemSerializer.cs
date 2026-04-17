using AsemblyTable.Core.Ports;
using AsemblyTable.Core.SystemElements;
using System;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

namespace AsemblyTable.Core.Serialization
{
	public class SystemSerializer : SingletonMB<SystemSerializer>
	{
		private string savePath = "";

		protected override void Awake()
		{
			base.Awake();
			savePath = Path.Combine(Application.persistentDataPath, "SystemSaveData.json");
		}

		public void SaveSystem()
		{
			SystemSaveData saveData = new SystemSaveData();

			saveData.elementsSaveData = SystemElementSpawner.Instance.Serialize();
			saveData.connectionsSaveData = PortConnectionsController.Instance.Serialize();

			string json = JsonUtility.ToJson(saveData, true);
			File.WriteAllText(savePath, json);
		}

		public void LoadSystem()
		{
			if (!File.Exists(savePath))
			{
				Debug.Log("Save file does not exist");
				return;
			}

			string json = File.ReadAllText(savePath);
			SystemSaveData data = JsonUtility.FromJson<SystemSaveData>(json);
			_ = LoadSystem(data);
		}

		public async Task LoadSystem(SystemSaveData saveData)
		{
			await SystemElementSpawner.Instance.Deserialize(saveData.elementsSaveData);
			await PortConnectionsController.Instance.Deserialize(saveData.connectionsSaveData);
		}
	}

	[Serializable]
	public struct SystemSaveData
	{
		public ElementsSaveData elementsSaveData;
		public ConnectionsSaveData connectionsSaveData;
	}

	[Serializable]
	public struct Vector3Serializable
	{
		public float x, y, z;

		public Vector3Serializable(float x, float y, float z)
		{
			this.x = x;
			this.y = y;
			this.z = z;
		}

		public static implicit operator Vector3(Vector3Serializable v)
			=> new Vector3(v.x, v.y, v.z);

		public static implicit operator Vector3Serializable(Vector3 v)
			=> new Vector3Serializable(v.x, v.y, v.z);

		public override string ToString()
			=> $"[{x:F1}, {y:F1}, {z:F1}]";
	}
}
