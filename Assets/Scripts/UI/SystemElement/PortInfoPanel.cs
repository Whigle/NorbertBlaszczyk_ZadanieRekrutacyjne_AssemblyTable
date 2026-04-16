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

	//private Port port;

	public void Show(Port port)
	{
		//this.port = port;

		TypeText.text = $"Type: {port.Data.Type}";
		NameText.text = $"Name: {port.Data.Name}";

		if (port.IsConnected)
		{
			ConnectionText.text = $"Connected to: {port.ConnectedPort.Parent}";
		}
		else
		{
			ConnectionText.text = "Not connected";
		}

		gameObject.SetActive(true);
	}

	public void Hide()
	{
		//this.port = null;
		gameObject.SetActive(false);
	}
}
