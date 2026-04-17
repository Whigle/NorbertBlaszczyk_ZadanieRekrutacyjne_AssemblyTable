using System;
using System.Collections.Generic;
using UnityEngine;

public class PortConnectionsController : MonoBehaviour, IRaycastListener
{
	//TODO: Better control over system state, if currently creating connections or not.

	private const string PORT_TAG = "Port";

	[SerializeField]
	private InputManager inputManager;

	[SerializeField]
	private LayerMask tableLayerMask;
	[SerializeField]
	private LayerMask portLayerMask;

	[SerializeField]
	private LineRenderer lineRenderer;

	[SerializeField]
	private PortsConnectionVisuals prefab;

	private Port manipulatedObject;

	private Dictionary<int, ConnectionData> connections = new Dictionary<int, ConnectionData>();

	private int idCounter = 0;

	private void Awake()
	{
		Raycaster.Instance.Register(new RegisterData(PORT_TAG, MouseEvent.LMBPressed, this));
		Raycaster.Instance.Register(new RegisterData("", MouseEvent.RMBPressed, this));
	}

	private void Update()
	{
		if (manipulatedObject != null)
		{
			if (Raycaster.Instance.RaycastFromMouseScreenPosition(tableLayerMask | portLayerMask, out RaycastHit hit))
			{
				lineRenderer.SetPosition(1, hit.point);
			}
		}
	}

	private void OnLMBPressed(RaycastHit hit)
	{
		if (manipulatedObject == null)
		{
			StartConnectionCreationProcess(hit);
		}
		else
		{
			TryCreatingConnection(hit);
		}
	}

	private bool OnRMBPressed(RaycastHit hit)
	{
		if (manipulatedObject != null)
		{
			manipulatedObject = null;
			ResetLineRenderer();

			return true;
		}
		else
		{
			if (hit.collider.tag == PORT_TAG)
			{
				var port = hit.collider.GetComponent<Port>();

				if (port.IsConnected)
				{
					port.Disconnect();
				}

				return true;
			}
		}

		return false;
	}

	private void StartConnectionCreationProcess(RaycastHit hit)
	{
		manipulatedObject = hit.collider.gameObject.GetComponent<Port>();

		if (manipulatedObject.Data.Type == Type.Input)
		{
			manipulatedObject = null;
			return;
		}

		lineRenderer.enabled = true;
		lineRenderer.SetPosition(0, manipulatedObject.transform.position);
	}

	private void TryCreatingConnection(RaycastHit hit)
	{
		Port otherPort = hit.collider.GetComponent<Port>();

		if (otherPort.Data.Type == Type.Output)
		{
			return;
		}

		var visual = SetupConnectionVisual(manipulatedObject, otherPort);

		ResetLineRenderer();

		var data = new ConnectionData(idCounter, manipulatedObject, otherPort, visual);
		connections.Add(idCounter++, data);

		data.Disconnected += OnDisconnected;

		manipulatedObject = null;
	}

	private void OnDisconnected(int id)
	{
		connections.Remove(id);
	}

	private void ResetLineRenderer()
	{
		lineRenderer.enabled = false;
		lineRenderer.SetPosition(0, Vector3.zero);
		lineRenderer.SetPosition(1, Vector3.zero);
	}

	private PortsConnectionVisuals SetupConnectionVisual(Port output, Port input)
	{
		output.Connect(input);
		input.Connect(output);

		var visual = Instantiate(prefab, this.transform);
		visual.Init(output, input);

		return visual;
	}

	public bool ProcessRaycast(MouseEvent mouseEvent, RaycastHit hit)
	{
		if (mouseEvent == MouseEvent.LMBPressed)
		{
			OnLMBPressed(hit);
			return true;
		}
		else if (mouseEvent == MouseEvent.RMBPressed)
		{
			return OnRMBPressed(hit);
		}

		return false;
	}
}

public class ConnectionData
{
	public event Action<int> Disconnected;

	public int Id;
	public Port OutputPort;
	public Port InputPort;
	public PortsConnectionVisuals Visuals;

	public ConnectionData(int Id, Port outputPort, Port inputPort, PortsConnectionVisuals visuals)
	{
		OutputPort = outputPort;
		InputPort = inputPort;
		Visuals = visuals;

		OutputPort.Disconnected += OnDisconnected;
		InputPort.Disconnected += OnDisconnected;
	}

	private void OnDisconnected(Port port)
	{
		OutputPort.Disconnected -= OnDisconnected;
		InputPort.Disconnected -= OnDisconnected;

		(port == OutputPort ? InputPort : OutputPort).Disconnect();

		Visuals.Disconnect();
		Disconnected?.Invoke(Id);
	}
}
