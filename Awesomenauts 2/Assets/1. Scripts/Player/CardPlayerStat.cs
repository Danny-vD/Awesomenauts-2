using System;

namespace Player {
	public abstract class CardPlayerStat
	{
		public abstract object GetValue();
		public abstract void SetValue(object value);
	}

	public class CardPlayerStat<T> : CardPlayerStat
	{
		public T Value;


		public CardPlayerStat(T value) : base()
		{
			Value = value;
		}

		public override object GetValue()
		{
			return Value;
		}

		public override void SetValue(object value)
		{
			if (value is T v)
			{
				Value = v;
				return;
			}
			throw new ArgumentException("Object is not of the correct type, Expected: "+ typeof(T)+ " got: "+ value?.GetType());
		}
	}
}