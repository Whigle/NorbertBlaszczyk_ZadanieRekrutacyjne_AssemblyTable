using System;
using UnityEngine;

public class ContextViewController : MonoBehaviour
{
	public event Action<SystemElement> ShowContextMenu;

	[SerializeField]
	private InputManager inputManager;

	[SerializeField]
	private LayerMask systemElementLayerMask;

	private SystemElement manipulatedObject;

	private void Awake()
	{
		inputManager.RMBPressed += OnRMBPressed;
		inputManager.RMBReleased += OnRMBReleased;
	}

	private void OnDestroy()
	{
		inputManager.RMBPressed -= OnRMBPressed;
		inputManager.RMBReleased -= OnRMBReleased;
	}

	private void OnRMBPressed()
	{
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

		if (Physics.Raycast(ray, out RaycastHit hit, 10, systemElementLayerMask))
		{
			manipulatedObject = hit.collider.gameObject.GetComponent<SystemElement>();
			ShowContextMenu?.Invoke(manipulatedObject);
		}
	}

	private void OnRMBReleased()
	{
		manipulatedObject = null;
	}
}
