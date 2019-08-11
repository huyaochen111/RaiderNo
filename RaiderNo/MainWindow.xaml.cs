using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e) // 查人
        {
            TextBox nameBox = this.FindName("nameBox") as TextBox;
            TextBox realmBox = this.FindName("realmBox") as TextBox;

            try
            {
                string name = nameBox.Text;
                string realm = realmBox.Text;
                dynamic user = GetUserInfo(name, realm);
                string userid = user.character.id.Value;
                List<dynamic> dungeons;
                if (userid.IndexOf("@temp") >= 0)
                {
                    dungeons = GetUserDungeonsByName(user.character.playerId.Value, name, realm);
                }
                else
                {
                    dungeons = GetUserDungeons(userid.Replace("@temp", ""));
                }
                Player playerForm = new Player(user, dungeons);
                playerForm.ShowDialog();
            }
            catch(Exception)
            {
                MessageBox.Show("抱歉！未找到该玩家");
            }
        }
        private void Button_Click_1(object sender, RoutedEventArgs e) // 查队
        {
            try
            {
                TextBox groupBox = this.FindName("groupBox") as TextBox;
                List<string> players = new List<string>(groupBox.Text.Split('\n'));
                List<string> playernames = new List<string>();
                List<long> playerDungeonsCount = new List<long>();
                List<string> playerBestDungeon = new List<string>();
                List<long> playerTotalLevels = new List<long>();
                List<dynamic> playerInfos = new List<dynamic>();
                List<dynamic> playerDungeons = new List<dynamic>();
                foreach (string player in players)
                {
                    if (playernames.Count >=5)
                    {
                        break;
                    }
                    string[] p = player.Replace("\n", "").Replace("\r", "").Replace("\r\n", "").Split('-');
                    string name = p[0];
                    string realm = p[1];
                    try
                    {
                        dynamic user = GetUserInfo(name, realm);
                        string userid = user.character.id.Value;
                        List<dynamic> dungeons;
                        if (userid.IndexOf("@temp") >= 0)
                        {
                            dungeons = GetUserDungeonsByName(user.character.playerId.Value, name, realm);
                        }
                        else
                        {
                            dungeons = GetUserDungeons(userid.Replace("@temp", ""));
                        }
                        playerInfos.Add(user);
                        playerDungeons.Add(dungeons);
                        playernames.Add(name + '-' + realm);

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
                        playerDungeonsCount.Add(dungeonsCount);
                        playerBestDungeon.Add(bestDungeonName + "(" + bestDungeonLevel + ", +" + bestDungeonReward + ")");
                        playerTotalLevels.Add(totalLevels);
                    }
                    catch (Exception)
                    {
                        playernames.Add("未找到该玩家");
                        playerDungeonsCount.Add(0);
                        playerBestDungeon.Add("-");
                        playerTotalLevels.Add(0);
                        playerInfos.Add(0);
                        playerDungeons.Add(0);
                    }
                }
                Console.WriteLine(playerDungeonsCount);
                GroupFrame groupFrame = new GroupFrame(playernames, playerDungeonsCount, playerBestDungeon, playerTotalLevels, playerInfos, playerDungeons);
                groupFrame.ShowDialog();
            }
            catch (Exception)
            {
                MessageBox.Show("输入数据格式有误");
            }
        }

        private dynamic GetUserInfo(string name, string realm)
        {
            string url = "https://wowapp.ot.netease.com/wowapp/api/character/simple?name=" + name + "&realm=" + realm;
            return HttpGetJson(url);
        }
        private List<dynamic> GetUserDungeons(string id)
        {
            List<dynamic> dungeons = new List<dynamic>();
            int page_num = 1;
            while (true) {
                string url = "https://wowapp.ot.netease.com/wowapp/api/dungeon/history/" + id + "?page_size=100&page_num=" + page_num;
                dynamic obj = HttpGetJson(url);
                dynamic list = obj.dungeonHistory.list;
                foreach (dynamic li in list) {
                    dungeons.Add(li);
                }
                if (page_num >= obj.dungeonHistory.pageTotal.Value) {
                    break;
                }
                page_num += 1;
            }
            return dungeons;
        }
        private List<dynamic> GetUserDungeonsByName(string playerId, string playerName, string playerRealm)
        {
            Console.WriteLine(playerId);
            Console.WriteLine(playerName);
            Console.WriteLine(playerRealm);
            List<dynamic> dungeons = new List<dynamic>();
            int page_num = 1;
            while (true)
            {
                string url = "https://wowapp.ot.netease.com/wowapp/api/dungeon/history/0?playerId=" + playerId + "&playerName=" + playerName + "&realm=" + playerRealm + "&page_size =100&page_num=" + page_num;

                Console.WriteLine(url);
                dynamic obj = HttpGetJson(url);
                dynamic list = obj.dungeonHistory.list;
                foreach (dynamic li in list)
                {
                    dungeons.Add(li);
                }
                if (page_num >= obj.dungeonHistory.pageTotal.Value)
                {
                    break;
                }
                page_num += 1;
            }
            return dungeons;
        }

        private dynamic HttpGetJson(string url)
        {
            string html = string.Empty;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.AutomaticDecompression = DecompressionMethods.GZip;

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                html = reader.ReadToEnd();
            }

            dynamic jsonObject = JsonConvert.DeserializeObject(html);
            return jsonObject;
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void TextBox_TextChanged_1(object sender, TextChangedEventArgs e)
        {

        }
    }
}
