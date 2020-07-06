using System;

namespace AwsomenautsCardGame.ScriptableObjects.Effects {
	public class EffectException : Exception
	{
		public EffectException(string message, Exception inner):base(message, inner) { }
		public EffectException(string message):this(message, null) { }
	}
}