using System.Collections.Generic;
using System.Linq;
using Enums.Particles;
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
			GameObject toInstantiate = GetGameObject(particleType);

			GameObject instantiated = parent
				? Instantiate(toInstantiate, position, Quaternion.identity, parent)
				: Instantiate(toInstantiate, position, Quaternion.identity);

			return instantiated;
		}

		private GameObject GetGameObject(ParticleType particleType)
		{
			return particlesPerParticleType.First(item => item.Key.Equals(particleType)).Value;
		}
	}
}