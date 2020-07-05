using System;
using UnityEngine;
using VDFramework.Exceptions;

namespace VDFramework.Singleton
{
	public abstract class Singleton<TSingleton> : BetterMonoBehaviour
		where TSingleton : Singleton<TSingleton>
	{
		private static TSingleton instance;

		public static TSingleton Instance
		{
			get
			{
				// ReSharper disable once ConvertIfStatementToNullCoalescingExpression
				if (instance == null)
				{
					if (typeof(MonoBehaviour).IsAssignableFrom(typeof(TSingleton))) //Fix to prevent invalid mono behaviours to be created.
					{
                        Debug.LogWarning("Use of singleton(" + typeof(TSingleton).Name + ") before it was assigned.");
					}
					instance = SingletonInstanceCreator<TSingleton>.CreateInstance();
				}

				return instance;
			}
			private set => instance = value;
		}

		public static TSingleton InstanceIfInitialized => IsInitialized ? instance : null;

		public static bool IsInitialized => instance != null;

		protected virtual void Awake()
		{
			if (!IsInitialized)
			{
				Instance = this as TSingleton;
			}
			else
			{
				DestroyThis(false);
				throw new SingletonViolationException();
			}
		}

		protected virtual void OnDestroy()
		{
			if (instance == this)
			{
				instance = null;
			}
		}

		/// <summary>
		/// Sets the instance of the singleton to null.
		/// </summary>
		public void DestroyInstance()
		{
			DestroyThis(true);
		}

		private void DestroyThis(bool destroyInstance)
		{
			if (destroyInstance)
			{
				instance = null;
			}

			if (gameObject.name.ToLower().Contains("singleton"))
			{
				Destroy(gameObject);
				return;
			}

			Destroy(this);
		}
	}
}