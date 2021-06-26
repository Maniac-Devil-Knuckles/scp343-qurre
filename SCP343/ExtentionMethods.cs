using System;
using Qurre.API;
using MEC;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System.Reflection;
using System.Linq;
using scp = SCP343.scp343;
using SCP343.Handlers;
using Qurre;

namespace SCP343
{
    public static class ExtentionMethods
    {
        internal static List<int> GetIntList(this Qurre.API.Config config, string key,List<int> def, string comment = "")
        {   
            try
            {
                string _def = "";
                foreach (string str in def.Select(d => d.ToString())) _def += $"{str},";
                _def = _def.Substring(0, _def.Length - 1);
                Log.Info(_def);
                string _result = config.GetString(key, _def, comment);
                List<int> result = new List<int>();
                string[] vs = _result.Split(',');
                if (vs.Length > 0)
                {
                    foreach(string str in vs)
                    {
                        if (int.TryParse(str, out int i)) result.Add(i);
                    }
                    if (result.Count == 0)
                    {
                        Log.Error("Not found list of int in " + key);
                        return def;
                    }
                    else return result;
                }
                else return result;
            }
            catch (Exception ex)
            {
                
                Log.Info(key);
                Log.Error(ex);
                return def;
            }
        }

        public static bool IsSCP343(this Player player) => scp343badgelist.Contains(player);

        public static Badge GetSCPBadge(this Player player)
        {
            Badge badge = null;
            if (player.IsSCP343()) badge = API.AllScp343Badges.FirstOrDefault(x => x.Player.Id == player.Id);
            return badge;
        }
    }
}
