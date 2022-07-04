using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float limitX;

    void Update()
    {
        for (int i = 0; i < Input.touchCount; ++i)
        {
            if (Input.GetTouch(i).phase.Equals(TouchPhase.Moved))
            {
                var mPlane = new Plane(Vector3.back, Vector3.zero);
                Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(i).position);
                float dist = 0f;

                if (mPlane.Raycast(ray, out dist))
                    if (Mathf.Abs(ray.GetPoint(dist).x) < limitX)
                    {
                        Camera.main.transform.position = new Vector3(ray.GetPoint(dist).x, transform.position.y, transform.position.z);
                    }
            }
        }
    }

}
