using System.Collections.Generic;
using UnityEngine;

public class Raycaster : SingletonMB<Raycaster>
{
	[SerializeField]
	private InputManager inputManager;

	private Dictionary<int, RegisterData> registerDatas = new Dictionary<int, RegisterData>();

	private int idCounter = 0;

	protected override void Awake()
	{
		base.Awake();

		inputManager.LMBPressed += OnLMBPressed;
		inputManager.RMBPressed += OnRMBPressed;
		inputManager.LMBReleased += OnLMBReleased;
		inputManager.RMBReleased += OnRMBReleased;
	}

	public int Register(RegisterData data)
	{
		registerDatas.Add(idCounter++, data);

		return idCounter;
	}

	public void Unregister(int id)
	{
		registerDatas.Remove(id);
	}

	public bool RaycastFromMouseScreenPosition(LayerMask mask, out RaycastHit hit, float maxDistance = 10)
	{
		Ray ray = Camera.main.ScreenPointToRay(inputManager.MouseScreenPosition);
		return Physics.Raycast(ray, out hit, maxDistance, mask);
	}

	private void ProcessRaycast(MouseEvent mouseEvent)
	{
		Ray ray = Camera.main.ScreenPointToRay(inputManager.MouseScreenPosition);

		if (Physics.Raycast(ray, out RaycastHit hit, 10))
		{
			string tag = hit.collider.tag;

			for (int i = 0; i < registerDatas.Count; i++)
			{
				if (registerDatas[i].ShouldCall(tag, mouseEvent))
				{
					bool useRaycast = registerDatas[i].TryCall(hit);
					if (useRaycast) return;
				}
			}
		}
	}

	private void OnLMBPressed()
	{
		ProcessRaycast(MouseEvent.LMBPressed);
	}

	private void OnRMBReleased()
	{
		ProcessRaycast(MouseEvent.RMBReleased);
	}

	private void OnLMBReleased()
	{
		ProcessRaycast(MouseEvent.LMBReleased);
	}

	private void OnRMBPressed()
	{
		ProcessRaycast(MouseEvent.RMBPressed);
	}
}

public struct RegisterData
{
	public string Tag;
	public MouseEvent Event;
	public IRaycastListener Listener;

	public RegisterData(string tag, MouseEvent mouseEvent, IRaycastListener listener)
	{
		Tag = tag;
		Event = mouseEvent;
		Listener = listener;
	}

	public bool ShouldCall(string tag, MouseEvent mouseEvent)
	{
		return ((Tag == tag || Tag == "") && Event == mouseEvent);
	}

	public bool TryCall(RaycastHit hit)
	{
		return Listener.ProcessRaycast(Event, hit);
	}
}

public enum MouseEvent
{
	None,
	LMBPressed,
	LMBReleased,
	RMBPressed,
	RMBReleased
}

public interface IRaycastListener
{
	bool ProcessRaycast(MouseEvent mouseEvent, RaycastHit hit);
}
