using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Patterns;
using KSH;

namespace KSH
{
    public class PlayerStatManager : SingletonBehaviour<PlayerStatManager> //싱글톤
    {
        [SerializeField] private HP hp;
        [SerializeField] private Hunger hunger;
        [SerializeField] private Thirsty thirsty;
        [SerializeField] private Fatigue fatigue;
        [SerializeField] private Canvas UI;
    
        public HP HP => hp;
        public Hunger Hunger => hunger;
        public Thirsty Thirsty => thirsty;
        public Fatigue Fatigue => fatigue;

        protected override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(gameObject);
        }

        public void ShowUI()
        {
          this.UI.enabled = true;
        }

        public void HideUI()
        {
          this.UI.enabled = false;
        }
    }   
}
