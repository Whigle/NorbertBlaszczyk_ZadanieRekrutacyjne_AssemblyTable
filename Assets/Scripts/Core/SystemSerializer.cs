using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

public class SystemSerializer : SingletonMB<SystemSerializer>
{
	private string savePath = "";

	protected override void Awake()
	{
		base.Awake();
		savePath = Path.Combine(Application.persistentDataPath, "SystemSaveData.json");
	}

	public void SaveSystem() {
		SystemSaveData saveData = new SystemSaveData();

		saveData.elementSaveDatas = new List<ElementSaveData>();

		foreach (var element in SystemElementSpawner.Instance.SpawnedElements)
		{
			saveData.elementSaveDatas.Add(new ElementSaveData()
			{
				InstanceId = element.Id,
				DataId = element.Data.Id,
				Position = element.transform.position
			});
		}

		//foreach (var connection in PortConnectionsController.Instance.Connections)
		//{
		//	saveData.connectionSaveData.Add(new ConnectionSaveData() {
		//		OutputElementId = connection.OutputPort.Parent.Id,
		//		OutputPortId = connection.OutputPort.Id,
		//		InputElementId = connection.InputPort.Parent.Id,
		//		InputPortId = connection.InputPort.Id,
		//	});
		//}

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
		LoadSystem(data);
	}

	public async Task LoadSystem(SystemSaveData saveData) 
	{
		SystemElementSpawner.Instance.ClearTable();

		foreach (var element in saveData.elementSaveDatas)
		{
			await SystemElementSpawner.Instance.SpawnSystemElement(element.DataId, element.InstanceId, element.Position);
		}
	}
}

[Serializable]
public struct SystemSaveData {
	public List<ElementSaveData> elementSaveDatas;
	public List<ConnectionSaveData> connectionSaveData;
}

[Serializable]
public struct ElementSaveData {
	public int InstanceId;
	public int DataId;
	public Vector3Serializable Position;
}

[Serializable]
public struct ConnectionSaveData {
	public int OutputElementId;
	public int OutputPortId;
	public int InputElementId;
	public int InputPortId;
}

[System.Serializable]
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
