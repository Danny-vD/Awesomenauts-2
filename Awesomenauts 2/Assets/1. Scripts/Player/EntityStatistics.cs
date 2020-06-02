using System;
using System.Collections.Generic;
using UnityEngine;

namespace Player {
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

		public enum CardPlayerStatDataType
		{
			Int,
			Float,
			String
		}

		[Serializable]
		public class InternalStat
		{
			public CardPlayerStatType type;
			public CardPlayerStatDataType dataType;
			public string value;
		}

		public void InitializeStatDictionary()
		{
			if (StartStatistics == null) StartStatistics = new List<InternalStat>();
			Stats = new Dictionary<CardPlayerStatType, CardPlayerStat>();
			foreach (InternalStat startStatistic in StartStatistics)
			{
				if (startStatistic.dataType == CardPlayerStatDataType.Int)
				{
					Stats.Add(startStatistic.type, new CardPlayerStat<int>(int.Parse(startStatistic.value)));
				}
				else if (startStatistic.dataType == CardPlayerStatDataType.Float)
				{
					Stats.Add(startStatistic.type, new CardPlayerStat<float>(float.Parse(startStatistic.value)));
				}
				else if (startStatistic.dataType == CardPlayerStatDataType.String)
				{
					Stats.Add(startStatistic.type, new CardPlayerStat<string>(startStatistic.value));
				}
			}

		}

		public void Invalidate()
		{
			foreach (KeyValuePair<CardPlayerStatType, OnStatTypeChanged> onStatTypeChanged in registeredEvents)
			{
				onStatTypeChanged.Value?.Invoke(GetValue(onStatTypeChanged.Key));
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
			Debug.Log($"Setting Stat Type: {type} to value: {value}");

			if (registeredEvents.ContainsKey(type))
				registeredEvents[type](value); //Call the Events.
			if (Stats.ContainsKey(type)) Stats[type].SetValue(value);
			else
			{
				CardPlayerStat stat=null;
				if (value is int v)
				{
					stat = new CardPlayerStat<int>(v);
				}
				else if (value is Enum e)
				{
					stat = new CardPlayerStat<int>((int)Convert.ChangeType(e, typeof(int)));
				}
				else if (value is float f)
				{
					stat = new CardPlayerStat<float>(f);
				}
				else
				{
					stat = new CardPlayerStat<string>(value.ToString());
				}

				Stats.Add(type, stat);
			}
		}
	}
}