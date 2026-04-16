using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemElementContextMenu : MonoBehaviour
{
	[SerializeField]
	private ContextViewController controller;
	[SerializeField]
	private SystemElementInfoPanel systemElementInfoPanel;

	private SystemElement element;

	private void Awake()
	{
		controller.ShowContextMenu += OnShowContextMenu;
		gameObject.SetActive(false);

	}
	private void OnDestroy()
	{
		controller.ShowContextMenu -= OnShowContextMenu;
	}

	private void OnShowContextMenu(SystemElement element)
	{
		this.element = element;
		gameObject.SetActive(true);
	}

	public void OnEditBtnPressed() 
	{
		if (element)
		{
			systemElementInfoPanel.Show(element);
		}
		gameObject.SetActive(false);
	}

	public void OnDeleteBtnPressed() 
	{
		if(element) {
			Destroy(element.gameObject);
			element = null;
		}
		gameObject.SetActive(false);
	}
}
