using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RaiderNo
{
    public class Dungeon
    {
        public string name;
        public long level;
        public long reward;
        public List<string> affixes = new List<string>();

        public Dungeon(dynamic dungeon)
        {
            name = dungeon.dungeonName;
            level = dungeon.dungeonLevel;
            reward = dungeon.reward;
            foreach (dynamic affix in dungeon.affixes)
            {
                affixes.Add(affix.Value);
            }
        }

        public string toString()
        {
            return name + "(" + level + ", +" + reward + ")";
        }
    }
    public class Player
    {
        // 如果用户创建不成功，错误保存于此
        public Boolean isValid = false;
        public string error = "未知错误";

        // 用户信息
        public string name;
        public string realm;
        public string id;
        public long score;
        public string bestDungeon;
        public List<Dungeon> currentSeasonDungeons = new List<Dungeon>();

        public Player(string playerFullname)
        {
            string[] segs = playerFullname.Split('-');
            if (segs.Length != 2)
            {
                error = "名称格式错误";
            }
            try
            {
                // 获取数据
                name = segs[0];
                realm = segs[1];
                dynamic player = Utility.GetUserInfo(name, realm);
                id = player.character.id.Value;
                List<dynamic> dungeons;
                if (id.IndexOf("@temp") >= 0)
                {
                    dungeons = Utility.GetUserDungeonsByName(player.character.playerId.Value, name, realm);
                }
                else
                {
                    dungeons = Utility.GetUserDungeons(id.Replace("@temp", ""));
                }
                foreach (dynamic dungeon in dungeons)
                {
                    Dungeon dung = new Dungeon(dungeon);
                    if (dung.affixes.Count == 4 && dung.affixes[3] == "迷醉")
                    {
                        currentSeasonDungeons.Add(new Dungeon(dungeon));
                    }
                }
                // 评估
                string bestDungeonName = "本赛季尚未限时通关";
                long bestDungeonLevel = 0;
                long bestDungeonReward = 0;
                Dictionary<string, long> dungeonsBestLevel = new Dictionary<string, long>();
                foreach (Dungeon dungeon in currentSeasonDungeons)
                {
                    if (dungeon.reward > 0)
                    {
                        if (dungeon.level > bestDungeonLevel || (dungeon.level == bestDungeonLevel && dungeon.reward > bestDungeonReward))
                        {
                            bestDungeonLevel = dungeon.level;
                            bestDungeonName = dungeon.name;
                            bestDungeonReward = dungeon.reward;
                        }
                        if (!dungeonsBestLevel.ContainsKey(dungeon.name))
                        {
                            dungeonsBestLevel.Add(dungeon.name, 0);
                        }
                        if (dungeon.level > dungeonsBestLevel[dungeon.name])
                        {
                            score += (dungeon.level - dungeonsBestLevel[dungeon.name]);
                            dungeonsBestLevel[dungeon.name] = dungeon.level;
                        }
                    }
                }
                if (bestDungeonLevel > 0)
                {
                    bestDungeon = bestDungeonName + "(" + bestDungeonLevel + ", +" + bestDungeonReward + ")";
                }
                else
                {
                    bestDungeon = bestDungeonName;
                }

                isValid = true;
            }
            catch
            {
                error = "未找到该玩家";
            }
        }
    }
}
