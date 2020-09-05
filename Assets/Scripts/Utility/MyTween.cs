using System.Collections.Generic;
using UnityEngine;

namespace Utility
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

        private static int rotationStatesCount = 50;
        private Queue<MyTweenRotationState> rotationStatesReady = new Queue<MyTweenRotationState>(rotationStatesCount);
        private List<MyTweenRotationState> tweeningRotationStates = new List<MyTweenRotationState>(rotationStatesCount);
        private void Start()
        {
            for (int i = 0; i < rotationStatesCount; i++)
                this.rotationStatesReady.Enqueue(new MyTweenRotationState());
        }


        public MyTweenState Rotate(Transform tweeningTransform, Vector3 targetDirection, float time)
        {
            if (this.rotationStatesReady.Count == 0)
                this.rotationStatesReady.Enqueue(new MyTweenRotationState());

            var rotationState = this.rotationStatesReady.Dequeue();
            rotationState.Init(tweeningTransform, targetDirection, time);
            this.tweeningRotationStates.Add(rotationState);

            return rotationState;
        }

        
        public void Cancel(MyTweenState state)
        {
            switch (state)
            {
                case MyTweenRotationState r:
                    this.rotationStatesReady.Enqueue(r);
                    this.tweeningRotationStates.Remove(r);
                    break;
                default:
                    break;
            }
        }


        private void Update()
        {
            for (int i = 0; i < this.tweeningRotationStates.Count; i++) {
                var state = tweeningRotationStates[i];

                bool tweeningFinished = state.Rotate();
                if (tweeningFinished) {
                    this.tweeningRotationStates.Remove(state);
                    this.rotationStatesReady.Enqueue(state); } }

        }

    }
}
