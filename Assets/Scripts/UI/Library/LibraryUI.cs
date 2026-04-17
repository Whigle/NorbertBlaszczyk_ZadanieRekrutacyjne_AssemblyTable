using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace AsemblyTable.Core.SystemElements.UI {
	public class LibraryUI : MonoBehaviour
	{
		[SerializeField]
		private SystemElementBtn systemElementBtnPrefab;

		[SerializeField]
		private TMP_Dropdown categoryDropdown;
		[SerializeField]
		private Transform scrollViewContent;

		private List<SystemElementBtn> buttons = new List<SystemElementBtn>();

		private void Start()
		{
			SetCategoryDropdown();

			SystemElementSpawner.Instance.SpawnableElementsPrepared += OnSpawnableElementsPrepared;
		}

		private void OnSpawnableElementsPrepared()
		{
			SystemElementSpawner.Instance.SpawnableElementsPrepared -= OnSpawnableElementsPrepared;

			foreach (var data in SystemElementSpawner.Instance.SpawnableElements)
			{
				var button = Instantiate(systemElementBtnPrefab, scrollViewContent);
				button.Init(data.Id, data.Name, data.Category);
				buttons.Add(button);
			}
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
}
