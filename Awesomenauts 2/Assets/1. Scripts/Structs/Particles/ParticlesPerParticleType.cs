using System;
using Enums.Particles;
using Interfaces;
using UnityEngine;

namespace Structs.Particles
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