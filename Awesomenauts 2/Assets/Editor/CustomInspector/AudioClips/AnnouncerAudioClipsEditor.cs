using Enums.Audio;
using ScriptableObjects;
using Structs.Audio;
using UnityEditor;

namespace CustomInspector.AudioClips
{
	[CustomEditor(typeof(AnnouncerAudioClips)), CanEditMultipleObjects]
	public class AnnouncerAudioClipsEditor : AAudioClipsEditor<AnnouncerSoundType, AnnouncerClipsPerSoundType> { }
}