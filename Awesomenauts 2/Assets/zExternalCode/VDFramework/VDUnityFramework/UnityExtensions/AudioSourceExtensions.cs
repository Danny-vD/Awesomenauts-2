using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace VDFramework.VDUnityFramework.UnityExtensions
{
	public static class AudioSourceExtensions
	{
		public static AudioSource GetFirstNotPlaying(this IEnumerable<AudioSource> collection)
		{
			int count = collection.Count();

			return count == 0 ? null : collection.FirstOrDefault(audioSource => !audioSource.isPlaying);
		}
	}
}