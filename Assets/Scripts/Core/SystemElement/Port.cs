using UnityEngine;

public class Port : MonoBehaviour
{
	[SerializeField]
	private PortData data;

	[SerializeField]
	private MeshRenderer meshRenderer;

	public PortData Data => data;

	public bool IsConnected { get; private set; }
	public Port ConnectedPort { get; private set; }
	public SystemElement Parent { get; private set; }

	private void Awake()
	{
		SetColor();
	}

	private void SetColor()
	{
		if(data.Type == Type.None) {
			Debug.LogError("Port type not set properly.", this);
			return;
		}

		Color color = data.Type == Type.Output ? Color.red : Color.green;

		var block = new MaterialPropertyBlock();
		meshRenderer.GetPropertyBlock(block);
		block.SetColor("_BaseColor", color);
		meshRenderer.SetPropertyBlock(block);
	}
}
