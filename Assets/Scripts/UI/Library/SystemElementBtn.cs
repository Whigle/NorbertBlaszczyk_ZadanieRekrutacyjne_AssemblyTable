using System;
using TMPro;
using UnityEngine;

public class SystemElementBtn : MonoBehaviour
{
	[SerializeField]
	private TMP_Text NameText;
	[SerializeField]
	private TMP_Text CategoryText;

	private SystemElementSO data;

	public void Init(SystemElementSO data)
	{
		this.data = data;
		SetupButton();
	}

	internal void SetVibility(Category filterCategory)
	{
		gameObject.SetActive(data.Category == filterCategory || filterCategory == Category.None ? true : false); 
	}

	private void SetupButton()
	{
		NameText.text = data.Name;
		CategoryText.text = data.Category.ToString();
	}

	public void OnClick() {
		SystemElementSpawner.Instance.SpawnSystemElement(data);
	}
}
