using System;

namespace AssemblyTable.Core.Ports
{
	[Serializable]
	public struct ConnectionSaveData
	{
		public int OutputElementId;
		public int OutputPortId;
		public int InputElementId;
		public int InputPortId;
	}
}


