using AssemblyTable.App.Ports;
using AssemblyTable.App.SystemElements;
using System;

namespace AssemblyTable.Core
{
	public class SystemChangedEventsBroadcaster
	{
		public event Action SystemChanged;

		private SystemElementSpawner spawner;
		private PortConnectionsController connectionsController;

		public SystemChangedEventsBroadcaster(SystemElementSpawner spawner, PortConnectionsController connectionsController)
		{
			this.spawner = spawner;
			this.connectionsController = connectionsController;

			spawner.SpawnedElement += CallSystemChangedEvent;
			spawner.DestroyedElement += CallSystemChangedEvent;
			connectionsController.ConnectionCreated += CallSystemChangedEvent;
			connectionsController.ConnectionRemoved += CallSystemChangedEvent;
		}

		~SystemChangedEventsBroadcaster()
		{
			if (spawner != null)
			{
				spawner.SpawnedElement -= CallSystemChangedEvent;
				spawner.DestroyedElement -= CallSystemChangedEvent;
			}
			if (connectionsController != null)
			{
				connectionsController.ConnectionCreated -= CallSystemChangedEvent;
				connectionsController.ConnectionRemoved -= CallSystemChangedEvent;
			}
		}

		private void CallSystemChangedEvent()
		{
			SystemChanged?.Invoke();
		}
	}
}
