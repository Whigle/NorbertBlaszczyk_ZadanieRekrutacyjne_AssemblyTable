using AssemblyTable.Core.SystemElements;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AssemblyTable.Core.SystemValidation
{
	public class SystemValidator : SingletonMB<SystemValidator>
	{
		[SerializeField]
		private List<LayoutValidatorProviderSO> providers;

		private List<ILayoutValidator> validators = new List<ILayoutValidator>();
		private StringBuilder raportGenerator = new StringBuilder();

		protected override void Awake()
		{
			base.Awake();

			foreach (var provider in providers)
			{
				validators.Add(provider.Provide());
			}
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

				if (result.IsValid)
				{
					continue;
				}

				raportGenerator.AppendLine(result.ToString());
			}

			return raportGenerator.ToString();
		}
	}
}