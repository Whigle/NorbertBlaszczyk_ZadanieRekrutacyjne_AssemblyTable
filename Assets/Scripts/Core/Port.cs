using UnityEngine;

public class Port : MonoBehaviour
{
	[SerializeField]
	private PortData data;

	[SerializeField]
	private MeshRenderer meshRenderer;

	private void Awake()
	{
		SetColor();
	}

	private void SetColor()
	{
		Color color = data.Type == Type.Output ? Color.red : Color.green;

		var block = new MaterialPropertyBlock();
		meshRenderer.GetPropertyBlock(block);
		block.SetColor("_BaseColor", color);
		meshRenderer.SetPropertyBlock(block);
	}
}
