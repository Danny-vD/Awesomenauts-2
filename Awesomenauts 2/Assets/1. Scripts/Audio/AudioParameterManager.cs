using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;

namespace Audio
{
	public static class AudioParameterManager
	{
		private const string masterBusPath = "Bus:/";
		
		private static readonly Dictionary<string, PARAMETER_ID> globalParameters =
			new Dictionary<string, PARAMETER_ID>();

		private static readonly Dictionary<string, Bus> buses = new Dictionary<string, Bus>();

		private static float masterVolume;
		private static bool masterMute;
		
		/////////////////////////////////////////////////
		//			Global parameters
		/////////////////////////////////////////////////

		/// <summary>
		/// Sets a global parameter by retrieving its ID from the eventDescription
		/// </summary>
		public static void SetGlobalParameter(string eventPath, string parameter, float newValue)
		{
			RuntimeManager.StudioSystem.setParameterByID(GetGlobalParameterID(eventPath, parameter), newValue);
		}

		/// <summary>
		/// Sets a global parameter by name or ID if cached
		/// <para>Use SetGlobalParameter(string, string, float) to cache the ID</para>
		/// </summary>
		public static void SetGlobalParameter(string parameter, float newValue)
		{
			if (globalParameters.TryGetValue(parameter, out PARAMETER_ID id))
			{
				RuntimeManager.StudioSystem.setParameterByID(id, newValue);
				return;
			}

			RuntimeManager.StudioSystem.setParameterByName(parameter, newValue);
		}

		private static PARAMETER_ID GetGlobalParameterID(string eventPath, string parameter)
		{
			if (globalParameters.TryGetValue(parameter, out PARAMETER_ID id))
			{
				return id;
			}

			return AddNewCachedParameter(eventPath, parameter);
		}

		private static PARAMETER_ID GetParameterIDInternal(string eventPath, string parameter)
		{
			EventDescription eventDescription =
				RuntimeManager.GetEventDescription(RuntimeManager.PathToGUID(eventPath));
			eventDescription.getParameterDescriptionByName(parameter, out PARAMETER_DESCRIPTION parameterDescription);
			return parameterDescription.id;
		}

		private static PARAMETER_ID AddNewCachedParameter(string eventPath, string parameter)
		{
			PARAMETER_ID id = GetParameterIDInternal(eventPath, parameter);

			globalParameters.Add(parameter, id);
			return id;
		}

		/////////////////////////////////////////////////
		//			Local parameters
		/////////////////////////////////////////////////

		/// <summary>
		/// Will set the parameter to the newValue for **ALL** instances of this event
		/// </summary>
		public static void SetEventParameter(string eventPath, string parameter, float newValue)
		{
			RuntimeManager.GetEventDescription(RuntimeManager.PathToGUID(eventPath))
				.getInstanceList(out EventInstance[] instances);

			foreach (EventInstance instance in instances)
			{
				instance.setParameterByName(parameter, newValue);
			}
		}

		/////////////////////////////////////////////////
		//			Bus parameters
		/////////////////////////////////////////////////
		
		public static void SetBusVolume(string busPath, float volume)
		{
			if (busPath == masterBusPath)
			{
				SetMasterVolume(volume);
				return;
			}
			
			Bus bus = GetBus(busPath);
			bus.setVolume(volume);
		}

		public static void SetMasterVolume(float volume, bool updateCached = true)
		{
			if (updateCached)
			{
				masterVolume = volume;
			}

			if (masterMute)
			{
				return;
			}
			
			Bus bus = GetBus(masterBusPath);
			bus.setVolume(volume);
		}

		public static void SetBusMute(string busPath, bool isMuted)
		{
			Bus bus = GetBus(busPath);
			bus.setMute(isMuted);
		}

		public static void SetMasterMute(bool isMuted)
		{
			SetMasterVolume(isMuted ? 0 : masterVolume, false);
			
			masterMute = isMuted;
		}

		public static float GetBusVolume(string busPath)
		{
			if (busPath == masterBusPath)
			{
				return masterVolume;
			}
			
			Bus bus = GetBus(busPath);
			bus.getVolume(out float volume);
			
			return volume;
		}

		private static Bus GetBus(string busPath)
		{
			if (buses.TryGetValue(busPath, out Bus bus))
			{
				return bus;
			}

			bus = RuntimeManager.GetBus(busPath);
			buses.Add(busPath, bus);
			return bus;
		}
	}
}