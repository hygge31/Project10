using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightController : MonoBehaviour
{
    public Light _light;
    public bool onLight;
    public Gradient gradient;
    public AnimationCurve animationCurve;



    public void LigtOn()
    {
        StartCoroutine(LightOnCo());
    }

    IEnumerator LightOnCo()
    {
        _light.enabled = true;
        float percent = 0;
        while (onLight)
        {
            percent += Time.deltaTime;
            percent %= 1;
            float interporation = (-Mathf.Pow(percent, 2) + percent) * 4;
            _light.color = gradient.Evaluate(percent);
            _light.intensity = animationCurve.Evaluate(percent);

            yield return null;
        }
        _light.enabled = false;
    }
}
