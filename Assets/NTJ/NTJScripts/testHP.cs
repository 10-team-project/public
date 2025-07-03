using UnityEngine;

namespace NTJ
{
    public class testHP : MonoBehaviour
    {
        // HP에 추가
        public float CurrentHP
        {
            get => 1.0f;// resource.Cur;
            set {} // =>  resource.Cur = value;
        }


        // Hunger에 추가

        public void SetHunger(float value)
        {
            //resourceDegenerator.Resource.Cur = value;
        }

        // Thirst에 추가
        public void SetThirst(float value)
        {
            //resourceDegenerator.Resource.Cur = value;
        }

        // Fatigue에 추가
        public void SetFatigue(float value)
        {
            //resourceDegenerator.Resource.Cur = value;
        }
        //   // HP에 추가
        //   public float CurrentHP
        //   {
        //       get => resource.Cur;
        //       set => resource.Cur = value;
        //   }
        // public float MaxHP
        // {
        //     get => resource.Max;
        // }
        //
        //
        //   // Hunger에 추가
        //
        //   public void SetHunger(float value)
        //   {
        //       resourceDegenerator.Resource.Cur = value;
        //   }
        //
        //   // Thirst에 추가
        //   public void SetThirst(float value)
        //   {
        //       resourceDegenerator.Resource.Cur = value;
        //   }
        //
        //   // Fatigue에 추가
        //   public void SetFatigue(float value)
        //   {
        //       resourceDegenerator.Resource.Cur = value;
        //   }
    }
}
