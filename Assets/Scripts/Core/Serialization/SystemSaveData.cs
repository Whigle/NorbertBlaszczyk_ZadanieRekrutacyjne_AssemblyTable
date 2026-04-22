using AssemblyTable.Core.Ports;
using AssemblyTable.Core.SystemElements;
using System;

namespace AssemblyTable.Core.Serialization
{
	[Serializable]
	public struct SystemSaveData
	{
		public ElementsSaveData elementsSaveData;
		public ConnectionsSaveData connectionsSaveData;
	}
}
