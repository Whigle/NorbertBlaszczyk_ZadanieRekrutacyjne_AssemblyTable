using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveSystemUI : MonoBehaviour
{
	public void SaveBtnPressec() {
		SystemSerializer.Instance.SaveSystem();
	}

	public void LoadBtnPressed() {
		SystemSerializer.Instance.LoadSystem();
	}
}
