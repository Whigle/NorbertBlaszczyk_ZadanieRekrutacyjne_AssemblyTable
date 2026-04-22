using System;
using TMPro;
using UnityEngine;

namespace AssemblyTable.Core.SystemElements.UI
{
	public class SystemElementBtn : MonoBehaviour
	{
		[SerializeField]
		private TMP_Text NameText;
		[SerializeField]
		private TMP_Text CategoryText;

		private int dataId;
		private string elementName;
		private Category category;
		private Action<int> onClick;

		public void Init(int dataId, string name, Category category, Action<int> onClick)
		{
			this.dataId = dataId;
			this.elementName = name;
			this.category = category;
			this.onClick = onClick;
			SetupButton();
		}

		internal void SetVibility(Category filterCategory)
		{
			gameObject.SetActive(category == filterCategory || filterCategory == Category.None ? true : false);
		}

		private void SetupButton()
		{
			NameText.text = elementName;
			CategoryText.text = category.ToString();
		}

		public void OnClick()
		{
			onClick?.Invoke(dataId);
		}
	}
}
