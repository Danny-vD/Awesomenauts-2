using UnityEngine;

public class CardSocket : MonoBehaviour
{
    private float origY;
    public float yScale;
    public float yOffset;
    public float yCardOffset;
    public float timeScale;
    public float timeOffset;

    private bool Active;

    private Card dockedTransform;

    // Start is called before the first frame update
    void Start()
    {
        origY = transform.position.y;
    }

	/// <summary>
	/// Activates or deactivates the Socket Movement
	/// </summary>
	/// <param name="active"></param>
    public void SetActive(bool active)
    {
        Active = active;
        if (!Active) ResetPositions();
    }

	/// <summary>
	/// Docks a transform to the Socket
	/// This way the transform will be moving with the Socket.
	/// </summary>
	/// <param name="dockedTransform"></param>
    public void DockCard(Card dockedTransform)
    {
        this.dockedTransform = dockedTransform;
    }


    private void ResetPositions()
    {
        Vector3 pos = transform.position;
        pos.y = origY;
        if (dockedTransform != null)
        {
            dockedTransform.transform.position = pos + Vector3.up * yCardOffset;
        }
        transform.position = pos;
    }

    // Update is called once per frame
    void Update()
    {
        if (!Active) return;
        Vector3 pos = transform.position;
        pos.y = origY + yOffset + Mathf.Sin(Time.realtimeSinceStartup * timeScale + timeOffset) * yScale;
        if (dockedTransform != null)
        {
            dockedTransform.transform.position = pos + Vector3.up * yCardOffset;
        }
        transform.position = pos;
    }
}
