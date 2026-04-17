using UnityEngine;

public class ManipulatorController : MonoBehaviour, IRaycastListener
{
	private const string SYSTEM_ELEMENT_TAG = "SystemElement";

	[SerializeField]
	private InputManager inputManager;

	[SerializeField]
	private LayerMask tableLayerMask;
	[SerializeField]
	private LayerMask systemElementLayerMask;

	private SEManipulator manipulatedObject;

	private void Start()
	{
		Raycaster.Instance.Register(new RegisterData(SYSTEM_ELEMENT_TAG, MouseEvent.LMBPressed, this));
		Raycaster.Instance.Register(new RegisterData(SYSTEM_ELEMENT_TAG, MouseEvent.LMBReleased, this));
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

	private void OnLMBPressed(RaycastHit hit)
	{
		manipulatedObject = hit.collider.gameObject.GetComponent<SEManipulator>();
	}

	private void OnLMBReleased()
	{
		manipulatedObject = null;
	}

	public bool ProcessRaycast(MouseEvent mouseEvent, RaycastHit hit)
	{
		if (mouseEvent == MouseEvent.LMBPressed)
		{
			OnLMBPressed(hit);
		}
		else if (mouseEvent == MouseEvent.LMBReleased)
		{
			OnLMBReleased();
		}

		return true;
	}
}
