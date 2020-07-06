using System;
using System.Collections.Generic;
using System.Linq;
using VDFramework.SharedClasses.Extensions;
using AwsomenautsCardGame.Enums.Audio;
using AwsomenautsCardGame.Structs.Audio;
using AwsomenautsCardGame.Utility;
using FMODUnity;
using UnityEngine;
using EventType = AwsomenautsCardGame.Enums.Audio.EventType;

namespace AwsomenautsCardGame.Audio
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
			FakeDictionaryUtil.PopulateEnumDictionary<EventPathPerEvent, Enums.Audio.EventType, string>(events);

			FakeDictionaryUtil.PopulateEnumDictionary<BusPathPerBus, BusType, string>(buses);

			FakeDictionaryUtil.PopulateEnumDictionary<EventsPerEmitter, EmitterType, Enums.Audio.EventType>(emitterEvents);
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

		public string GetPath(Enums.Audio.EventType eventType)
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
			Enums.Audio.EventType eventType = emitterEvents.First(item => item.Key == emitterType).Value;
			return GetPath(eventType);
		}
	}
}