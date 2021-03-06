﻿using System;

namespace Interfaces
{
	public interface IKeyValuePair<TKey, TValue> : IEquatable<IKeyValuePair<TKey, TValue>>
	{
		TKey Key { get; set; }
		TValue Value { get; set; }
	}
}