using UnityEngine;

public class Segment2 : MonoBehaviour
{
    private Transform _point1, _point2;
    public Color SetColor
    {
        set
        {
            this.gameObject.GetComponent<MeshRenderer>().material.color = value;
        }
    }

    private void Update()
    {
        var leght = Mathf.Abs((_point1.position - _point2.position).magnitude);
        transform.localScale = new Vector3(0.5f, leght, 0.5f);
        transform.up = (_point1.position - _point2.position).normalized;
        transform.position = _point1.position + (_point2.position - _point1.position);
    }

    public void SetPoints(Transform point1, Transform point2)
    {
        _point1 = point1;
        _point2 = point2;
    }
}
