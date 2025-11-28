using UnityEngine;

class CameraManager : LazySingleton<CameraManager>
{
    [SerializeField] private Camera mainCamera;

    public void MoveCameraUp(float offset)
    {
        mainCamera.transform.position += offset * Vector3.up;
    }
}