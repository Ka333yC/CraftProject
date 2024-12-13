namespace Assets.Scripts.Apart.Extensions
{
	public static class FastNoiseLiteExtensions
	{
		/// <summary>
		/// 2D noise at given position using current settings
		/// </summary>
		/// <returns>
		/// Noise output bounded between 0...1
		/// </returns>
		public static float GetNoise01(this FastNoiseLite noise, float x, float y)
		{
			return noise.GetNoise(x, y) * 0.5f + 0.5f;
		}

		/// <summary>
		/// 3D noise at given position using current settings
		/// </summary>
		/// <returns>
		/// Noise output bounded between 0...1
		/// </returns>
		public static float GetNoise01(this FastNoiseLite noise, float x, float y, float z)
		{
			return noise.GetNoise(x, y, z) * 0.5f + 0.5f;
		}
	}
}
