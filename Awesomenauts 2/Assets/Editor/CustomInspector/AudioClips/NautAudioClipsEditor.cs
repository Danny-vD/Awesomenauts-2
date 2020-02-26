using Enums.Audio;
using ScriptableObjects;
using Structs.Audio;
using UnityEditor;

namespace CustomInspector.AudioClips
{
	[CustomEditor(typeof(NautAudioClips)), CanEditMultipleObjects]
	public class NautAudioClipsEditor : AAudioClipsEditor<NautSoundType, NautClipsPerSoundType> { }
}