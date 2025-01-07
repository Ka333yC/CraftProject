namespace _Scripts.Apart.ObjectPoolPattern
{
	public interface IPool<T>
	{
		public T Get();
		public void Return(T toSetFree);
	}
}
