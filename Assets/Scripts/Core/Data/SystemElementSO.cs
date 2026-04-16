using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "AssemblyTable/SystemElementsSO", fileName = "SystemElementSO")]
public class SystemElementSO : ScriptableObject
{
	
	[SerializeField, FormerlySerializedAs("name")]
	private string elementName;
	[SerializeField]
	private Category category;
	[SerializeField]
	private Color color;

	[SerializeField]
	private AssetReferenceGameObject prefab;

	public string Name => elementName;
	public Category Category => category;
	public Color Color => color;
	public AssetReferenceGameObject Prefab => prefab;
}

public enum Category
{
	None = 0,
	Source = 1,
	Converter = 2,
	Final = 3
}
