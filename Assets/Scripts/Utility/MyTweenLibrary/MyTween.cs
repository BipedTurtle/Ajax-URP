using System.Collections.Generic;
using UnityEngine;

namespace Utility.MyTweenLibrary
{
    public class MyTween : MonoBehaviour
    {
        public static MyTween Instance { get; private set; }
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);

            if (Instance != null) {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }

        #region Various Tweening States
        // rotation
        private static int rotationStatesCount = 50;
        private Queue<MyTweenRotationState> rotationStatesReady = new Queue<MyTweenRotationState>(rotationStatesCount);
        private List<MyTweenRotationState> tweeningRotationStates = new List<MyTweenRotationState>(rotationStatesCount);

        // movement
        private static int moveStatesCount = 50;
        private Queue<MyTweenMoveState> moveStatesReady = new Queue<MyTweenMoveState>(moveStatesCount);
        private List<MyTweenMoveState> tweeningMoveStates = new List<MyTweenMoveState>(moveStatesCount);
        #endregion
        private void Start()
        {
            for (int i = 0; i < rotationStatesCount; i++)
                this.rotationStatesReady.Enqueue(new MyTweenRotationState());
        }


        public MyTweenState Rotate(Transform tweeningTransform, Vector3 targetDirection, float time, float wait = 0)
        {
            if (this.rotationStatesReady.Count == 0)
                this.rotationStatesReady.Enqueue(new MyTweenRotationState());

            var rotationState = this.rotationStatesReady.Dequeue();
            rotationState.Init(tweeningTransform, targetDirection, time, wait);
            this.tweeningRotationStates.Add(rotationState);

            return rotationState;
        }


        public MyTweenState Move(Transform tweeningTransform, Vector3 to, float time, float wait = 0)
        {
            if (this.moveStatesReady.Count == 0)
                this.moveStatesReady.Enqueue(new MyTweenMoveState());

            var moveState = this.moveStatesReady.Dequeue();
            moveState.Init(tweeningTransform, to, time, wait);
            this.tweeningMoveStates.Add(moveState);

            return moveState;
        }

        
        public void Cancel(MyTweenState state)
        {
            switch (state)
            {
                case MyTweenRotationState r:
                    this.rotationStatesReady.Enqueue(r);
                    this.tweeningRotationStates.Remove(r);
                    break;
                case MyTweenMoveState m:
                    this.moveStatesReady.Enqueue(m);
                    this.tweeningMoveStates.Remove(m);
                    break;
                default:
                    break;
            }
        }


        private void Update()
        {
            this.UpdateTweening<MyTweenRotationState>(this.tweeningRotationStates, this.rotationStatesReady);
            this.UpdateTweening<MyTweenMoveState>(this.tweeningMoveStates, this.moveStatesReady);
        }


        private void UpdateTweening<T>(List<T> tweeningStates, Queue<T> tweenStatesReady) where T : MyTweenState
        {
            for (int i = 0; i < tweeningStates.Count; i++) {
                var state = tweeningStates[i];

                bool tweeningFinished = state.Tween();
                if (tweeningFinished) {
                    tweeningStates.Remove(state);
                    tweenStatesReady.Enqueue(state); } }
        }
    }
}
