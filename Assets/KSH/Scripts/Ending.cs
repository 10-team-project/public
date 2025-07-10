using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text;
using EditorAttributes;
using SHG;

public class Ending : MonoBehaviour
{
    [SerializeField] private Camera Badcamera;
    [SerializeField] private Camera Happycamera;
    [SerializeField] private GameObject BadEnding;
    [SerializeField] private GameObject HappyEnding;
    [SerializeField] private TMP_Text[] Badtext;
    [SerializeField] private TMP_Text[] Happytext;
    [SerializeField] private string[] originalTexts;
    [SerializeField] private FadeInOut fadeInOut;
    static public bool IS_HAPPY = false;
    
    private void Start()
    {
      App.Instance.AudioManager.PlayBGM(
        App.Instance.AudioManager.BadEndingSound
        );
        //Badcamera.gameObject.SetActive(false);
        Happycamera.gameObject.SetActive(false);
        //BadEnding.SetActive(false);
        HappyEnding.SetActive(false);
        End(BadEnding, Badtext, Badcamera);
    }

    [Button("Ending")]
    private void EndingTest()
    {
        if (IS_HAPPY)
        {
            End(HappyEnding, Happytext, Happycamera);
        }
        else
        {
            End(BadEnding, Badtext, Badcamera);
        }
    }

    public void End(GameObject gameObject, TMP_Text[] texts, Camera camera)
    {
        StartCoroutine(EndRoutine(gameObject, texts, camera));
    }
    
    private IEnumerator EndRoutine(GameObject gameObject, TMP_Text[] texts, Camera camera)
    {
        camera.gameObject.SetActive(true);
        gameObject.SetActive(true);
        
        yield return StartCoroutine(fadeInOut.FadeIn());
        
        yield return StartCoroutine(TypeTextEffect(texts, originalTexts));

        yield return StartCoroutine(fadeInOut.FadeOut());
    }
    
    IEnumerator TypeTextEffect(TMP_Text[] text, string[] fulltexts)
    {
            for (int i = 0; i < text.Length; i++) 
            {
              string fulltext = fulltexts[i];
              text[i].text = string.Empty;

              StringBuilder sb = new StringBuilder();

              for (int j = 0; j < fulltext.Length; j++)
              {
                  sb.Append(fulltext[j]);
                  text[i].text = sb.ToString();
                  yield return new WaitForSeconds(0.1f);
              }
              yield return new WaitForSeconds(0.5f);
             }
      }
    }
