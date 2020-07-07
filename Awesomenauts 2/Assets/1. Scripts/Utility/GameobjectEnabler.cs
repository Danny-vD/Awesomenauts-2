using VDFramework.VDUnityFramework.BaseClasses;

namespace AwsomenautsCardGame.Utility
{
	public class GameobjectEnabler : BetterMonoBehaviour
	{
		public void SetActiveState(bool isActive)
		{
			gameObject.SetActive(isActive);
		}

		public void SetActive()
		{
			SetActiveState(true);
		}

		public void SetDisabled()
		{
			SetActiveState(false);
		}

		public void ToggleActive()
		{
			SetActiveState(!gameObject.activeSelf);
		}
	}
}