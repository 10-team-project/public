using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSprite : MonoBehaviour
{
    private Image characterImage;

    private void Start()
    {
        characterImage = GetComponent<Image>();
    }

    public void ChangeSprite(Sprite sprite) //이미지 변경
    {
        characterImage.sprite = sprite;
    }
}
