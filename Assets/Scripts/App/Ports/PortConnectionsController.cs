using AssemblyTable.Core;
using AssemblyTable.Core.Ports;
using AssemblyTable.Core.SystemElements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace AssemblyTable.App.Ports
{
	public class PortConnectionsController : MonoBehaviour, IRaycastListener, IConnectionsSaveDataProvider
	{
		//TODO: Better control over system state, if currently creating connections or not.

		public event Action ConnectionCreated;
		public event Action ConnectionRemoved;

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

		private ISystemElementSpawner systemElementSpawner;

		public void Initialize(ISystemElementSpawner systemElementSpawner) {
			this.systemElementSpawner = systemElementSpawner;
		}

		public void Deinitialize() {
			//
		}

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

			ConnectionCreated?.Invoke();
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

			if (manipulatedObject.Data.Type == Core.Ports.Type.Input)
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

			if (otherPort.Data.Type == Core.Ports.Type.Output)
			{
				return;
			}

			CreateConnection(manipulatedObject, otherPort);

			manipulatedObject = null;
		}

		private void OnDisconnected(int id)
		{
			connections.Remove(id);

			ConnectionRemoved?.Invoke();
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

			foreach (var connection in connections)
			{
				saveData.Connections.Add(new ConnectionSaveData()
				{
					OutputElementId = connection.Value.OutputPort.Parent.Id,
					OutputPortId = connection.Value.OutputPort.Id,
					InputElementId = connection.Value.InputPort.Parent.Id,
					InputPortId = connection.Value.InputPort.Id,
				});
			}

			return saveData;
		}

		public async Task Deserialize(ConnectionsSaveData saveData)
		{
			foreach (var connection in saveData.Connections)
			{
				var outputPort = systemElementSpawner.SpawnedElements[connection.OutputElementId].Ports[connection.OutputPortId];
				var inputPort = systemElementSpawner.SpawnedElements[connection.InputElementId].Ports[connection.InputPortId];
				CreateConnection(outputPort, inputPort);
			}

			await Task.CompletedTask;
		}
	}
}


