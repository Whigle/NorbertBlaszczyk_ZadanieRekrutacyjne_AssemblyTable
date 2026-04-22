using System;
using System.Collections.Generic;

namespace AssemblyTable.Core.Ports
{
	[Serializable]
	public struct ConnectionsSaveData
	{
		public List<ConnectionSaveData> Connections;
	}
}


