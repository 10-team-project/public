using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KSH
{
    public interface IDecayable
    {
        float DecayRate { get; } //줄어들 양
        bool Decaying {get;}
        void TimeDecay(); // 시간에 따라 양이 줄어드는 기능
    }
}

