using System;
using AwsomenautsCardGame.Enums.Particles;
using AwsomenautsCardGame.Interfaces;
using UnityEngine;

namespace AwsomenautsCardGame.Structs.Particles
{
	[Serializable]
	public struct ParticlesPerParticleType : IKeyValuePair<ParticleType, GameObject>
	{
		[SerializeField]
		private ParticleType key;
		
		[SerializeField]
		private GameObject value;

		public ParticleType Key
		{
			get => key;
			set => key = value;
		}

		public GameObject Value
		{
			get => value;
			set => this.value = value;
		}

		public bool Equals(IKeyValuePair<ParticleType, GameObject> other)
		{
			return Key == other.Key;
		}
	}
}