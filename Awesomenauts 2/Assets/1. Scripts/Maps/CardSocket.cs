using System.Collections;
using System.Collections.Generic;
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

    private Transform dockedTransform;

    // Start is called before the first frame update
    void Start()
    {
        origY = transform.position.y;
    }

    public void SetActive(bool active)
    {
        Active = active;
        if (!Active) ResetPositions();
    }

    public void DockTransform(Transform dockedTransform)
    {
        this.dockedTransform = dockedTransform;
    }


    private void ResetPositions()
    {
        Vector3 pos = transform.position;
        pos.y = origY;
        if (dockedTransform != null)
        {
            dockedTransform.position = pos + Vector3.up * yCardOffset;
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
            dockedTransform.position = pos + Vector3.up * yCardOffset;
        }
        transform.position = pos;
    }
}
