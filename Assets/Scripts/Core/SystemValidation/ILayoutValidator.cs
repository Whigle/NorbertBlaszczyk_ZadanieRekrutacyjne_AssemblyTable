namespace AssemblyTable.Core.SystemValidation
{
	public interface ILayoutValidator
	{
		ValidationResult Validate(LayoutState state);
	}
}