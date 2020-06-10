using System;
using System.Collections.Generic;
using System.Linq;
using Enums.Audio;
using FMODUnity;
using Structs.Audio;
using UnityEngine;
using Utility;
using VDFramework.Extensions;
using EventType = Enums.Audio.EventType;

namespace Audio
{
	[Serializable]
	public class EventPaths
	{
		[SerializeField]
		private List<EventPathPerEvent> events = new List<EventPathPerEvent>();

		[SerializeField]
		private List<BusPathPerBus> buses = new List<BusPathPerBus>();

		[SerializeField]
		private List<EventsPerEmitter> emitterEvents = new List<EventsPerEmitter>();

		private readonly Dictionary<EmitterType, StudioEventEmitter> emitters =
			new Dictionary<EmitterType, StudioEventEmitter>();

		public EventPaths()
		{
			buses.Add(new BusPathPerBus() {Key = BusType.Master, Value = "Bus:/"});
		}

		public void UpdateDictionaries()
		{
			FakeDictionaryUtil.PopulateEnumDictionary<EventPathPerEvent, EventType, string>(events);

			FakeDictionaryUtil.PopulateEnumDictionary<BusPathPerBus, BusType, string>(buses);

			FakeDictionaryUtil.PopulateEnumDictionary<EventsPerEmitter, EmitterType, EventType>(emitterEvents);
		}

		public void AddEmitters(GameObject gameObject)
		{
			foreach (EmitterType emitterType in default(EmitterType).GetValues())
			{
				StudioEventEmitter emitter = gameObject.AddComponent<StudioEventEmitter>();
				emitter.Event = GetPathForEmitter(emitterType);

				emitters.Add(emitterType, emitter);
			}
		}

		public string GetPath(EventType eventType)
		{
			return events.First(item => item.Key.Equals(eventType)).Value;
		}

		public string GetPath(BusType busType)
		{
			return buses.First(item => item.Key.Equals(busType)).Value;
		}

		public StudioEventEmitter GetEmitter(EmitterType emitterType)
		{
			return emitters[emitterType];
		}

		private string GetPathForEmitter(EmitterType emitterType)
		{
			EventType eventType = emitterEvents.First(item => item.Key == emitterType).Value;
			return GetPath(eventType);
		}
	}
}