using System;
using UnityEngine;

namespace AssemblyTable.Core.Serialization
{
	[Serializable]
	public struct Vector3Serializable
	{
		public float x, y, z;

		public Vector3Serializable(float x, float y, float z)
		{
			this.x = x;
			this.y = y;
			this.z = z;
		}

		public static implicit operator Vector3(Vector3Serializable v)
			=> new Vector3(v.x, v.y, v.z);

		public static implicit operator Vector3Serializable(Vector3 v)
			=> new Vector3Serializable(v.x, v.y, v.z);

		public override string ToString()
			=> $"[{x:F1}, {y:F1}, {z:F1}]";
	}
}
