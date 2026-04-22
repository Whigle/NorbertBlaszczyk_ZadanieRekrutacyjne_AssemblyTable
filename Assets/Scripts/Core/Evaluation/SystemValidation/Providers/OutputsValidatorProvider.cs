using UnityEngine;

namespace AssemblyTable.Core.SystemValidation
{
	[CreateAssetMenu(fileName = "OutputsValidatorProvider", menuName = "AssemblyTable/LayoutValidator/Providers/Outputs")]
	public class OutputsValidatorProvider : LayoutValidatorProviderSO
	{
		public override ILayoutValidator Provide()
		{
			return new OutputsValidator();
		}
	}

	public class OutputsValidator : ILayoutValidator
	{
		public ValidationResult Validate(LayoutState state)
		{
			var result = new ValidationResult("OutputsValidator result: Not all outputs are connected", ValidationResult.Severity.Warning);

			foreach (var systemElement in state.SystemElements)
			{
				foreach (var output in systemElement.Outputs)
				{
					if (!output.IsConnected)
					{
						result.IsValid = false;

						result.AppendLine($"Id:{systemElement.Id}, SE:{systemElement.Data.Name}, P:{output.Data.Name}");
					}
				}
			}

			return result;
		}
	}
}