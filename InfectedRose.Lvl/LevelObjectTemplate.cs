using System;
using System.Numerics;
using InfectedRose.Core;
using RakDotNet.IO;

namespace InfectedRose.Lvl
{
    public class LevelObjectTemplate : IConstruct
    {
        public ulong ObjectId { get; set; }
        
        public int Lot { get; set; }
        
        public uint AssetType { get; set; }
        
        public uint UnknownInt { get; set; }
        
        public uint UnknownInt1 { get; set; }

        public Vector3 Position { get; set; }
        
        public Quaternion Rotation { get; set; }
        
        public float Scale { get; set; }

        public LegoDataDictionary LegoInfo { get; set; }

        public uint LvlVersion { get; set; }
        
        public LevelObjectTemplate(uint lvlVersion = 0x26)
        {
            LvlVersion = lvlVersion;
        }
        
        public void Serialize(BitWriter writer)
        {
            writer.Write(ObjectId);

            writer.Write(Lot);

            if (LvlVersion >= 0x26)
                writer.Write(AssetType);

            if (LvlVersion >= 0x20)
                writer.Write(UnknownInt);

            writer.Write(Position);

            writer.WriteNiQuaternion(Rotation);

            writer.Write(Scale);

            writer.WriteNiString(LegoInfo.ToString("\n"), true);

            if (LvlVersion >= 0x7)
                writer.Write(UnknownInt1);
        }

        public void Deserialize(BitReader reader)
        {
            ObjectId = reader.Read<ulong>();

            Lot = reader.Read<int>();
            
            if (LvlVersion >= 0x26)
                AssetType = reader.Read<uint>();

            if (LvlVersion >= 0x20)
                UnknownInt = reader.Read<uint>();

            Position = reader.Read<Vector3>();

            Rotation = reader.ReadNiQuaternion();

            Scale = reader.Read<float>();

            var legoInfo = reader.ReadNiString(true);

            if (legoInfo.Length > 0)
            {
                try
                {
                    LegoInfo = LegoDataDictionary.FromString(legoInfo);
                }
                catch (Exception e)
                {
                    Console.WriteLine(legoInfo);
                    
                    Console.WriteLine(e);
                    throw;
                }
            }

            if (LvlVersion >= 0x7)
                UnknownInt1 = reader.Read<uint>();
        }
    }
}