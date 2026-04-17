using System;
using UnityEngine;

public class InputManager : SingletonMB<InputManager>
{
	//TODO: remove inspector references to InputManager and use Instance only

	public event Action LMBPressed;
	public event Action RMBPressed;

	public event Action LMBReleased;
	public event Action RMBReleased;

	public Vector2 MouseScreenPosition => Input.mousePosition;

	private void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			LMBPressed?.Invoke();
		}

		if (Input.GetMouseButtonUp(0))
		{
			LMBReleased?.Invoke();
		}

		if (Input.GetMouseButtonDown(1))
		{
			RMBPressed?.Invoke();
		}

		if (Input.GetMouseButtonUp(1))
		{
			RMBReleased?.Invoke();
		}
	}
}
