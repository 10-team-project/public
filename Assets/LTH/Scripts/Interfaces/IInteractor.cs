using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LTH
{
    public interface IInteractor
    {
        GameObject GetGameObject();
        public void TakeDamage(int amount);
    }

}
