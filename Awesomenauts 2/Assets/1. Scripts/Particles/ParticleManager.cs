using System.Collections.Generic;
using System.Linq;
using Enums.Particles;
using Structs.Audio;
using Structs.Particles;
using UnityEngine;
using Utility;
using VDFramework.Singleton;

namespace Particles
{
	public class ParticleManager : Singleton<ParticleManager>
	{
		[SerializeField]
		private List<ParticlesPerParticleType> particlesPerParticleType = new List<ParticlesPerParticleType>();

		protected override void Awake()
		{
			base.Awake();
			DontDestroyOnLoad(gameObject);
		}
		
		public void UpdateDictionaries()
		{
			FakeDictionaryUtil.PopulateEnumDictionary<ParticlesPerParticleType, ParticleType, GameObject>(particlesPerParticleType);
		}

		public GameObject InstantiateParticle(ParticleType particleType, Vector3 position, Transform parent = null)
		{
			GameObject toSpawn = GetGameObject(particleType);

			GameObject spawned = parent
				? Instantiate(toSpawn, position, Quaternion.identity, parent)
				: Instantiate(toSpawn, position, Quaternion.identity);

			return spawned;
		}

		private GameObject GetGameObject(ParticleType particleType)
		{
			return particlesPerParticleType.First(item => item.Key.Equals(particleType)).Value;
		}
	}
}