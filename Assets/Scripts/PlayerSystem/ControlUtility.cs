using Entities;
using Entities.EnemySystem;
using UnityEngine;
using UnityEngine.Assertions.Must;
using Utility;

namespace PlayerSystem
{
    public static class ControlUtility
    {
        private static Camera _mainCamera;
        private static Camera MainCamera
        {
            get
            {
                if (_mainCamera == null)
                    _mainCamera = Camera.main;

                return _mainCamera;
            }
        }
        private static float proximityTreshold = 8f;
        public static Transform GetClosestEnemyFromCursor() 
        {
            var cursorWorldPoint = MainCamera.ScreenToWorldPoint(Input.mousePosition).Set(y: .5f);

            Enemy closestEnemy = null;

            float closestEnemySqrDistance = 0;
            var enemiesAlive = InteractionChart.Instance.EnemiesAlive;
            for (int i = 0; i < enemiesAlive.Count; i++) {
                var enemy = enemiesAlive[i];
                float toEnemySqrDistance = (enemy.transform.localPosition - cursorWorldPoint).sqrMagnitude;
                bool withinDistance = toEnemySqrDistance <= proximityTreshold * proximityTreshold;

                if (!withinDistance)
                    continue;

                if (closestEnemy == null) {
                    closestEnemy = enemy;
                    closestEnemySqrDistance = toEnemySqrDistance;
                    continue;
                }

                if (toEnemySqrDistance < closestEnemySqrDistance) {
                    closestEnemy = enemy;
                    closestEnemySqrDistance = toEnemySqrDistance;
                }
            }

            return (closestEnemy == null) ? null : closestEnemy.transform;
        }


        public static Vector3 GetMousePositionWorld()
        {
            float zDepth = MainCamera.transform.localPosition.y;
            return MainCamera.ScreenToWorldPoint(Input.mousePosition.Set(z: zDepth));
        }


        public static Vector3 GetPlayerToCursorVector()
        {
            var mousePositionWorld = GetMousePositionWorld();
            var playerPos = PlayerController.Instance.transform.localPosition;
            var toCursorVector = (mousePositionWorld - playerPos).Set(y: 0);
            return toCursorVector;
        }
    }

}