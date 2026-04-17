using System;
using UnityEngine;

public class ContextViewController : MonoBehaviour, IRaycastListener
{
	private const string SYSTEM_ELEMENT_TAG = "SystemElement";

	public event Action<SystemElement> ShowContextMenu;

	[SerializeField]
	private LayerMask systemElementLayerMask;

	private SystemElement manipulatedObject;

	private void Awake()
	{
		Raycaster.Instance.Register(new RegisterData(SYSTEM_ELEMENT_TAG, MouseEvent.RMBPressed, this));
		Raycaster.Instance.Register(new RegisterData(SYSTEM_ELEMENT_TAG, MouseEvent.RMBReleased, this));
	}

	private void OnRMBPressed(RaycastHit hit)
	{
		manipulatedObject = hit.collider.gameObject.GetComponent<SystemElement>();
		ShowContextMenu?.Invoke(manipulatedObject);
	}

	private void OnRMBReleased()
	{
		manipulatedObject = null;
	}

	public bool ProcessRaycast(MouseEvent mouseEvent, RaycastHit hit)
	{
		if (mouseEvent == MouseEvent.RMBPressed)
		{
			OnRMBPressed(hit);
		}
		else if (mouseEvent == MouseEvent.RMBReleased)
		{
			OnRMBReleased();
		}

		return true;
	}
}
