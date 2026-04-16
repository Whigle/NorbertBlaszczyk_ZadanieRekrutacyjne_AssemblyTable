using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct PortData
{
	[SerializeField]
	private Type type;
	[SerializeField]
	private string name;
	[SerializeField]
	private List<string> compatibleCategories;
	[SerializeField]
	private List<string> compatibleElements;

	public Type Type => type;
	public string Name => name;
	public IReadOnlyList<string> CompatibleCategories => compatibleCategories;
	public IReadOnlyList<string> CompatibleElements => compatibleElements;

	public PortData(Type type = 0, string name = "NotSet", List<string> compatibleCategories = null, List<string> compatibleElements = null)
	{
		this.type = type;
		this.name = name;
		this.compatibleCategories = compatibleCategories != null ? compatibleCategories : new List<string>();
		this.compatibleElements = compatibleElements != null ? compatibleElements : new List<string>();
	}
}

public enum Type
{
	Input = 0,
	Output = 1
}
