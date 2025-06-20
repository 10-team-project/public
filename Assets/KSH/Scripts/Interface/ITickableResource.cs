using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KSH
{
    public interface ITickableResource
    {
        float DecayTime { get; } //줄어들 시간
        float ResourceAmount { get; } //자원이 변화할 양
        bool Decaying { get; } //true, false 조절 가능
        void ResourceTick(); // 시간에 따라 수치 변화 기능
    }
}

