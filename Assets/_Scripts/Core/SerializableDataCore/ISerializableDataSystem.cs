using _Scripts.Core.SettingsCore;

namespace _Scripts.Core.GameProgressCore
{
	public interface ISerializableDataSystem
	{
		/// <summary>
		/// Загружает свои данные из dataContainer
		/// </summary>
		public void LoadFrom(SerializableDataContainer dataContainer);
		/// <summary>
		/// Выгружает свои данные в dataContainer
		/// </summary>
		public void SaveTo(SerializableDataContainer dataContainer);
	}
}