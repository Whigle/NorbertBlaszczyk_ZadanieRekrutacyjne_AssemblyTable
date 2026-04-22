using AssemblyTable.Core.Ports;
using AssemblyTable.Core.SystemElements;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AssemblyTable.Core.SystemValidation
{
	[CreateAssetMenu(fileName = "TemplateComplianceValidatorProvider", menuName = "AssemblyTable/LayoutValidator/Providers/TemplateCompliance")]
	public class TemplateComplianceValidatorProvider : LayoutValidatorProviderSO
	{
		[SerializeField]
		private LayoutTemplateSO templateSO;

		public override ILayoutValidator Provide()
		{
			return new TemplateComplianceValidator(templateSO);
		}
	}

	public class TemplateComplianceValidator : ILayoutValidator
	{
		private LayoutTemplateSO templateSO;

		public TemplateComplianceValidator(LayoutTemplateSO templateSO)
		{
			this.templateSO = templateSO;
		}

		public ValidationResult Validate(LayoutState state)
		{
			var result = new ValidationResult("TemplateComplianceValidator result: Differences from template", ValidationResult.Severity.Error);

			if (templateSO == null || !templateSO.IsValid())
			{
				Debug.LogError("Template is invalid. Aborting.");
				return result;
			}

			List<SystemElement> existingElements = new List<SystemElement>();
			List<Port> existingOutputPorts = new List<Port>();

			foreach (var element in state.SystemElements)
			{
				existingElements.Add(element);
				foreach (var output in element.Outputs)
				{
					existingOutputPorts.Add(output);
				}
			}

			result = CheckElementsRequirements(result, existingElements);
			result = CheckConnectionsRequirements(result, existingOutputPorts);

			return result;
		}

		private ValidationResult CheckConnectionsRequirements(ValidationResult result, List<Port> existingOutputPorts)
		{
			foreach (var reqConnection in templateSO.RequiredConnections)
			{
				var connectionStart = templateSO.RequiredElements.FirstOrDefault(element => element.ElementNodeId == reqConnection.FromNodeId);
				var connectionEnd = templateSO.RequiredElements.FirstOrDefault(element => element.ElementNodeId == reqConnection.ToNodeId);

				var outputElementType = connectionStart.ElementType;
				var inputElementType = connectionEnd.ElementType;

				var validStarts = existingOutputPorts.FindAll(port => port.Parent.Data == outputElementType && port.IsConnected);

				if (validStarts.Count == 0)
				{
					result.IsValid = false;
					result.AppendLine($"Missing connection between {reqConnection.FromNodeId} and {reqConnection.ToNodeId}");
					continue;
				}

				Port selectedPort = null;

				foreach (var port in validStarts)
				{
					if (port.ConnectedPort.Parent.Data == inputElementType)
					{
						selectedPort = port;
						break;
					}
				}

				if (selectedPort != null)
				{
					existingOutputPorts.Remove(selectedPort);
				}
				else
				{
					result.IsValid = false;
					result.AppendLine($"Missing connection between {reqConnection.FromNodeId} and {reqConnection.ToNodeId}");
				}
			}

			return result;
		}

		private ValidationResult CheckElementsRequirements(ValidationResult result, List<SystemElement> existingElements)
		{
			for (int i = 0; i < templateSO.RequiredElements.Count; i++)
			{
				var element = existingElements.FirstOrDefault(element => element.Data == templateSO.RequiredElements[i].ElementType);

				if (element == null)
				{
					result.IsValid = false;
					result.AppendLine($"Need more elements of type:{templateSO.RequiredElements[i].ElementType.Name}");
				}
				else
				{
					existingElements.Remove(element);
				}
			}

			if (existingElements.Count > 0)
			{
				result.IsValid = false;
				foreach (var element in existingElements)
				{
					result.AppendLine($"Not needed element of type:{element.Data.Name}");
				}
			}

			return result;
		}
	}
}
