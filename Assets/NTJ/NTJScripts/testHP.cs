using UnityEngine;

namespace NTJ
{
    public class testHP : MonoBehaviour
    {
        // HP�� �߰�
        public float CurrentHP
        {
            get => 1.0f;// resource.Cur;
            set {} // =>  resource.Cur = value;
        }


        // Hunger�� �߰�

        public void SetHunger(float value)
        {
            //resourceDegenerator.Resource.Cur = value;
        }

        // Thirst�� �߰�
        public void SetThirst(float value)
        {
            //resourceDegenerator.Resource.Cur = value;
        }

        // Fatigue�� �߰�
        public void SetFatigue(float value)
        {
            //resourceDegenerator.Resource.Cur = value;
        }
        //   // HP�� �߰�
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
        //   // Hunger�� �߰�
        //
        //   public void SetHunger(float value)
        //   {
        //       resourceDegenerator.Resource.Cur = value;
        //   }
        //
        //   // Thirst�� �߰�
        //   public void SetThirst(float value)
        //   {
        //       resourceDegenerator.Resource.Cur = value;
        //   }
        //
        //   // Fatigue�� �߰�
        //   public void SetFatigue(float value)
        //   {
        //       resourceDegenerator.Resource.Cur = value;
        //   }
    }
}
