using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class SystemElementInfoPanel : MonoBehaviour
{
	[SerializeField]
	private TMP_Text NameText;
	[SerializeField]
	private TMP_Text CategoryText;
	[SerializeField]
	private Transform portsContent;

	[SerializeField]
	private PortInfoPanel panelInfoPrefab;

	private SystemElement element;

	public void Show(SystemElement element)
	{
		NameText.text = $"Type: {element.Data.Name}";
		CategoryText.text = $"Name: {element.Data.Category}";

		this.element = element;

		gameObject.SetActive(true);
	}

	public void Hide()
	{
		element = null;
		gameObject.SetActive(false);
	}
}
