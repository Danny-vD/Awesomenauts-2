namespace Networking {
	public class TeamAsset<T>
	{
		public T RedTeam;
		public T BlueTeam;
		public T Get(int teamID) => teamID == 0 ? RedTeam : BlueTeam;
	}
}