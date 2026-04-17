using System;
using UnityEngine;

public class PortsConnectionVisuals : MonoBehaviour
{
	[SerializeField]
	LineRenderer lineRenderer;

	private Port outputPort;
	private Port inputPort;

	public void Init(Port outputPort, Port inputPort)
	{
		this.outputPort = outputPort;
		this.inputPort = inputPort;
	}

	internal void Disconnect()
	{
		Destroy(this.gameObject);
	}

	private void Update()
	{
		lineRenderer.SetPosition(0, outputPort.transform.position);
		lineRenderer.SetPosition(1, inputPort.transform.position);
	}
}
