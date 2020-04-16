namespace VDFramework.Extensions
{
	public static class Int32Extentions
	{
		/// <summary>
		/// Check every bit one by one to see if they are equal
		/// </summary>
		/// <returns>TRUE if at least one bit is equal</returns>
		public static bool HasOneMatchingBit(this int number, int toCheck, bool shouldCheckZero = false)
		{
			if (number == toCheck)
			{
				return true;
			}
			
			for (int currentBit = 0; currentBit < 32; currentBit++)
			{
				int bitToCheck = 1 << currentBit;

				int myBit = number & bitToCheck;

				if (!shouldCheckZero && myBit == 0)
				{
					continue;
				}

				int theirBit = toCheck & bitToCheck;
				
				if (myBit == theirBit)
				{
					return true;
				}
			}

			return false;
		}
	}
}