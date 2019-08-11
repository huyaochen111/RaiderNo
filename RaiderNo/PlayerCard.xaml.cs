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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RaiderNo
{
    /// <summary>
    /// PlayerCard.xaml 的交互逻辑
    /// </summary>
    public partial class PlayerCard : UserControl
    {
        private Player player;
        public PlayerCard(Player player)
        {
            InitializeComponent();

            this.player = player;
            if (player.isValid)
            {
                nameLabel.Content = player.name + "-" + player.realm;
                scoreLabel.Content = "赛季评分 " + player.score;
                dungeonBestLabel.Content = "最佳 " + player.bestDungeon;
                dungeonCountLabel.Content = "完成 " + player.currentSeasonDungeons.Count + "次";
            }
            else
            {
                nameLabel.Content = player.error;
                scoreLabel.Content = "";
                dungeonBestLabel.Content = "";
                dungeonCountLabel.Content = "";
                details.IsEnabled = false;
            }
        }

        private void Details_Click(object sender, RoutedEventArgs e)
        {
            PlayerWindow playerWindow = new PlayerWindow(player);
            playerWindow.ShowDialog();
        }
    }
}
