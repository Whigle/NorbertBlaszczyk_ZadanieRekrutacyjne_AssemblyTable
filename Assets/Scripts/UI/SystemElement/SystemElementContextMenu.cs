using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AssemblyTable.Core.SystemElements.UI
{
	public class SystemElementContextMenu : MonoBehaviour
	{
		//TODO: Fix connection between ContextViewController and SystemElementContextMenu
		[SerializeField]
		private ContextViewController controller;
		[SerializeField]
		private SystemElementInfoPanel systemElementInfoPanel;

		private SystemElement element;
		private RectTransform rectTransform;

		private void Awake()
		{
			rectTransform = GetComponent<RectTransform>();
			controller.ShowContextMenu += OnShowContextMenu;
			gameObject.SetActive(false);

		}
		private void OnDestroy()
		{
			controller.ShowContextMenu -= OnShowContextMenu;
		}

		private void OnShowContextMenu(SystemElement element)
		{
			rectTransform.anchoredPosition = InputManager.Instance.MouseScreenPosition;
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
			if (element)
			{
				element.Delete();
				element = null;
			}
			gameObject.SetActive(false);
		}
	}
}
