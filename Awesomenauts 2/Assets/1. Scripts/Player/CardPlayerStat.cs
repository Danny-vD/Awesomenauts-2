using System;

public abstract class CardPlayerStat
{
	public CardPlayerStatDataType Type;

	protected CardPlayerStat(CardPlayerStatDataType type)
	{
		Type = type;
	}
	public abstract object GetValue();
	public abstract void SetValue(object value);
}

public class CardPlayerStat<T> : CardPlayerStat
{
	public T Value;


	public CardPlayerStat(T value, CardPlayerStatDataType type) : base(type)
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
		throw new ArgumentException("Object is not of the correct type");
	}
}
