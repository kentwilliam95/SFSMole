using Sfs2X.Entities.Data;

public class Utility
{
    public const string CMD_JOINEDROOM = "CMD_JOINEDROOM";
    public const string CMD_HIT = "CMD_HIT";
    public const string CMD_GAMEEND = "CMD_GAMEEND";
    public const string CMD_USERJOINNED = "CMD_USERJOINNED";
    public const string CMD_USERLEAVE = "CMD_USERLEAVE";
    public const string CMD_GAMESTARTING = "CMD_GAMESTARTING";
    public const string CMD_GAMEUPDATE = "CMD_GAMEUPDATE";

    public struct RoomSetting
    {
        public int MaxPlayer;
        public string Name;
        public int ID;

        public int ScoreMultiplierPerHit;
    }

    public struct PlayerData
    {
        public int ID;
        public string Name;
        public int Score => HitCount * 100;
        public int HitCount;
        public int MissCount;
        public bool IsPlayer;

        
    }
}