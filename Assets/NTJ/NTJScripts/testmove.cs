using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NTJ
{
    public class testmove : MonoBehaviour
    {
        public float speed = 5f;

        void Update()
        {
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");

            Vector3 move = new Vector3(h, 0, v) * speed * Time.deltaTime;
            transform.Translate(move, Space.World);
        }
    }
}