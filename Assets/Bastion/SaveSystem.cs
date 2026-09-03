using UnityEngine;

namespace Bastion
{
    public static class SaveSystem
    {
        public const string Key = "BASTION_HAVEN_V1";

        public static void Save(GameSim sim, TechTree tech)
        {
            if (sim == null || tech == null) return;
            var done = "";
            foreach (var id in tech.Done)
            {
                if (done.Length > 0) done += ",";
                done += id;
            }
            var line = string.Join("|",
                sim.Day, F(sim.Food), F(sim.Water), F(sim.Power), F(sim.Scrap), F(sim.Morale),
                sim.Pop, sim.WardensIdle, sim.Peacekeepers, sim.OrderAsh,
                sim.CisternQueued ? 1 : 0, sim.CisternBuilt ? 1 : 0,
                sim.AshPosted ? 1 : 0, sim.RaidHit ? 1 : 0, F(sim.CisternDays),
                tech.Science, tech.QueueId ?? "", F(tech.QueueDays), (int)tech.Doctrine, done);
            PlayerPrefs.SetString(Key, line);
            PlayerPrefs.Save();
            sim.ToastNow("Archive written. Day " + sim.Day);
        }

        public static bool Load(GameSim sim, TechTree tech)
        {
            if (sim == null || tech == null) return false;
            if (!PlayerPrefs.HasKey(Key))
            {
                sim.ToastNow("No archive.");
                return false;
            }
            var p = PlayerPrefs.GetString(Key).Split('|');
            if (p.Length < 19)
            {
                sim.ToastNow("Archive broken.");
                return false;
            }
            sim.Day = I(p[0], sim.Day);
            sim.Food = Fl(p[1], sim.Food);
            sim.Water = Fl(p[2], sim.Water);
            sim.Power = Fl(p[3], sim.Power);
            sim.Scrap = Fl(p[4], sim.Scrap);
            sim.Morale = Fl(p[5], sim.Morale);
            sim.Pop = I(p[6], sim.Pop);
            sim.WardensIdle = I(p[7], sim.WardensIdle);
            sim.Peacekeepers = I(p[8], sim.Peacekeepers);
            sim.OrderAsh = I(p[9], sim.OrderAsh);
            sim.CisternQueued = p[10] == "1";
            sim.CisternBuilt = p[11] == "1";
            sim.AshPosted = p[12] == "1";
            sim.RaidHit = p[13] == "1";
            sim.CisternDays = Fl(p[14], sim.CisternDays);
            tech.Science = I(p[15], tech.Science);
            tech.QueueId = p[16];
            tech.QueueDays = Fl(p[17], tech.QueueDays);
            tech.Doctrine = (Doctrine)I(p[18], 0);
            tech.Done.Clear();
            if (p.Length > 19 && p[19].Length > 0)
            {
                foreach (var id in p[19].Split(','))
                    if (id.Length > 0) tech.Done.Add(id);
            }
            sim.ToastNow("Archive opened. Day " + sim.Day);
            return true;
        }

        static string F(float v) => v.ToString("F2");
        static int I(string s, int fb) { int v; return int.TryParse(s, out v) ? v : fb; }
        static float Fl(string s, float fb) { float v; return float.TryParse(s, out v) ? v : fb; }
    }
}
