using System;
using UnityEngine;

namespace Networking {
	[Serializable]
	public class BorderInfo
	{
		public bool IsValid => BorderMaterial != null && BorderMesh != null;
		public Mesh BorderMesh;
		public Material BorderMaterial;
	}
}