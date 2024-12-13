using System;

namespace Assets.Scripts.Core.UICore
{
	public class ReactiveProperty<T>
	{
		private T _value;

		public event Action<T> OnChanged;

		public ReactiveProperty() : this(default(T))
		{
		}

		public ReactiveProperty(T defaultValue)
		{
			_value = defaultValue;
		}

		public T Value
		{
			get
			{
				return _value;
			}

			set
			{
				if(_value != null && _value.Equals(value))
				{
					return;
				}

				_value = value;
				OnChanged?.Invoke(_value);
			}
		}

		public override string ToString()
		{
			return Value.ToString();
		}
	}
}
