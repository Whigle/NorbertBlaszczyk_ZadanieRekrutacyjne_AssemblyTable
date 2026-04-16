using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SystemElement : MonoBehaviour
{
	private SystemElementSO data;

	[SerializeField]
	private MeshRenderer meshRenderer;
	[SerializeField]
	private List<Port> ports;

	public IReadOnlyList<Port> Inputs { get; private set; }
	public IReadOnlyList<Port> Outputs { get; private set; }
	public IReadOnlyList<Port> Ports => ports;

	public SystemElementSO Data => data;

	public void Init(SystemElementSO data)
	{
		Inputs = ports.Where(port => port.Data.Type == Type.Input).ToList();
		Outputs = ports.Where(port => port.Data.Type == Type.Output).ToList();

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
