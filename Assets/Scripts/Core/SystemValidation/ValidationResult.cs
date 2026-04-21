namespace AssemblyTable.Core.SystemValidation
{
	public struct ValidationResult
	{
		public bool IsValid { get; set; }
		public string Result { get; private set; }

		public Severity ResultSeverity { get; private set; }

		public ValidationResult(string result, Severity resultSeverity = Severity.None, bool isValid = true)
		{
			Result = result;
			IsValid = isValid;
			ResultSeverity = resultSeverity;
		}

		public void AppendLine(string text)
		{
			Result += $"\n{text}";
		}

		public override string ToString()
		{
			return Result;
		}

		public enum Severity
		{
			None = 0,
			Warning = 1,
			Error = 2,
		}
	}
}