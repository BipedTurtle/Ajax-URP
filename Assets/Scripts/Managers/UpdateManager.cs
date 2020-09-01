using System;
using System.Collections.Generic;
using UnityEngine;

namespace Managers
{

    public class UpdateManager : MonoBehaviour
    {
        public static UpdateManager Instance { get; private set; }

        private void Awake()
        {
            #region Singleton
            if (Instance != null) {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            #endregion
        }


        private LinkedList<Action> currentGlobalUpdateSubscribers = new LinkedList<Action>();
        private List<Action> globalUpdateToBeAdded = new List<Action>(50);
        private List<Action> globalUpdateToBeRemoved = new List<Action>(50);
        private void Update()
        {
            if (globalUpdateToBeAdded.Count > 0)
            {
                var additionalEnumerator = globalUpdateToBeAdded.GetEnumerator();
                while (additionalEnumerator.MoveNext())
                    currentGlobalUpdateSubscribers.AddLast(additionalEnumerator.Current);

                globalUpdateToBeAdded.Clear();
            }

            var enumerator = currentGlobalUpdateSubscribers.GetEnumerator();
            while (enumerator.MoveNext())
                enumerator.Current?.Invoke();

            if (globalUpdateToBeRemoved.Count > 0) {
                var removeEnumerator = this.globalUpdateToBeRemoved.GetEnumerator();
                while (removeEnumerator.MoveNext())
                    this.currentGlobalUpdateSubscribers.Remove(removeEnumerator.Current);
            }

            globalUpdateToBeRemoved.Clear();
        }

        public void SubscribeToGlobalUpdate(Action action)
            => globalUpdateToBeAdded.Add(action);

        public void UnSubscribeFromGlobalUpdate(Action action)
            => this.globalUpdateToBeRemoved.Add(action);


        private LinkedList<Action> currentGlobalFixedUpdateSubscribers = new LinkedList<Action>();
        private List<Action> globalFixedUpdateToBeAdded = new List<Action>(50);
        private List<Action> globalFixedUPdateToBeRemoved = new List<Action>(50);
        private void FixedUpdate()
        {
            if (globalFixedUpdateToBeAdded.Count > 0)
            {
                var addEnumerator = globalFixedUpdateToBeAdded.GetEnumerator();
                while (addEnumerator.MoveNext())
                    currentGlobalFixedUpdateSubscribers.AddLast(addEnumerator.Current);

                globalFixedUpdateToBeAdded.Clear();
            }

            var enumerator = currentGlobalFixedUpdateSubscribers.GetEnumerator();
            while (enumerator.MoveNext())
                enumerator.Current?.Invoke();

            if (globalFixedUPdateToBeRemoved.Count > 0) {
                var removeEnumerator = globalFixedUPdateToBeRemoved.GetEnumerator();
                while (removeEnumerator.MoveNext())
                    currentGlobalFixedUpdateSubscribers.Remove(removeEnumerator.Current);
            }

            globalFixedUPdateToBeRemoved.Clear();
        }

        public void SubscribeToGlobalFixedUpdate(Action action)
            => this.globalFixedUpdateToBeAdded.Add(action);

        public void UnsubscribeFromGlobalFixedUpdate(Action action)
            => this.globalFixedUPdateToBeRemoved.Add(action);
    }
}

