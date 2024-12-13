namespace Assets.Scripts.Undone.SettingsCore
{
	public interface ISettingsSystem
	{
		/// <summary>
		/// Загружает свои данные из settingsData
		/// </summary>
		public void GetFrom(SettingsData settingsData);
		/// <summary>
		/// Загружает свои данные в settingsData
		/// </summary>
		public void SetTo(SettingsData settingsData);
	}
}
