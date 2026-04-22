using AssemblyTable.Core.Evaluation;
using UnityEngine;
using UnityEngine.UI;

namespace AssemblyTable.UI.SystemValidation
{
	[RequireComponent(typeof(Button))]
	public class ValidateBtn : MonoBehaviour
	{
		private Button button;

		private EvaluationModeController evaluationModeController;

		public void Initialize(EvaluationModeController evaluationModeController)
		{
			this.evaluationModeController = evaluationModeController;

			button = GetComponent<Button>();
			evaluationModeController.EvaluationModeChanged += OnEvaluationModeChanged;
			OnEvaluationModeChanged(evaluationModeController.CurrentMode);
		}

		private void Deinitialize()
		{
			if (evaluationModeController != null)
			{
				evaluationModeController.EvaluationModeChanged -= OnEvaluationModeChanged;
			}
		}

		private void OnEvaluationModeChanged(EvaluationMode mode)
		{
			button.interactable = mode == EvaluationMode.Test;
		}

		public void OnValidateBtnPressed()
		{
			if (evaluationModeController.CurrentMode == EvaluationMode.Test)
			{
				evaluationModeController.Evaluate();
			}
		}
	}
}
