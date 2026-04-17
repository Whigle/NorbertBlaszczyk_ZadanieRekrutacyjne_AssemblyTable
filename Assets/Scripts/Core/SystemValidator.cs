using System.Collections.Generic;
using System.Text;

public class SystemValidator : SingletonMB<SystemValidator>
{
	public List<ILayoutValidator> validators = new List<ILayoutValidator>();

	private StringBuilder raportGenerator = new StringBuilder();

	protected override void Awake()
	{
		base.Awake();

		//validators.Add(new ConnectionCompatibilityValidator());
		validators.Add(new OutputsValidator());
	}

	public bool ValidateSystem(out string raport)
	{
		LayoutState state = new LayoutState(SystemElementSpawner.Instance.SpawnedElements);

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

	public ValidationResult(string result, bool isValid = true)
	{
		Result = result;
		IsValid = isValid;
	}

	public void AppendLine(string text)
	{
		Result += $"\n{text}";
	}

	public override string ToString()
	{
		return Result;
	}
}

public struct LayoutState
{
	public IReadOnlyList<SystemElement> SystemElements { get; private set; }

	public LayoutState(IReadOnlyList<SystemElement> systemElements)
	{
		SystemElements = systemElements;
	}
}

public class ConnectionCompatibilityValidator : ILayoutValidator
{
	public ValidationResult Validate(LayoutState state)
	{
		throw new System.NotImplementedException();
	}
}


public class OutputsValidator : ILayoutValidator
{
	public ValidationResult Validate(LayoutState state)
	{
		var result = new ValidationResult("OutputsValidator result: Not all outputs are connected");

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