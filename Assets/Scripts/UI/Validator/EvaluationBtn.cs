using AssemblyTable.Core.Evaluation;
using TMPro;
using UnityEngine;

namespace AssemblyTable.UI.Evaluation
{
	public class EvaluationBtn : MonoBehaviour
	{
		[SerializeField]
		private TMP_Text buttonText;

		private void Start()
		{
			SetText();
		}

		public void OnEvaluationBtnPressed()
		{
			EvaluationModeController.Instance.SwitchEvaluationMode();
			SetText();
		}

		public void SetText()
		{
			buttonText.text = $"Switch to:{EvaluationModeController.Instance.GetNextEvaluationMode()}";
		}
	}
}
