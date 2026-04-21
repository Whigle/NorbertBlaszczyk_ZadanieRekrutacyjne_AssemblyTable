using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AssemblyTable.Core.Serialization.UI
{
	public class SaveSystemUI : MonoBehaviour
	{
		public void SaveBtnPressec()
		{
			SystemSerializer.Instance.SaveSystem();
		}

		public void LoadBtnPressed()
		{
			SystemSerializer.Instance.LoadSystem();
		}
	}
}
