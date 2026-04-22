using AssemblyTable.Core.Ports;
using AssemblyTable.Core.SystemElements;
using System;

namespace AssemblyTable.Core
{
	public class SystemChangedEventsBroadcaster
	{
		public event Action SystemChanged;

		public SystemChangedEventsBroadcaster()
		{
			SystemElementSpawner.Instance.SpawnedElement += CallSystemChangedEvent;
			SystemElementSpawner.Instance.DestroyedElement += CallSystemChangedEvent;
			PortConnectionsController.Instance.ConnectionCreated += CallSystemChangedEvent;
			PortConnectionsController.Instance.ConnectionRemoved += CallSystemChangedEvent;
		}

		~SystemChangedEventsBroadcaster()
		{
			SystemElementSpawner.Instance.SpawnedElement -= CallSystemChangedEvent;
			SystemElementSpawner.Instance.DestroyedElement -= CallSystemChangedEvent;
			PortConnectionsController.Instance.ConnectionCreated -= CallSystemChangedEvent;
			PortConnectionsController.Instance.ConnectionRemoved -= CallSystemChangedEvent;
		}

		private void CallSystemChangedEvent()
		{
			SystemChanged?.Invoke();
		}
	}
}
