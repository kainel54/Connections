using System;
using System.Collections.Generic;
using UnityEngine;

namespace IH.Level
{
    [Serializable]
    public struct RoomPair
    {
        public LevelTypeEnum levelTypeEnum;
        public List<LevelRoom> levelRooms;
    }
    
    [CreateAssetMenu(fileName = "StageData", menuName = "SO/StageData")]
    public class StageDataSO : ScriptableObject
    {
        public SoundSO DefaultSoundSo;
        
        public int roomCount;
        public int specialRoomCount;
        public int rewardRoomCount;
        public List<RoomPair> roomPairs;
    }
}