using System.Diagnostics.CodeAnalysis;

namespace Enums.Audio
{
	[SuppressMessage("ReSharper", "InconsistentNaming")]
	public enum EventType
	{
		SFX_CARDS_CardDeath,
		SFX_CARDS_CardDraw,
		SFX_CARDS_CardHit,
		SFX_CARDS_MeleeAttack,
		SFX_CARDS_CardPlace,
		SFX_CARDS_RangedAttack,
		SFX_NEXUS_NexusHit,
		SFX_TURRET_TurretAttack,
		MUSIC_Music,
		AMBIENT_Ambient,
	}
}