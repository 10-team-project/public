using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KSH
{
    public interface IResource
    {
        float Max { get; }//최대 자원 양
        float Cur { get; } //현재 자원 양

        void Decrease(float amount); //자원 감소
        void Increase(float amount); //자원 증가
    }
}
