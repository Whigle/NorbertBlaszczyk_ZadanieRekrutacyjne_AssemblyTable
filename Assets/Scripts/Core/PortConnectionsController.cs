using AssemblyTable.Core.Serialization;
using AssemblyTable.Core.SystemElements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace AssemblyTable.Core.Ports
{
	public class PortConnectionsController : SingletonMB<PortConnectionsController>, IRaycastListener, ISerializable<ConnectionsSaveData>
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
		public IReadOnlyList<ConnectionData> Connections => connections.Values.ToList();

		private void Start()
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

		public void CreateConnection(Port output, Port input)
		{

			var visual = SetupConnectionVisual(output, input);

			ResetLineRenderer();

			var data = new ConnectionData(idCounter, output, input, visual);
			connections.Add(idCounter++, data);

			data.Disconnected += OnDisconnected;
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

			CreateConnection(manipulatedObject, otherPort);

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

		public ConnectionsSaveData Serialize()
		{
			ConnectionsSaveData saveData = new ConnectionsSaveData() { Connections = new List<ConnectionSaveData>() };

			foreach (var connection in PortConnectionsController.Instance.Connections)
			{
				saveData.Connections.Add(new ConnectionSaveData()
				{
					OutputElementId = connection.OutputPort.Parent.Id,
					OutputPortId = connection.OutputPort.Id,
					InputElementId = connection.InputPort.Parent.Id,
					InputPortId = connection.InputPort.Id,
				});
			}

			return saveData;
		}

		public async Task Deserialize(ConnectionsSaveData saveData)
		{
			foreach (var connection in saveData.Connections)
			{
				var outputPort = SystemElementSpawner.Instance.SpawnedElements[connection.OutputElementId].Ports[connection.OutputPortId];
				var inputPort = SystemElementSpawner.Instance.SpawnedElements[connection.InputElementId].Ports[connection.InputPortId];
				CreateConnection(outputPort, inputPort);
			}

			await Task.CompletedTask;
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

	[Serializable]
	public struct ConnectionsSaveData
	{
		public List<ConnectionSaveData> Connections;
	}


	[Serializable]
	public struct ConnectionSaveData
	{
		public int OutputElementId;
		public int OutputPortId;
		public int InputElementId;
		public int InputPortId;
	}
}


