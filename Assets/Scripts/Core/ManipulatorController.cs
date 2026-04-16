using UnityEngine;

public class ManipulatorController : MonoBehaviour
{
	[SerializeField]
	private InputManager inputManager;

	[SerializeField]
	private LayerMask tableLayerMask;
	[SerializeField]
	private LayerMask systemElementLayerMask;

	private SEManipulator manipulatedObject;

	private void Awake()
	{
		inputManager.LMBPressed += OnLMBPressed;
		inputManager.LMBReleased += OnLMBReleased;
	}

	private void Update()
	{
		if (manipulatedObject)
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

			if (Physics.Raycast(ray, out RaycastHit hit, 10, tableLayerMask))
			{
				manipulatedObject.SnapToPosition(hit.point);
			}
		}
	}

	private void OnDestroy()
	{
		inputManager.LMBPressed -= OnLMBPressed;
		inputManager.LMBReleased -= OnLMBReleased;
	}

	private void OnLMBPressed()
	{
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

		if (Physics.Raycast(ray, out RaycastHit hit, 10, systemElementLayerMask))
		{
			manipulatedObject = hit.collider.gameObject.GetComponent<SEManipulator>();
		}
	}

	private void OnLMBReleased()
	{
		manipulatedObject = null;
	}
}
