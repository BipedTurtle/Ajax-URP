using UnityEngine;

namespace Utility.MyTweenLibrary
{
    public class MyTweenMoveState : MyTweenState
    {
        private Transform moverTransform;
        private Vector3 from;
        private Vector3 to;
        private float time;
        private float wait;
        public void Init(Transform moverTransform, Vector3 to, float time, float wait)
        {
            this.moverTransform = moverTransform;
            this.from = moverTransform.localPosition;
            this.to = to;
            this.time = (time == 0) ? 1f : time;
            this.wait = wait;

            this.timeProgress = 0;
            this.timeWaited = 0;
        }


        private float timeProgress;
        private float timeWaited;
        public override bool Tween()
        {
            this.timeWaited += Time.deltaTime;
            if (timeWaited < this.wait)
                return false;

            this.timeProgress += Time.deltaTime;
            float progress = (1f / this.time) * Mathf.Clamp(this.timeProgress, 0, this.time);

            this.moverTransform.localPosition = Vector3.Lerp(this.from, this.to, progress);

            bool movementFinished = progress >= 1f;
            base.IsTweening = !movementFinished;
            return movementFinished;
        }
    }
}
