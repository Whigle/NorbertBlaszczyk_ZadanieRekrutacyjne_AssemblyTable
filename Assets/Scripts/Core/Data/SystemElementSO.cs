using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AssemblyTable/SystemElementsSO", fileName = "SystemElementSO")]
public class SystemElementSO : ScriptableObject
{
	[SerializeField]
	private string name;
	[SerializeField]
	private Category category;
	[SerializeField]
	private Color color;

	public string Name => name;
	public Category Category => category;
	public Color Color => color;
}

public enum Category {
	Source = 0,
	Converter = 1,
	Final = 2
}
