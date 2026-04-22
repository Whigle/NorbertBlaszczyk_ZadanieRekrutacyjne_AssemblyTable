using AssemblyTable.Core.Evaluation;
using UnityEngine;

namespace AssemblyTable.Core
{
	public class OnSystemChangedValidator : MonoBehaviour
	{
		//TODO: Broadcaster need to exist, can not be created by OnSystemChangedValidator
		private SystemChangedEventsBroadcaster broadcaster;

		private void Start()
		{
			broadcaster = new SystemChangedEventsBroadcaster();

			EvaluationModeController.Instance.EvaluationModeChanged += OnEvaluationModeChanged;
			OnEvaluationModeChanged(EvaluationModeController.Instance.CurrentMode);
		}

		private void OnEvaluationModeChanged(EvaluationMode mode)
		{
			if (mode == EvaluationMode.Learning)
			{
				broadcaster.SystemChanged += OnSystemChanged;
			}
			else
			{
				broadcaster.SystemChanged -= OnSystemChanged;
			}
		}

		private void OnSystemChanged()
		{
			EvaluationModeController.Instance.Evaluate();
		}
	}
}
