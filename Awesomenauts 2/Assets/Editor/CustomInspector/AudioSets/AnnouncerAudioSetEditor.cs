using Enums.Audio;
using ScriptableObjects.Audio.AudioSet;
using Structs.Audio;
using UnityEditor;

namespace CustomInspector.AudioSets
{
	[CustomEditor(typeof(AnnouncerAudioSet)), CanEditMultipleObjects]
	public class AnnouncerAudioSetEditor : AAudioSetEditor<AnnouncerSound, ClipDataPerAnnouncerSound> { }
}