using System;
using Interfaces;
using UnityEngine;
using Enums.Audio;

namespace Structs.Audio
{
	[Serializable]
	public struct InitialValuePerBus : IKeyValuePair<BusType, float>
	{
		[SerializeField]
		private BusType key;

		[SerializeField]
		private float value;

		public bool isMuted;

		public BusType Key
		{
			get => key;
			set => key = value;
		}

		public float Value
		{
			get => value;
			set => this.value = value;
		}

		public bool Equals(IKeyValuePair<BusType, float> other)
		{
			return other != null && other.Key == Key;
		}
	}
}