using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeInOut : MonoBehaviour
{
    [SerializeField] private GameObject FadeObject;

    public IEnumerator FadeIn()
    {
        FadeObject.SetActive(true);
        for(float f = 1f; f > 0f; f -= 0.005f)
        {
            Color color = FadeObject.GetComponent<Image>().color;
            color.a = f;
            FadeObject.GetComponent<Image>().color = color;
            yield return null;
        }
        yield return new WaitForSeconds(1f);
        FadeObject.SetActive(false);
    }
    
    public IEnumerator FadeOut()
    {
        FadeObject.SetActive(true);
        for(float f = 0f; f < 1f; f += 0.02f)
        {
            Color color = FadeObject.GetComponent<Image>().color;
            color.a = f;
            FadeObject.GetComponent<Image>().color = color;
            yield return null;
        }
    }
}
