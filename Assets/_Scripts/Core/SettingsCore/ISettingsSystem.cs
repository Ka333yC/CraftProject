namespace _Scripts.Core.SettingsCore
{
	public interface ISettingsSystem
	{
		/// <summary>
		/// Загружает свои данные из settingsData
		/// </summary>
		public void Initialize(SettingsData settingsData);
		/// <summary>
		/// Загружает свои данные в settingsData
		/// </summary>
		public void WriteTo(SettingsData settingsData);
	}
}
