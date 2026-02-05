using UnityEngine;

public class FixCamera : MonoBehaviour
{
    void Start()
    {
        Camera cam = GetComponent<Camera>();
        Rect screen = cam.rect;
        screen.width = 1;
        float aspectRatio = 4f / 3f;

        // ajusta la pantalla
        float windowRatio = (float) Screen.width / Screen.height;
        screen.width = aspectRatio / (windowRatio);

        // mueve la camara 
        screen.x = (1 - screen.width) / 2f;
        cam.rect = screen;
    }
}
