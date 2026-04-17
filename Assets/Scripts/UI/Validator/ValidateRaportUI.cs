using TMPro;
using UnityEngine;

public class ValidateRaportUI : MonoBehaviour
{
	[SerializeField]
	private GameObject raportPanel;
	[SerializeField]
	private TMP_Text raportText;

	public void OnValidateBtnPressed()
	{
		string raport = "";
		bool isValid = SystemValidator.Instance.ValidateSystem(out raport);

		if (isValid)
		{
			raportText.text = "System is valid. Good Job.";
		}
		else
		{
			raportText.text = raport;
		}

		raportPanel.SetActive(true);
	}

	public void OnCloseBtnPressed()
	{
		raportPanel.SetActive(false);
	}
}
