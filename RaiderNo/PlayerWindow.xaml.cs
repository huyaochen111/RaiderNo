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
    public partial class PlayerWindow : Window
    {
        public PlayerWindow(Player player)
        {
            InitializeComponent();

            this.Title = player.name + " - " + player.realm;
            bestDungeonLabel.Content = player.bestDungeon;
            dungeonsCountLabel.Content = player.currentSeasonDungeons.Count;
            totalLevelsLabel.Content = player.score;
            latestDungeonsLabel.Content = "";

            for (int i = 0; i < Math.Min(player.currentSeasonDungeons.Count, 10); i++)
            {
                if (i > 0)
                {
                    latestDungeonsLabel.Content = latestDungeonsLabel.Content + "\n";
                }
                latestDungeonsLabel.Content = latestDungeonsLabel.Content + player.currentSeasonDungeons[i].toString();
            }
        }
    }
}
