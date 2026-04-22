using AssemblyTable.Core.Evaluation;
using UnityEngine;
using UnityEngine.UI;

namespace AssemblyTable.UI.SystemValidation
{
	[RequireComponent(typeof(Button))]
	public class ValidateBtn : MonoBehaviour
	{
		private Button button;

		private void Start()
		{
			button = GetComponent<Button>();
			EvaluationModeController.Instance.EvaluationModeChanged += OnEvaluationModeChanged;
			OnEvaluationModeChanged(EvaluationModeController.Instance.CurrentMode);
		}

		private void OnDestroy()
		{
			if (EvaluationModeController.Instance != null)
			{
				EvaluationModeController.Instance.EvaluationModeChanged -= OnEvaluationModeChanged;
			}
		}

		private void OnEvaluationModeChanged(EvaluationMode mode)
		{
			button.interactable = mode == EvaluationMode.Test;
		}

		public void OnValidateBtnPressed()
		{
			if (EvaluationModeController.Instance.CurrentMode == EvaluationMode.Test)
			{
				EvaluationModeController.Instance.Evaluate();
			}
		}
	}
}
