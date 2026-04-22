using AssemblyTable.Core.Ports;
using System;

namespace AssemblyTable.App.Ports
{
	public class ConnectionData
	{
		public event Action<int> Disconnected;

		public int Id;
		public Port OutputPort;
		public Port InputPort;
		public PortsConnectionVisuals Visuals;

		public ConnectionData(int Id, Port outputPort, Port inputPort, PortsConnectionVisuals visuals)
		{
			this.Id = Id;
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
}


