using AsemblyTable.Core.SystemElements;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class SystemValidator : SingletonMB<SystemValidator>
{
	public List<ILayoutValidator> validators = new List<ILayoutValidator>();

	private StringBuilder raportGenerator = new StringBuilder();

	protected override void Awake()
	{
		base.Awake();

		validators.Add(new ConnectionCompatibilityValidator());
		validators.Add(new OutputsValidator());
	}

	public bool ValidateSystem(out string raport)
	{
		LayoutState state = new LayoutState(SystemElementSpawner.Instance.SpawnedElements.Values);

		Queue<ValidationResult> results = new Queue<ValidationResult>();

		bool isValid = true;

		foreach (ILayoutValidator validator in validators)
		{
			var result = validator.Validate(state);

			isValid = result.IsValid && isValid;

			results.Enqueue(result);
		}

		raport = GenerateRaport(results); ;

		return isValid;
	}

	private string GenerateRaport(Queue<ValidationResult> results)
	{
		raportGenerator.Clear();

		while (results.Count > 0)
		{
			var result = results.Dequeue();
			
			if(result.IsValid) 
			{
				continue;
			}

			raportGenerator.AppendLine(result.ToString());
		}

		return raportGenerator.ToString();
	}
}

public interface ILayoutValidator
{
	ValidationResult Validate(LayoutState state);
}

public struct ValidationResult
{
	public bool IsValid { get; set; }
	public string Result { get; private set; }

	public Severity ResultSeverity { get; private set; }

	public ValidationResult(string result, Severity resultSeverity = Severity.None, bool isValid = true)
	{
		Result = result;
		IsValid = isValid;
		ResultSeverity = resultSeverity;
	}

	public void AppendLine(string text)
	{
		Result += $"\n{text}";
	}

	public override string ToString()
	{
		return Result;
	}

	public enum Severity {
		None = 0,
		Warning = 1,
		Error = 2,	
	}
}

public struct LayoutState
{
	public IEnumerable<SystemElement> SystemElements { get; private set; }

	public LayoutState(IEnumerable<SystemElement> systemElements)
	{
		SystemElements = systemElements;
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