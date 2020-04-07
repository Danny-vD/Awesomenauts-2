using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EntityStatistics
{
	[HideInInspector]
	public Dictionary<CardPlayerStatType, CardPlayerStat> Stats;

	public List<InternalStat> StartStatistics;

	public delegate void OnStatTypeChanged(object newValue);
	private Dictionary<CardPlayerStatType, OnStatTypeChanged> registeredEvents = new Dictionary<CardPlayerStatType, OnStatTypeChanged>();

	public void Register(CardPlayerStatType type, OnStatTypeChanged del, bool directUpdate = false)
	{
		if (!registeredEvents.ContainsKey(type)) registeredEvents[type] = del;
		else registeredEvents[type] += del;

		if (directUpdate && HasValue(type))
		{
			del(GetValue(type)); //Update directly after registration
		}
	}

	public void InitializeStatDictionary()
	{
		if (StartStatistics == null) StartStatistics = new List<InternalStat>();
		Stats = new Dictionary<CardPlayerStatType, CardPlayerStat>();
		foreach (InternalStat startStatistic in StartStatistics)
		{
			if (startStatistic.dataType == CardPlayerStatDataType.Int)
			{
				Stats.Add(startStatistic.type, new CardPlayerStat<int>(int.Parse(startStatistic.value), CardPlayerStatDataType.Int));
			}
			else if (startStatistic.dataType == CardPlayerStatDataType.Float)
			{
				Stats.Add(startStatistic.type, new CardPlayerStat<float>(float.Parse(startStatistic.value), CardPlayerStatDataType.Float));
			}
			else if (startStatistic.dataType == CardPlayerStatDataType.String)
			{
				Stats.Add(startStatistic.type, new CardPlayerStat<string>(startStatistic.value, CardPlayerStatDataType.String));
			}
		}

	}

	public bool HasValue(CardPlayerStatType type) => Stats != null && Stats.ContainsKey(type);

	public T GetValue<T>(CardPlayerStatType type)
	{
		object obj = GetValue(type);
		if (obj != null) return (T)obj;
		return default;
	}

	public object GetValue(CardPlayerStatType type)
	{
		if (Stats.ContainsKey(type)) return Stats[type].GetValue();
		return null;
	}

	public void SetValue<T>(CardPlayerStatType type, T value) => SetValue(type, (object)value);

	public void SetValue(CardPlayerStatType type, object value)
	{
		Debug.Log($"Setting Stat Type: {type}to value: {value}");

		if (registeredEvents.ContainsKey(type))
			registeredEvents[type](value); //Call the Events.
		if (Stats.ContainsKey(type)) Stats[type].SetValue(value);
	}
}