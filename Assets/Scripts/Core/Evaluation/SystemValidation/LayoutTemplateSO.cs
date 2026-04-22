using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AssemblyTable.Core.SystemValidation
{
	[CreateAssetMenu(fileName = "LayoutTemplateSO", menuName = "AssemblyTable/LayoutValidator/LayoutTemplateSO")]
	public class LayoutTemplateSO : ScriptableObject
	{
		[SerializeField]
		private List<RequiredElementData> requiredElements;
		[SerializeField]
		private List<RequiredConnectionData> requiredConnections;

		public IReadOnlyList<RequiredElementData> RequiredElements => requiredElements;
		public IReadOnlyList<RequiredConnectionData> RequiredConnections => requiredConnections;

		public bool IsValid() 
		{
			foreach (var reqConnection in RequiredConnections)
			{
				var connectionStart = RequiredElements.FirstOrDefault(element => element.ElementNodeId == reqConnection.FromNodeId);
				var connectionEnd = RequiredElements.FirstOrDefault(element => element.ElementNodeId == reqConnection.ToNodeId);

				if (string.IsNullOrEmpty(connectionStart.ElementNodeId) || string.IsNullOrEmpty(connectionEnd.ElementNodeId))
				{
					Debug.LogError("Template is invalid.", this);
					return false;
				}
			}

			return true;
		}

	}

	[Serializable]
	public struct RequiredElementData
	{
		[Tooltip("Id which will be used in connections setup")]
		public string ElementNodeId;
		public SystemElementSO ElementType;
	}

	[Serializable]
	public struct RequiredConnectionData
	{
		public string FromNodeId;
		public string ToNodeId;
	}
}
