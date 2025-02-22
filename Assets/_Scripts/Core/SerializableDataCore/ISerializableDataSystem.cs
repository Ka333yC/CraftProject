﻿using _Scripts.Core.SettingsCore;

namespace _Scripts.Core.GameProgressCore
{
	public interface ISerializableDataSystem
	{
		/// <summary>
		/// Загружает свои данные из dataContainer
		/// </summary>
		public void Initialize(SerializableDataContainer dataContainer);
		/// <summary>
		/// Загружает свои данные в dataContainer
		/// </summary>
		public void WriteTo(SerializableDataContainer dataContainer);
	}
}