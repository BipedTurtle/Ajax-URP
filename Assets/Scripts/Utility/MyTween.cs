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

        private Queue<MyTweenRotationState> rotationStatesReady = new Queue<MyTweenRotationState>(20);
        private List<MyTweenRotationState> tweeningRotationStates = new List<MyTweenRotationState>();
        private void Start()
        {
            for (int i = 0; i < 20; i++)
                this.rotationStatesReady.Enqueue(new MyTweenRotationState());
        }


        public MyTweenState Rotate(Transform tweeningTransform, Vector3 targetDirection, float time)
        {
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
