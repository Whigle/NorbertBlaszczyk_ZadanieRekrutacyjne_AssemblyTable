using AssemblyTable.Core.SystemValidation;
using System;
using UnityEngine;

namespace AssemblyTable.Core.Evaluation
{
	public class EvaluationModeController
	{
		public event Action<EvaluationMode> EvaluationModeChanged;
		public event Action<bool, string> OnValidationCompleted;

		public EvaluationMode CurrentMode { get; private set; } = EvaluationMode.Learning;

		private ILayoutStateProvider layoutStateProvider;
		private SystemValidator systemValidator;

		public EvaluationModeController(SystemValidator systemValidator, ILayoutStateProvider layoutStateProvider)
		{
			this.systemValidator = systemValidator;
			this.layoutStateProvider = layoutStateProvider;

			ChangeEvaluationMode(CurrentMode);

			systemValidator.RaportGenerated += OnRaportGenerated;
		}

		~EvaluationModeController()
		{
			if (systemValidator != null)
			{
				systemValidator.RaportGenerated -= OnRaportGenerated;
			}
		}

		private void OnRaportGenerated(bool isValid, string raport)
		{
			OnValidationCompleted?.Invoke(isValid, raport);
		}

		public void Evaluate()
		{
			if (layoutStateProvider == null)
			{
				Debug.LogError("LayoutStateProvider is null.");
				return;
			}

			var state = layoutStateProvider.Provide();

			systemValidator.ValidateSystem(state);
		}

		public void SwitchEvaluationMode()
		{
			ChangeEvaluationMode(GetNextEvaluationMode());
		}

		public EvaluationMode GetNextEvaluationMode()
		{
			int current = (int)CurrentMode;
			int next = current + 1;
			next = (next % (Enum.GetValues(typeof(EvaluationMode)).Length));

			return (EvaluationMode)next;
		}

		public void ChangeEvaluationMode(EvaluationMode newMode)
		{
			if (newMode == CurrentMode)
			{
				return;
			}

			CurrentMode = newMode;
			EvaluationModeChanged?.Invoke(CurrentMode);
		}
	}

	public enum EvaluationMode
	{
		Learning = 0,
		Test = 1
	}
}
