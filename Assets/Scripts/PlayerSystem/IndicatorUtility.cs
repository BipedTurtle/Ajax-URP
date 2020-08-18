using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace PlayerSystem
{
    public enum IndicatorSpawnType
    {
        Player,
        Cursor
    }


    public static class IndicatorUtility
    {
        public static Vector3 GetSpawnPos(IndicatorSpawnType indicatorSpawnType)
        {
            switch (indicatorSpawnType)
            {
                case IndicatorSpawnType.Player:
                default:
                    return PlayerController.Instance.transform.localPosition;
                case IndicatorSpawnType.Cursor:
                    return ControlUtility.GetMousePositionWorld();
            }
        }
    }
}
