using TMPro;
using UnityEngine;

public class PortInfoPanel : MonoBehaviour
{
	[SerializeField]
	private TMP_Text TypeText;
	[SerializeField]
	private TMP_Text NameText;
	[SerializeField]
	private TMP_Text ConnectionText;

	public void Show(Port port)
	{
		TypeText.text = $"Type: {port.Data.Type}";
		NameText.text = $"Name: {port.Data.Name}";

		ConnectionText.text = port.GetConnectionDetails();

		gameObject.SetActive(true);
	}

	public void Hide()
	{
		gameObject.SetActive(false);
	}
}
