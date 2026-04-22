using AssemblyTable.Core.SystemValidation;

namespace AssemblyTable.Core
{
	public interface ILayoutStateProvider
	{
		LayoutState Provide();
	}
}
