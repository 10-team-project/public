using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KSH;

namespace KSH
{
    public class HungerItem : MonoBehaviour, KSH.IUsable //임시 아이템
    {
        private Hunger hunger;
        [SerializeField] private float amount;
        void Start()
        {
            hunger = PlayerStatManager.Instance.Hunger;
        }
    
        public void Use()
        {
            hunger.Eat(amount);
            Debug.Log("배가차요");
        }
    }
}
