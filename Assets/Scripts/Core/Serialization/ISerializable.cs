using System.Threading.Tasks;

namespace AssemblyTable.Core.Serialization
{
	public interface ISerializable<T>
	{
		public T Serialize();
		public Task Deserialize(T data);
	}
}
