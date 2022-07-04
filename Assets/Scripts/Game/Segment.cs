using System;
using UnityEngine;

public class Segment : MonoBehaviour
{
    [SerializeField] private Joint joint;
    [SerializeField] private Rigidbody _rigidbody;

    public event Action Activate;
    public Rigidbody Rigidbody => _rigidbody;
    public void SetConnectedBody(Rigidbody rigidbody)
    { 
        joint.connectedBody = rigidbody;
    }
    public void ReetConnectedBody()
          => joint.connectedBody = null;
    public Color SetColor
    {
        set
        {
            this.gameObject.GetComponent<MeshRenderer>().material.color = value;
        }
    }


    private void OnMouseDown()
    {
        Activate?.Invoke();
    }
}
