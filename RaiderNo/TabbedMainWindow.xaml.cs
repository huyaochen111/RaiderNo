using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
    /// TabbedMainWindow.xaml 的交互逻辑
    /// </summary>
    
    public partial class TabbedMainWindow : Window
    {
        //delegate Player GetPlayerMethod();
        //readonly GetPlayerMethod getPlayerMethod = new GetPlayerMethod(GetPlayer);

        Boolean isGetting = false;
        Boolean isStopping = false;

        public TabbedMainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (isGetting)
            {
                isStopping = true;
                button.Content = "停止中...";
                button.IsEnabled = false;
            }
            else
            {
                stackPanel.Children.Clear();
                string[] segments = inputBox.Text.Split('\n');
                if (segments.Length > 0)
                {
                    button.Content = "停止";
                    isGetting = true;
                    new Thread(() =>
                    {
                        for (int i = 0; i < segments.Length; i++)
                        {
                            Player player = new Player(segments[i]);
                            this.Dispatcher.BeginInvoke(new Action(() =>
                            {
                                stackPanel.Children.Add(new PlayerCard(player));
                            }));
                            if (isStopping)
                            {
                                break;
                            }
                        }
                        this.Dispatcher.BeginInvoke(new Action(() =>
                        {
                            button.Content = "查询";
                            button.IsEnabled = true;
                            isGetting = false;
                            isStopping = false;
                        }));
                    }).Start();
                    
                }
            }
        }

        //static private Player GetPlayer()
        //{
        //    string playerFullname = playerFullnames.Dequeue();

        //    Dispatcher.BeginInvoke(new Action(() => tb.Text = Num));
        //    return new Player(playerFullname);
        //}

        //private void GotPlayer(IAsyncResult result)
        //{
        //    Player player = getPlayerMethod.EndInvoke(result);
        //    stackPanel.Children.Add(new PlayerCard(player));
        //}
    }
}
