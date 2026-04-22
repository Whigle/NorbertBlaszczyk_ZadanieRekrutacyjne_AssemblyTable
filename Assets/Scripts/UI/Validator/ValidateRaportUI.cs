using AssemblyTable.Core.Evaluation;
using TMPro;
using UnityEngine;

namespace AssemblyTable.UI.SystemValidation
{
	public class ValidateRaportUI : MonoBehaviour
	{
		[SerializeField]
		private GameObject raportPanel;
		[SerializeField]
		private TMP_Text raportText;

		private EvaluationModeController evaluationModeController;

		public void Initialize(EvaluationModeController evaluationModeController)
		{
			this.evaluationModeController = evaluationModeController;

			evaluationModeController.OnValidationCompleted += OnValidationCompleted;
		}

		public void Deinitialize()
		{
			if (evaluationModeController != null)
			{
				evaluationModeController.OnValidationCompleted -= OnValidationCompleted;
			}
		}

		public void OnValidationCompleted(bool isValid, string raport)
		{
			if (isValid)
			{
				raportText.text = "System is valid. Good Job.";
			}
			else
			{
				raportText.text = raport;
			}

			raportPanel.SetActive(true);
		}

		public void OnCloseBtnPressed()
		{
			raportPanel.SetActive(false);
		}
	}
}
