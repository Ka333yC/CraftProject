namespace ObjectPoolPattern
{
	public interface IPool<T>
	{
		public T Get();
		public void Return(T toSetFree);
	}
}
