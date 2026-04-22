using AssemblyTable.Core.Ports.UI;
using TMPro;
using UnityEngine;

namespace AssemblyTable.Core.SystemElements.UI
{
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

		public void Show(SystemElement element)
		{
			NameText.text = element.Data.Name;
			CategoryText.text = element.Data.Category.ToString();

			ClearPortsContentChildren();

			for (int i = 0; i < element.Ports.Count; i++)
			{
				var portInfo = Instantiate(panelInfoPrefab, portsContent);
				portInfo.Show(element.Ports[i]);
			}

			gameObject.SetActive(true);
		}

		void ClearPortsContentChildren()
		{
			while (portsContent.childCount > 0)
			{
				DestroyImmediate(portsContent.GetChild(0).gameObject);
			}
		}

		public void Hide()
		{
			gameObject.SetActive(false);
		}
	}
}
