using System;
using UnityEngine;

public class InputManager : MonoBehaviour
{
	//TODO: remove inspector references to InputManager and use Instance only
	public static InputManager Instance;

	public event Action LMBPressed;
	public event Action RMBPressed;

	public event Action LMBReleased;
	public event Action RMBReleased;

	public Vector2 MouseScreenPosition => Input.mousePosition;

	private void Awake()
	{
		if (Instance != null)
		{
			Debug.LogError($"More then one {nameof(InputManager)} on scene, this one will be destroyed", this);
			Destroy(this);
		}

		Instance = this;
	}

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
