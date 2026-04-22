using AssemblyTable.Core.SystemElements;
using System.Collections.Generic;

namespace AssemblyTable.Core.SystemValidation
{
	public struct LayoutState
	{
		public IEnumerable<SystemElement> SystemElements { get; private set; }

		public LayoutState(IEnumerable<SystemElement> systemElements)
		{
			SystemElements = systemElements;
		}
	}
}