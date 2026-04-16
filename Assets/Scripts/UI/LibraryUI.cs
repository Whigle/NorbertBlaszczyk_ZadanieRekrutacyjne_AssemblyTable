using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class LibraryUI : MonoBehaviour
{
	[SerializeField]
	private string systemElementDataLabel = "SEData";
	[SerializeField]
	private SystemElementBtn systemElementBtnPrefab;

	[SerializeField]
	private TMP_Dropdown categoryDropdown;
	[SerializeField]
	private Transform scrollViewContent;

	private List<SystemElementBtn> buttons = new List<SystemElementBtn>();

	private void Awake()
	{
		SetCategoryDropdown();
		LoadSystemElementData();
	}

	private void LoadSystemElementData()
	{
		Addressables.LoadAssetsAsync<SystemElementSO>(systemElementDataLabel, addressable =>
		{
			if (addressable != null)
			{
				var button = Instantiate(systemElementBtnPrefab, scrollViewContent);
				button.Init(addressable);
				buttons.Add(button);
			}
		}).Completed += OnLoadDone;
	}

	private void OnLoadDone(AsyncOperationHandle<IList<SystemElementSO>> handle)
	{
		if (handle.Status != AsyncOperationStatus.Succeeded)
			Debug.LogError("Failed to load assets");
	}

	private void SetCategoryDropdown()
	{
		string[] CategoriesNames = System.Enum.GetNames(typeof(Category));

		categoryDropdown.ClearOptions();

		var options = new List<TMP_Dropdown.OptionData>();

		for (int i = 0; i < CategoriesNames.Length; i++)
		{
			options.Add(new TMP_Dropdown.OptionData(CategoriesNames[i]));
		}

		categoryDropdown.AddOptions(options);
	}

	public void OnValueChanged(int selectedOptionIndex)
	{
		var selectedOption = categoryDropdown.options[selectedOptionIndex];

		Category selectedCategory = (Category)System.Enum.Parse(typeof(Category), selectedOption.text);

		FilterElements(selectedCategory);
	}

	private void FilterElements(Category selectedCategory)
	{
		foreach (var button in buttons)
		{
			button.SetVibility(selectedCategory);
		}
	}
}
