using System;
using Qurre.API;
using System.Collections.Generic;
using System.Linq;
using Qurre;

namespace SCP343
{
    public static class ExtentionMethods
    {
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

        public static List<T> GetListEnum<T>(this Config config, string key, List<T> def, string comment = "") where T: struct
        {
            try
            {
                string _def = string.Join(",", def.Select(d => Convert.ToInt32(d).ToString()));
                Log.Info(_def);
                string _result = config.GetString(key, _def, comment);
                List<T> result = _result.Split(',').Select(r =>
                {
                    if (Enum.TryParse(r, out T t)) return t;
                    Enum.TryParse("-1", out t);
                    return t;
                }).ToList();
                return result;
            }
            catch (Exception ex)
            {
                Log.Error(key);
                Log.Error(ex);
                return def;
            }
        }

    }
}
