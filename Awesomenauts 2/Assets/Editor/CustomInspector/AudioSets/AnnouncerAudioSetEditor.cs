using Enums.Audio;
using ScriptableObjects;
using Structs.Audio;
using UnityEditor;

namespace CustomInspector.AudioSets
{
	[CustomEditor(typeof(AnnouncerAudioSet)), CanEditMultipleObjects]
	public class AnnouncerAudioSetEditor : AAudioSetEditor<AnnouncerSound, ClipDataPerAnnouncerSound> { }
}