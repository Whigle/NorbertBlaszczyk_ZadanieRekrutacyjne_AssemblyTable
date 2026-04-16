using TMPro;
using UnityEngine;

public class VisibilityBtn : MonoBehaviour
{
	[SerializeField]
	private Animator animator;
	[SerializeField]
	private TMP_Text fakeIcon;

	private readonly int showParameterId = Animator.StringToHash("Show");

	private bool show = true;

	private void Awake()
	{
		UpdateVisuals();
	}

	public void OnClick()
	{
		show = !show;

		UpdateVisuals();
	}

	private void UpdateVisuals()
	{
		animator.SetBool(showParameterId, show);
		fakeIcon.text = show ? "<<" : ">>";
	}
}
