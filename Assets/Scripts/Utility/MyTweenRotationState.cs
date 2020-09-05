using UnityEngine;

namespace Utility
{
    public class MyTweenRotationState : MyTweenState
    {
        private Transform rotatingTransform;
        private Quaternion currentRotation;
        private Quaternion targetRotation;
        private float timeOfRotation;

        public void Init(Transform rotatingTransform, Vector3 targetDireciton, float time)
        {
            this.rotatingTransform = rotatingTransform;
            this.currentRotation = rotatingTransform.rotation;

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
            rotatingTransform.rotation = Quaternion.Lerp(currentRotation, targetRotation, progress);

            bool tweeningFinished = progress >= 1f;
            base.IsTweening = !tweeningFinished;

            return tweeningFinished;
        }

    }
}
