using AssemblyTable.Core.SystemElements;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace AssemblyTable.Core
{
	public interface ISystemElementSpawner
	{
		IReadOnlyList<SystemElementSO> SpawnableElements { get; }
		IReadOnlyDictionary<int, SystemElement> SpawnedElements { get; }

		event Action SpawnableElementsPrepared;

		public Task SpawnSystemElement(int dataId, int instanceId = -1, Vector3? position = null);
	}
}
