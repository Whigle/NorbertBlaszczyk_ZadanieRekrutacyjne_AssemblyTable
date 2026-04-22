using AssemblyTable.Core.SystemValidation;
using System;

namespace AssemblyTable.Core.Evaluation
{
	public class EvaluationModeController : SingletonMB<EvaluationModeController>
	{
		public event Action<EvaluationMode> EvaluationModeChanged;
		public event Action<bool, string> OnValidationCompleted;

		public EvaluationMode CurrentMode { get; private set; } = EvaluationMode.Learning;

		protected override void Awake()
		{
			base.Awake();
		}

		private void Start()
		{
			ChangeEvaluationMode(CurrentMode);

			SystemValidator.Instance.RaportGenerated += OnRaportGenerated;
		}

		protected override void OnDestroy()
		{
			if (SystemValidator.Instance)
			{
				SystemValidator.Instance.RaportGenerated -= OnRaportGenerated;
			}
		}

		private void OnRaportGenerated(bool isValid, string raport)
		{
			OnValidationCompleted?.Invoke(isValid, raport);
		}

		public void Evaluate()
		{
			SystemValidator.Instance.ValidateSystem();
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
