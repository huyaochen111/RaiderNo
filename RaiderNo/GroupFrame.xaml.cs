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
    /// group.xaml 的交互逻辑
    /// </summary>
    public partial class GroupFrame : Window
    {
        List<dynamic> playerInfos, playerDungeons;
        public GroupFrame(List<string> playernames,List<long> playerDungeonsCount, List<string> playerBestDungeon, List<long> playerTotalLevels, List<dynamic> playerInfos,
            List<dynamic> playerDungeons)
        {
            InitializeComponent();

            this.playerInfos = playerInfos;
            this.playerDungeons = playerDungeons;

            Grid grid = this.FindName("Grid") as Grid;

            Label label = new Label
            {
                Content = "玩家-服务器",
                FontSize = 16,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                VerticalContentAlignment = VerticalAlignment.Center
            };
            Grid.SetColumn(label, 0);
            Grid.SetRow(label, 0);
            grid.Children.Add(label);
            
            try
            {
                for (int i = 0; i < 5; i++)
                {
                    Label la = new Label
                    {
                        Content = playernames[i],
                        FontSize = 16,
                        HorizontalContentAlignment = HorizontalAlignment.Center,
                        VerticalContentAlignment = VerticalAlignment.Center
                    };
                    Grid.SetColumn(la, 0);
                    Grid.SetRow(la, i + 1);
                    grid.Children.Add(la);
                }
            }
            catch(Exception)
            {

            }
            

            label = new Label
            {
                Content = "赛季大米次数",
                FontSize = 16,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                VerticalContentAlignment = VerticalAlignment.Center
            };
            Grid.SetColumn(label, 1);
            Grid.SetRow(label, 0);
            grid.Children.Add(label);

            try
            {
                for (int i = 0; i < 5; i++)
                {
                    Label la = new Label
                    {
                        Content = playerDungeonsCount[i],
                        FontSize = 16,
                        HorizontalContentAlignment = HorizontalAlignment.Center,
                        VerticalContentAlignment = VerticalAlignment.Center
                    };
                    Grid.SetColumn(la, 1);
                    Grid.SetRow(la, i + 1);
                    grid.Children.Add(la);
                }
            }
            catch(Exception)
            {

            }

            label = new Label
            {
                Content = "赛季最佳",
                FontSize = 16,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                VerticalContentAlignment = VerticalAlignment.Center
            };
            Grid.SetColumn(label, 2);
            Grid.SetRow(label, 0);
            grid.Children.Add(label);

            try
            {

                for (int i = 0; i < 5; i++)
                {
                    Label la = new Label
                    {
                        Content = playerBestDungeon[i],
                        FontSize = 16,
                        HorizontalContentAlignment = HorizontalAlignment.Center,
                        VerticalContentAlignment = VerticalAlignment.Center
                    };
                    Grid.SetColumn(la, 2);
                    Grid.SetRow(la, i + 1);
                    grid.Children.Add(la);
                }
            }
            catch(Exception)
            {

            }
            

            label = new Label
            {
                Content = "赛季评分",
                FontSize = 16,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                VerticalContentAlignment = VerticalAlignment.Center
            };
            Grid.SetColumn(label, 3);
            Grid.SetRow(label, 0);
            grid.Children.Add(label);

            try
            {
                for (int i = 0; i < 5; i++)
                {
                    Label la = new Label
                    {
                        Content = playerTotalLevels[i],
                        FontSize = 16,
                        HorizontalContentAlignment = HorizontalAlignment.Center,
                        VerticalContentAlignment = VerticalAlignment.Center
                    };
                    Grid.SetColumn(la, 3);
                    Grid.SetRow(la, i + 1);
                    grid.Children.Add(la);
                }
            }
            catch(Exception)
            {

            }
            

            label = new Label
            {
                Content = "详情",
                FontSize = 16,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                VerticalContentAlignment = VerticalAlignment.Center
            };
            Grid.SetColumn(label, 4);
            Grid.SetRow(label, 0);
            grid.Children.Add(label);

            try
            {
                for (int i = 0; i < 5; i++)
                {
                    Button bu = new Button();
                    bu.Click += Bu_Click;
                    bu.Content = "查看详情";
                    bu.FontSize = 16;
                    bu.Margin = new Thickness(10, 10, 10, 10);
                    bu.HorizontalContentAlignment = System.Windows.HorizontalAlignment.Center;
                    bu.VerticalContentAlignment = VerticalAlignment.Center;
                    bu.Tag = i;
                    if (playernames[i] == "未找到该玩家")
                    {
                        bu.IsEnabled = false;
                    }
                    Grid.SetColumn(bu, 4);
                    Grid.SetRow(bu, i + 1);
                    grid.Children.Add(bu);
                }
            }
            catch (Exception)
            {

            }
            
        }

        private void Bu_Click(object sender, RoutedEventArgs e)
        {
            int i = (int)((Button)sender).Tag;
            Console.WriteLine(i);

            Player playerForm = new Player(this.playerInfos[i], this.playerDungeons[i]);
            playerForm.ShowDialog();
        }
    }
}
