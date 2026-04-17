using System;
using TMPro;
using UnityEngine;

public class SystemElementBtn : MonoBehaviour
{
	[SerializeField]
	private TMP_Text NameText;
	[SerializeField]
	private TMP_Text CategoryText;

	private int dataId;
	private string elementName;
	private Category category;

	public void Init(int dataId, string name, Category category)
	{
		this.dataId = dataId;
		this.elementName = name;
		this.category = category;
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

	public void OnClick() {
		SystemElementSpawner.Instance.SpawnSystemElement(dataId);
	}
}
