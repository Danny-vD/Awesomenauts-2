using Enums.Audio;
using ScriptableObjects;
using Structs.Audio;
using UnityEditor;

namespace CustomInspector.AudioSets
{
	[CustomEditor(typeof(NautAudioSet)), CanEditMultipleObjects]
	public class NautAudioSetEditor : AAudioSetEditor<NautSound, ClipDataPerNautSound> { }
}