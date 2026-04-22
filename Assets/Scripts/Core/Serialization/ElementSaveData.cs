using AssemblyTable.Core.Serialization;
using System;

namespace AssemblyTable.Core.SystemElements
{
	[Serializable]
	public struct ElementSaveData
	{
		public int InstanceId;
		public int DataId;
		public Vector3Serializable Position;
	}
}