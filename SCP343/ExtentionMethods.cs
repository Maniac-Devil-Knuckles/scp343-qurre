using System;
using Qurre.API;
using System.Collections.Generic;
using System.Linq;
using Qurre;

namespace SCP343
{
    public static class ExtentionMethods
    {
        internal static List<int> GetIntList(this Config config, string key,List<int> def, string comment = "")
        {   
            try
            {
                string _def = string.Join(",", def.Select(d => d.ToString()));
                Log.Info(_def);
                string _result = config.GetString(key, _def, comment);
                List<int> result = _result.Split(',').Where(s => int.TryParse(s, out var res)).Select(s => int.Parse(s)).ToList();
                return result;
            }
            catch (Exception ex)
            {
                Log.Info(key);
                Log.Error(ex);
                return def;
            }
        }

        public static void SetHP(this Player player, float value)
        {
            if (value + player.Hp >= player.MaxHp) player.Hp = player.MaxHp;
            else player.Hp += value;
        }

        public static bool IsSCP343(this Player player) => scp343badgelist.Contains(player);

        public static Badge GetSCPBadge(this Player player)
        {
            Badge badge = null;
            if (player.IsSCP343()) badge = scp343badgelist.Get(player);
            return badge;
        }

        internal static int IndexOf(this List<int> list, ItemType en)
        {
            int num = (int)en;
            return list.IndexOf(num);
        }

    }
}
