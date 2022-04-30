using UnityEngine;

public class Utils : MonoBehaviour
{
    public static Vector3 ScreenToWorld(Camera camera, Vector3 position)
    {
        position.z = camera.transform.position.z + .5f;
        return camera.ScreenToWorldPoint(position);
    }
}
