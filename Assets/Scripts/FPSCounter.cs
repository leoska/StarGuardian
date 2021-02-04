using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FPSCounter : MonoBehaviour
{
    public Text TextFPS;
    public float  timer;

    private float _fps;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        timer += 2 * Time.deltaTime;
        if (timer >1)
        {
            _fps += (Time.unscaledDeltaTime - _fps) * 0.1f;
            float FPS = 1f / _fps;
            TextFPS.text = "FPS: " + Mathf.Ceil(FPS).ToString();
            timer = 0;
        }
    }
}
