using AssemblyTable.Core;
using AssemblyTable.Core.Evaluation;

namespace AssemblyTable.App.SystemValidation
{
	public class SystemChangedValidator
	{
		private SystemChangedEventsBroadcaster broadcaster;
		private EvaluationModeController evaluationModeController;

		public SystemChangedValidator(SystemChangedEventsBroadcaster broadcaster, EvaluationModeController evaluationModeController)
		{
			this.broadcaster = broadcaster;
			this.evaluationModeController = evaluationModeController;

			evaluationModeController.EvaluationModeChanged += OnEvaluationModeChanged;
			OnEvaluationModeChanged(evaluationModeController.CurrentMode);
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
			evaluationModeController.Evaluate();
		}
	}
}
