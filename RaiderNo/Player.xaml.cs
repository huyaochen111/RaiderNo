using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace RaiderNo
{
    /// <summary>
    /// Player.xaml 的交互逻辑
    /// </summary>
    public partial class Player : Window
    {
        public Player(dynamic playerInfo, List<dynamic> dungeons)
        {
            InitializeComponent();

            this.Title = playerInfo.character.name + " - " + playerInfo.character.realm;

            Label dungeonsCountLabel = this.FindName("dungeonsCount") as Label;
            Label bestDungeonLabel = this.FindName("bestDungeon") as Label;
            Label totalLevelsLabel = this.FindName("totalLevels") as Label;
            Label latestDungeonsLabel = this.FindName("latestDungeons") as Label;

            string bestDungeonName = "本赛季尚未限时通关";
            long bestDungeonLevel = 0;
            long bestDungeonReward = 0;
            long dungeonsCount = 0;
            Dictionary<string, long> dungeonsBestLevel = new Dictionary<string, long>();
            long totalLevels = 0;
            List<string> latestDungeons = new List<string>();

            foreach (dynamic dungeon in dungeons)
            {
                if (dungeon.affixes.Last.Value == "迷醉")
                {
                    if (latestDungeons.Count < 10)
                    {
                        latestDungeons.Add(dungeon.dungeonName.Value + "(" + dungeon.dungeonLevel.Value + ", +" + dungeon.reward + ")");
                    }
                    if (dungeon.reward.Value > 0)
                    {
                        if (dungeon.dungeonLevel.Value > bestDungeonLevel || (dungeon.dungeonLevel.Value == bestDungeonLevel && dungeon.reward.Value > bestDungeonReward))
                        {
                            bestDungeonLevel = dungeon.dungeonLevel.Value;
                            bestDungeonName = dungeon.dungeonName.Value;
                            bestDungeonReward = dungeon.reward;
                        }
                        if (!dungeonsBestLevel.ContainsKey(dungeon.dungeonName.Value))
                        {
                            dungeonsBestLevel.Add(dungeon.dungeonName.Value, 0);
                        }
                        if (dungeon.dungeonLevel.Value > dungeonsBestLevel[dungeon.dungeonName.Value])
                        {
                            totalLevels += (dungeon.dungeonLevel.Value - dungeonsBestLevel[dungeon.dungeonName.Value]);
                            dungeonsBestLevel[dungeon.dungeonName.Value] = dungeon.dungeonLevel.Value;
                        }
                    }
                    dungeonsCount += 1;
                }
            }
            dungeonsCountLabel.Content = dungeonsCount;
            bestDungeonLabel.Content = bestDungeonName + "(" + bestDungeonLevel + ", +" + bestDungeonReward + ")";
            totalLevelsLabel.Content = totalLevels;
            latestDungeonsLabel.Content = string.Join("\n", latestDungeons);
        }
    }
}
