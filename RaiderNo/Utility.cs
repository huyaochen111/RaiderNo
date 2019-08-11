using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace RaiderNo
{
    class Utility
    {
        static public dynamic GetUserInfo(string name, string realm)
        {
            string url = "https://wowapp.ot.netease.com/wowapp/api/character/simple?name=" + name + "&realm=" + realm;
            return HttpGetJson(url);
        }
        static public List<dynamic> GetUserDungeons(string id)
        {
            List<dynamic> dungeons = new List<dynamic>();
            int page_num = 1;
            while (true)
            {
                string url = "https://wowapp.ot.netease.com/wowapp/api/dungeon/history/" + id + "?page_size=100&page_num=" + page_num;
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
        static public List<dynamic> GetUserDungeonsByName(string playerId, string playerName, string playerRealm)
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

        static public dynamic HttpGetJson(string url)
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

    }
}
