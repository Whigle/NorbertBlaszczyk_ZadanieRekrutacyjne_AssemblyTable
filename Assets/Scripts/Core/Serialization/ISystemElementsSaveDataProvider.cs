using AssemblyTable.Core.SystemElements;
using System.Threading.Tasks;

namespace AssemblyTable.Core
{
	public interface ISystemElementsSaveDataProvider
	{
		Task Deserialize(ElementsSaveData elementsSaveData);
		ElementsSaveData Serialize();
	}
}
