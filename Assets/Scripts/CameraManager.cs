using UnityEngine;

class CameraManager : LazySingleton<CameraManager>
{
    [SerializeField] private Camera mainCamera;

    public void MoveCameraUp(float offset)
    {
        // TODO: It should'n be 1.5, it should be calculated according to the camera angle.
        mainCamera.transform.position += offset / 1.5f * (mainCamera.transform.up - mainCamera.transform.forward);
    }
}