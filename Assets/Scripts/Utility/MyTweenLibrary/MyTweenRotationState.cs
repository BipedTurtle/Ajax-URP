using UnityEngine;

namespace Utility.MyTweenLibrary
{
    public class MyTweenRotationState : MyTweenState
    {
        private Transform rotatingTransform;
        private Quaternion currentRotation;
        private Quaternion targetRotation;
        private float timeOfRotation;
        private float wait;

        public void Init(Transform rotatingTransform, Vector3 targetDireciton, float time, float wait)
        {
            this.rotatingTransform = rotatingTransform;
            this.currentRotation = rotatingTransform.rotation;

            this.targetRotation = Quaternion.LookRotation(targetDireciton);
            this.timeOfRotation = time;
            this.wait = wait;

            this.timeProgression = 0;
            this.timeWaited = 0;
        }


        private float timeProgression;
        private float timeWaited;
        public override bool Tween()
        {
            this.timeWaited += Time.deltaTime;
            if (timeWaited < this.wait)
                return false;

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
