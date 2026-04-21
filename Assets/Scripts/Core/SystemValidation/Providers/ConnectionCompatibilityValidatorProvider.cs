using UnityEngine;

namespace AssemblyTable.Core.SystemValidation
{
	[CreateAssetMenu(fileName = "ConnectionCompatibilityValidatorProvider", menuName = "AssemblyTable/LayoutValidatorProviders/ConnectionCompatibility")]
	public class ConnectionCompatibilityValidatorProvider : LayoutValidatorProviderSO
	{
		public override ILayoutValidator Provide()
		{
			return new ConnectionCompatibilityValidator();
		}
	}


	public class ConnectionCompatibilityValidator : ILayoutValidator
	{
		public ValidationResult Validate(LayoutState state)
		{
			var result = new ValidationResult("ConnectionCompatibilityValidator result: Not all connections are valid", ValidationResult.Severity.Error);

			foreach (var systemElement in state.SystemElements)
			{
				foreach (var output in systemElement.Outputs)
				{
					if (output.IsConnected && !output.IsConnectionValid())
					{
						result.IsValid = false;

						result.AppendLine($"Id:{systemElement.Id}, SE1:{systemElement.Data.Name}, P1:{output.Data.Name}, SE2:{output.ConnectedPort.Parent.Data.Name}, P2:{output.ConnectedPort.Data.Name}");
					}
				}
			}

			return result;
		}
	}
}