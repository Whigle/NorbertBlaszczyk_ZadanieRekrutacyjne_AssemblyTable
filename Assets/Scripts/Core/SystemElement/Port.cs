using System;
using UnityEngine;

public class Port : MonoBehaviour
{
	public event Action<Port> Disconnected;

	[SerializeField]
	private PortData data;

	[SerializeField]
	private MeshRenderer meshRenderer;

	public PortData Data => data;

	public bool IsConnected => ConnectedPort != null;
	public Port ConnectedPort { get; private set; }
	public SystemElement Parent { get; private set; }

	private void Awake()
	{
		SetColor();
	}

	private void SetColor()
	{
		if (data.Type == Type.None)
		{
			Debug.LogError("Port type not set properly.", this);
			return;
		}

		Color color = data.Type == Type.Output ? Color.red : Color.green;

		var block = new MaterialPropertyBlock();
		meshRenderer.GetPropertyBlock(block);
		block.SetColor("_BaseColor", color);
		meshRenderer.SetPropertyBlock(block);
	}

	public void Connect(Port port) 
	{
		if (ConnectedPort != null)
		{
			ConnectedPort.Disconnect();
		}

		ConnectedPort = port;
	}

	public void Disconnect() 
	{
		ConnectedPort = null;
		Disconnected?.Invoke(this);
	}

	public string GetConnectionDetails()
	{
		if (ConnectedPort == null)
		{
			return "Port is not connected";
		}
		else
		{
			return $"Port connected to: {ConnectedPort.Data.Name}";
		}
	}
}
