using AssemblyTable.App.Ports;
using AssemblyTable.App.SystemElements;
using AssemblyTable.App.SystemValidation;
using AssemblyTable.Core;
using AssemblyTable.Core.Evaluation;
using AssemblyTable.Core.Serialization;
using AssemblyTable.Core.Serialization.UI;
using AssemblyTable.Core.SystemElements.UI;
using AssemblyTable.Core.SystemValidation;
using AssemblyTable.UI.Evaluation;
using AssemblyTable.UI.SystemValidation;
using UnityEngine;

namespace AssemblyTable.App
{
	public class AssemblyTableBootstrapper : MonoBehaviour
	{
		[SerializeField]
		private SystemElementSpawner systemElementSpawner;
		[SerializeField]
		private PortConnectionsController portConnectionsController;
		[SerializeField]
		private SystemValidator systemValidator;
		[SerializeField]
		private SystemSerializer systemSerializer;

		[SerializeField]
		private ValidateBtn validateBtn;
		[SerializeField]
		private ValidateRaportUI validateRaportUI;
		[SerializeField]
		private EvaluationBtn evaluationBtn;
		[SerializeField]
		private LibraryUI libraryUI;
		[SerializeField]
		private SaveSystemUI saveSystemUI;

		private void Awake()
		{
			portConnectionsController.Initialize(systemElementSpawner);

			var evaluationModeController = new EvaluationModeController(systemValidator, systemElementSpawner);
			var systemChangedEventsBroadcaster = new SystemChangedEventsBroadcaster(systemElementSpawner, portConnectionsController);
			var onSystemChangedValidator = new SystemChangedValidator(systemChangedEventsBroadcaster, evaluationModeController);

			systemSerializer.Initialize(systemElementSpawner, portConnectionsController);

			validateBtn.Initialize(evaluationModeController);
			validateRaportUI.Initialize(evaluationModeController);
			evaluationBtn.Initialize(evaluationModeController);
			libraryUI.Initialize(systemElementSpawner);
			saveSystemUI.Initialize(systemSerializer);
		}
	}
}
