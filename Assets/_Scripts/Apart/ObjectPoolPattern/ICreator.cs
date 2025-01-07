namespace _Scripts.Apart.ObjectPoolPattern
{
	public interface ICreator<T>
	{
		/// <summary>
		/// Возвращает вновь созданный объект
		/// </summary>
		/// <returns></returns>
		public T Create();
	}
}
