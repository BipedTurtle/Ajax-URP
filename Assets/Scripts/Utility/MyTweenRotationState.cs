using System.Collections.Generic;
using UnityEngine;

namespace Utility
{
    public class MyTweenRotationState : MyTweenState
    {
        public bool IsTweening { get; private set; }

        private Transform rotatingTransform;
        private Quaternion targetRotation;
        private float timeOfRotation;

        public void Init(Transform rotatingTransform, Vector3 targetDireciton, float time)
        {
            this.rotatingTransform = rotatingTransform;
            this.targetRotation = Quaternion.LookRotation(targetDireciton);
            this.timeOfRotation = time;

            this.timeProgression = 0;
        }


        private float timeProgression;
        public bool Rotate()
        {
            timeProgression += Time.deltaTime;
            timeProgression = Mathf.Clamp(timeProgression, 0, timeOfRotation);
            var progress = (1f / timeOfRotation) * timeProgression;
            //Debug.Log(progress);
            rotatingTransform.rotation = Quaternion.Lerp(rotatingTransform.rotation, targetRotation, progress);

            bool tweeningFinished = progress >= 1f;
            this.IsTweening = !tweeningFinished;
            return tweeningFinished;
        }

    }
}
