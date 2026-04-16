using System.Collections.Generic;
using UnityEngine;

public class SystemElement : MonoBehaviour
{
	private SystemElementSO data;

	[SerializeField]
	private MeshRenderer meshRenderer;
	[SerializeField]
	private List<Port> inputs;
	[SerializeField]
	private List<Port> outputs;

	public void Init(SystemElementSO data)
	{
		this.data = data;

		if (data.Category == Category.None)
		{
			Debug.LogError("Category not set properly.", this);
			return;
		}

		SetColor();
	}

	private void SetColor()
	{
		Color color = data.Color;

		var block = new MaterialPropertyBlock();
		meshRenderer.GetPropertyBlock(block);
		block.SetColor("_BaseColor", color);
		meshRenderer.SetPropertyBlock(block);
	}
}
