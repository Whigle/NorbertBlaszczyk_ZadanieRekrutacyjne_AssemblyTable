using System.IO;
using System.Threading.Tasks;
using UnityEngine;

namespace AssemblyTable.Core.Serialization
{
	public class SystemSerializer : MonoBehaviour
	{
		private string savePath = "";

		private ISystemElementsSaveDataProvider systemElementsSaveDataProvider;
		private IConnectionsSaveDataProvider connectionsSaveDataProvider;

		protected void Awake()
		{
			savePath = Path.Combine(Application.persistentDataPath, "SystemSaveData.json");
		}

		public void Initialize(ISystemElementsSaveDataProvider systemElementsSaveDataProvider, IConnectionsSaveDataProvider connectionsSaveDataProvider)
		{
			this.systemElementsSaveDataProvider = systemElementsSaveDataProvider;
			this.connectionsSaveDataProvider = connectionsSaveDataProvider;
		}

		public void Deinitialize()
		{
			//
		}

		public void SaveSystem()
		{
			SystemSaveData saveData = new SystemSaveData();

			saveData.elementsSaveData = systemElementsSaveDataProvider.Serialize();
			saveData.connectionsSaveData = connectionsSaveDataProvider.Serialize();

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
			await systemElementsSaveDataProvider.Deserialize(saveData.elementsSaveData);
			await connectionsSaveDataProvider.Deserialize(saveData.connectionsSaveData);
		}
	}
}
