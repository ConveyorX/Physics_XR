using UnityEngine;

public class FramerateClamp : MonoBehaviour
{
    public int maxFPS = 60;

    private void Start()
    {
        Application.targetFrameRate = maxFPS;
    }
}
