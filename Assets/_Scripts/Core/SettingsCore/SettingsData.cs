namespace _Scripts.Core.SettingsCore
{
	public class SettingsData
	{
		private const string _dataNamePrefix = "Settings.";
		
		private readonly SerializableDataContainer _dataContainer;
		
		public SettingsData(SerializableDataContainer dataContainer)
		{
			_dataContainer = dataContainer;
		}
		
		public bool TryGet<T>(string dataName, out T dataValue)
		{
			return _dataContainer.TryGet(GetSettingsDataName(dataName), out dataValue);
		}

		public void Set<T>(string dataName, T dataValue)
		{
			_dataContainer.Set(GetSettingsDataName(dataName), dataValue);
		}

		public void Reset(string dataName)
		{
			_dataContainer.Reset(GetSettingsDataName(dataName));
		}

		private string GetSettingsDataName(string dataName)
		{
			return _dataNamePrefix + dataName;
		}
	}
}