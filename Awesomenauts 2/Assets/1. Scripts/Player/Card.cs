using Mirror;
using UnityEngine;

public class Card : NetworkBehaviour
{
	public MeshRenderer CoverUpRenderer;
    // Start is called before the first frame update
    void Start()
    {
	    SetCoverState(isClient && !hasAuthority);
    }

	/// <summary>
	/// Sets the Cover Up Renderer Active or Inactive.
	/// </summary>
	/// <param name="covered"></param>
    public void SetCoverState(bool covered)
    {
	    Debug.Log("Enable Cover Up Renderer: " + covered);
		CoverUpRenderer.enabled = covered;
	}
}
