using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Scripts
{
    public class Test : MonoBehaviour
    {
        private float distanceToMove = 5f;
        private float distanceMoved;
        private void Update()
        {
            if (distanceMoved > distanceToMove)
            {
                Debug.Log($"center after movement: {GetComponent<Collider>().bounds.center}");
                return;
            }

            var movment = this.distanceToMove * Time.deltaTime;
            transform.localPosition += Vector3.right * movment;
            distanceMoved += movment;
        }
    }
}
