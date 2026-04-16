using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemElement : MonoBehaviour
{
	[SerializeField]
	private SystemElementSO data;

	[SerializeField]
	private MeshRenderer meshRenderer;
	[SerializeField]
	private List<Port> inputs;
	[SerializeField]
	private List<Port> outputs;

	private void Awake()
	{
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
