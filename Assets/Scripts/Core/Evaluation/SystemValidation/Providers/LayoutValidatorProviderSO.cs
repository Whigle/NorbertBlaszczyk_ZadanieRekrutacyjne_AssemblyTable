using UnityEngine;

namespace AssemblyTable.Core.SystemValidation
{
	public abstract class LayoutValidatorProviderSO : ScriptableObject
	{
		public abstract ILayoutValidator Provide();
	}
}
