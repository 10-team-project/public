using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LTH
{
    [System.Serializable]
    public class TransitionObject
    {
        public Transform target;

        [HideInInspector] public Vector3 initialPosition;
        [HideInInspector] public Quaternion initialRotation;

        public Vector3 horrorOffset;
        public Vector3 horrorRotation;
    }

    public class SceneTraumaTransition : MonoBehaviour
    {
        [Header("Dynamic Objects")]
        [SerializeField] private TransitionObject[] transitionObjects;

        [Header("Backgrounds")]
        [SerializeField] public GameObject[] peacefulBackgrounds;
        [SerializeField] public GameObject[] horrorBackgrounds;

        [Header("Lighting")]
        [SerializeField] public Light directionalLight;
        [SerializeField] public Color peacefulLightColor = Color.white;
        [SerializeField] public Color horrorLightColor = new Color(0.3f, 0.1f, 0.1f);
        [SerializeField] public float peacefulLightIntensity = 1f;
        [SerializeField] public float horrorLightIntensity = 0.3f;

        [Header("Skybox")]
        [SerializeField] public Material peacefulSkybox;
        [SerializeField] public Material horrorSkybox;

        [Header("Audio")]
        [SerializeField] public AudioSource bgmSource;
        [SerializeField] public AudioClip peacefulBGM;
        [SerializeField] public AudioClip horrorBGM;
        [SerializeField] public AudioClip sfx;

        [Header("Fade UI")]
        [SerializeField] public Image fadeImage;
        [SerializeField] public float fadeSpeed;

        private bool isHorror = false;

        private void Start()
        {
            if (fadeImage != null) fadeImage.color = new Color(0, 0, 0, 0);

            foreach (var obj in transitionObjects)
            {
                if (obj.target == null) continue;
                obj.initialPosition = obj.target.position;
                obj.initialRotation = obj.target.rotation;
            }
        }

       private void Update()
        {
            if (Input.GetKeyDown(KeyCode.G))
            {
                StartCoroutine(PlayTraumaTransition());
            }
        }

        public IEnumerator PlayTraumaTransition()
        {
            yield return FadeToBlack();

            isHorror = !isHorror;

            ToggleBackgrounds();
            UpdateLighting();
            UpdateSkybox();
            UpdateAudio();
            PlaySFX();
            ApplyObjectTransforms();

            yield return FadeFromBlack();
        }

        private void ToggleBackgrounds()
        {
            SetActiveObjects(peacefulBackgrounds, !isHorror);
            SetActiveObjects(horrorBackgrounds, isHorror);
        }

        private void UpdateLighting()
        {
            if (directionalLight == null) return;

            if (isHorror)
            {
                directionalLight.color = horrorLightColor;
                directionalLight.intensity = horrorLightIntensity;
            }
            else
            {
                directionalLight.color = peacefulLightColor;
                directionalLight.intensity = peacefulLightIntensity;
            }
        }

        private void UpdateSkybox()
        {
            if (peacefulSkybox == null || horrorSkybox == null) return;

            if (isHorror)
            {
                RenderSettings.skybox = horrorSkybox;
            }
            else
            {
                RenderSettings.skybox = peacefulSkybox;
            }
            DynamicGI.UpdateEnvironment();
        }

        private void UpdateAudio()
        {
            if (bgmSource == null) return;

            if (isHorror)
            {
                bgmSource.clip = horrorBGM;
            }
            else
            {
                bgmSource.clip = peacefulBGM;
            }

            bgmSource.loop = true;
            bgmSource.Play();
        }

        private void PlaySFX()
        {
            if (sfx != null)
            {
                AudioSource.PlayClipAtPoint(sfx, Camera.main.transform.position);
            }
        }

        private void SetActiveObjects(GameObject[] objects, bool isActive)
        {
            if (objects == null) return;

            foreach (var obj in objects)
            {
                if (obj != null) obj.SetActive(isActive);
            }
        }

        private IEnumerator FadeToBlack()
        {
            float t = 0f;
            while (t < 1f)
            {
                t += Time.deltaTime * fadeSpeed;
                if (fadeImage != null)
                    fadeImage.color = new Color(0, 0, 0, Mathf.Clamp01(t));
                yield return null;
            }
        }

        private IEnumerator FadeFromBlack()
        {
            float t = 1f;
            while (t > 0f)
            {
                t -= Time.deltaTime * fadeSpeed;
                if (fadeImage != null)
                    fadeImage.color = new Color(0, 0, 0, Mathf.Clamp01(t));
                yield return null;
            }
        }

        private void ApplyObjectTransforms()
        {
            if (transitionObjects == null) return;

            foreach (var obj in transitionObjects)
            {
                if (obj.target == null) continue;

                if (isHorror)
                {
                    obj.target.position = obj.initialPosition + obj.horrorOffset;
                    obj.target.rotation = Quaternion.Euler(obj.horrorRotation);
                }
                else
                {
                    obj.target.position = obj.initialPosition;
                    obj.target.rotation = obj.initialRotation;
                }
            }
        }
    }
}