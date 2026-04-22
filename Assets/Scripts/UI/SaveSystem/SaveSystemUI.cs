using UnityEngine;

namespace AssemblyTable.Core.Serialization.UI
{
	public class SaveSystemUI : MonoBehaviour
	{
		private SystemSerializer systemSerializer;

		public void Initialize(SystemSerializer systemSerializer)
		{
			this.systemSerializer = systemSerializer;
		}

		public void Deinitialize()
		{
			//
		}

		public void SaveBtnPressec()
		{
			systemSerializer.SaveSystem();
		}

		public void LoadBtnPressed()
		{
			systemSerializer.LoadSystem();
		}
	}
}
