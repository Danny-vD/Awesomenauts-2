using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using Guid = System.Guid;

namespace ExtentionMethods.FMODUnity
{
	public static class StudioEventEmitterExtentions
	{
		private static readonly Dictionary<string, Guid> guidsPerEvent = new Dictionary<string, Guid>();
		
		public static PARAMETER_ID GetParameterID(this StudioEventEmitter emitter, string parameterName)
		{
			EventDescription eventDescription = RuntimeManager.GetEventDescription(GetGuid(emitter.Event));
			eventDescription.getParameterDescriptionByName(parameterName, out PARAMETER_DESCRIPTION parameterDescription);
			return parameterDescription.id;
		}

		private static Guid GetGuid(string emitterEventPath)
		{
			if (guidsPerEvent.TryGetValue(emitterEventPath, out Guid guid))
			{
				return guid;
			}

			guid = RuntimeManager.PathToGUID(emitterEventPath);
			guidsPerEvent.Add(emitterEventPath, guid);
			
			return guid;
		}
	}
}