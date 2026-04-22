using AssemblyTable.Core.Evaluation;
using System;
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

		private void Start()
		{
			EvaluationModeController.Instance.OnValidationCompleted += OnValidationCompleted;
		}

		private void OnDestroy()
		{
			if (EvaluationModeController.Instance != null)
			{
				EvaluationModeController.Instance.OnValidationCompleted -= OnValidationCompleted;
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
