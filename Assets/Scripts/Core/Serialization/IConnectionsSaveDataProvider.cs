using AssemblyTable.Core.Ports;
using System.Threading.Tasks;

namespace AssemblyTable.Core
{
	public interface IConnectionsSaveDataProvider
	{
		Task Deserialize(ConnectionsSaveData connectionsSaveData);
		ConnectionsSaveData Serialize();
	}
}
