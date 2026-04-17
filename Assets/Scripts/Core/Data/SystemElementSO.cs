using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "AssemblyTable/SystemElementsSO", fileName = "SystemElementSO")]
public class SystemElementSO : ScriptableObject
{
	//TODO: Add unique ID for newly created ScriptableObject and validator	

	[SerializeField]
	private int id = 0;
	[SerializeField, FormerlySerializedAs("name")]
	private string elementName;
	[SerializeField]
	private Category category;
	[SerializeField]
	private Color color;

	[SerializeField]
	private AssetReferenceGameObject prefab;

	public int Id => id;
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
