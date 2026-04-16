using System;
using UnityEngine;

public class SEManipulator : MonoBehaviour
{
	public event Action OnMouseEntered;
	public event Action OnMouseExited;

	[SerializeField]
	private Transform snapPoint;
	[SerializeField]
	private LayerMask layerMask;

	private bool manipulate = false;
	private Vector3 snapPointOffset;

	private void Awake()
	{
		snapPointOffset = transform.position - snapPoint.position;
	}

	public void SnapToPosition(Vector3 point)
	{
		transform.position = point + snapPointOffset;
	}
}
