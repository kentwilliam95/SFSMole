using System.Collections.Generic;
using Sfs2X.Entities.Data;
using UnityEngine;

public class Utility
{
    public const string CMD_LATENCY = "CMD_LATENCY";
    public const string CMD_PING = "CMD_PING";
    public const string CMD_PONG = "CMD_PONG";

    public const string CMD_JOINEDROOM = "CMD_JOINEDROOM";
    public const string CMD_HIT = "CMD_HIT";
    
    public const string CMD_USERJOINNED = "CMD_USERJOINNED";
    public const string CMD_USERLEAVE = "CMD_USERLEAVE";
    public const string CMD_GAMESTARTING = "CMD_GAMESTARTING";
    
    public const string CMD_GAMEREADY = "CMD_GAMEREADY";
    public const string CMD_STARTGAME = "CMD_STARTGAME";
    public const string CMD_GAMESETTINGRECEIVED = "CMD_GAMESETTINGRECEIVED";
    public const string CMD_GAMEEND = "CMD_GAMEEND";

    public struct RoomSetting
    {
        public int MaxPlayer;
        public string Name;
        public int ID;

        public int ScoreMultiplierPerHit;
    }

    public class GameSetting
    {
        public int ID;
        public string Name;
        public int Score => HitCount * 100;
        public int HitCount;
        
        public List<int> SpawnSequence;
        public int SequenceIndex { get; private set; }

        public int MissCount;
        public bool IsPlayer;
        
        public void IncreaseHitCount()
        {
            HitCount++;
        }

        public void AddSpawnSequence(int index)
        {
            if (SpawnSequence == null)
            {
                SpawnSequence = new List<int>(16);
            }
            SpawnSequence.Add(index);
        }

        public int GetSpawnSequence()
        {
            if (SequenceIndex >= SpawnSequence.Count)
            {
                return int.MinValue;
            }

            return SpawnSequence[SequenceIndex++];
        }
    }
}