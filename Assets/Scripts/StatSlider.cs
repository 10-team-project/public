using UnityEngine;
using UnityEngine.UI;
using SHG;
using Character = SHG.TempCharacter;

public class StatSlider : MonoBehaviour
{

  [SerializeField] Character.Stat statType;
  
  void OnEnable()
  {
    Slider slider = this.GetComponent<Slider>();
    switch (this.statType)
    {
      case Character.Stat.Hp:
        App.Instance.PlayerStatManager.HP.HpBarSlider = slider;
        break;
      case Character.Stat.Fatigue:
        App.Instance.PlayerStatManager.Fatigue.FatigueSlider = slider;
        break;
      case Character.Stat.Hunger:
        App.Instance.PlayerStatManager.Hunger.HungerSlider = slider;
        break;
      case Character.Stat.Hydration:
        App.Instance.PlayerStatManager.Thirsty.ThirstySlider = slider;
        break;
    }
  }

  void OnDisable()
  {

    switch (this.statType)
    {
      case Character.Stat.Hp:
        App.Instance.PlayerStatManager.HP.HpBarSlider = null;
        break;
      case Character.Stat.Fatigue:
        App.Instance.PlayerStatManager.Fatigue.FatigueSlider = null;
        break;
      case Character.Stat.Hunger:
        App.Instance.PlayerStatManager.Hunger.HungerSlider = null;
        break;
      case Character.Stat.Hydration:
        App.Instance.PlayerStatManager.Thirsty.ThirstySlider = null;
        break;
    }
  }
}
