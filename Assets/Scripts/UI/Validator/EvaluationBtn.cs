using AssemblyTable.Core.Evaluation;
using TMPro;
using UnityEngine;

namespace AssemblyTable.UI.Evaluation
{
	public class EvaluationBtn : MonoBehaviour
	{
		[SerializeField]
		private TMP_Text buttonText;

		private EvaluationModeController evaluationModeController;

		public void Initialize(EvaluationModeController evaluationModeController)
		{
			this.evaluationModeController = evaluationModeController;
			SetText();
		}

		public void Deinitialize()
		{
			//
		}

		public void OnEvaluationBtnPressed()
		{
			evaluationModeController.SwitchEvaluationMode();
			SetText();
		}

		public void SetText()
		{
			buttonText.text = $"Switch to:{evaluationModeController.GetNextEvaluationMode()}";
		}
	}
}
