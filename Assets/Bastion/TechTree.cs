using System.Collections.Generic;
using UnityEngine;

namespace Bastion
{
    public enum Doctrine { None, PaxHaven, IronHaven }

    public sealed class TechDef
    {
        public string Id;
        public string Name;
        public string Blurb;
        public int Days;
        public int Scrap;
        public string Requires;
    }

    public sealed class TechTree
    {
        public int Science;
        public string QueueId;
        public float QueueDays;
        public Doctrine Doctrine;
        public readonly HashSet<string> Done = new HashSet<string>();

        public static readonly TechDef[] Catalog =
        {
            new TechDef { Id = "ration", Name = "Ration Discipline", Blurb = "Water use -12%.", Days = 2, Scrap = 8 },
            new TechDef { Id = "filters", Name = "Silt Filters", Blurb = "Cistern +4 water/day.", Days = 3, Scrap = 12, Requires = "ration" },
            new TechDef { Id = "radio", Name = "Beat Radios", Blurb = "Peacekeepers +10 order.", Days = 3, Scrap = 10 },
            new TechDef { Id = "breach", Name = "Breach Drills", Blurb = "Strike odds +8%.", Days = 3, Scrap = 14 },
            new TechDef { Id = "pax", Name = "Pax Haven", Blurb = "Doctrine: order from trust.", Days = 4, Scrap = 16, Requires = "radio" },
            new TechDef { Id = "iron", Name = "Iron Haven", Blurb = "Doctrine: order from fear.", Days = 4, Scrap = 16, Requires = "breach" },
            new TechDef { Id = "greenhouse", Name = "Glass Bay", Blurb = "Hydro food +6/day.", Days = 4, Scrap = 18, Requires = "filters" },
            new TechDef { Id = "archive", Name = "The Archive", Blurb = "Science +1/day. Extra event choice.", Days = 5, Scrap = 20 }
        };

        public static TechDef Find(string id)
        {
            for (int i = 0; i < Catalog.Length; i++)
                if (Catalog[i].Id == id) return Catalog[i];
            return null;
        }

        public bool Owns(string id) => Done.Contains(id);

        public bool Queue(GameSim sim, string id)
        {
            if (!string.IsNullOrEmpty(QueueId))
            {
                sim.ToastNow("Lab is busy.");
                return false;
            }
            var t = Find(id);
            if (t == null) return false;
            if (Done.Contains(id))
            {
                sim.ToastNow("Already known.");
                return false;
            }
            if (!string.IsNullOrEmpty(t.Requires) && !Done.Contains(t.Requires))
            {
                sim.ToastNow("Missing prerequisite.");
                return false;
            }
            if (sim.Scrap < t.Scrap)
            {
                sim.ToastNow("Need more scrap.");
                return false;
            }
            if (id == "pax" && Doctrine == Doctrine.IronHaven) { sim.ToastNow("Iron already chosen."); return false; }
            if (id == "iron" && Doctrine == Doctrine.PaxHaven) { sim.ToastNow("Pax already chosen."); return false; }
            sim.Scrap -= t.Scrap;
            QueueId = id;
            QueueDays = t.Days;
            sim.ToastNow("Researching " + t.Name + ".");
            return true;
        }

        public void TickDay(GameSim sim)
        {
            Science += Owns("archive") ? 2 : 1;
            if (string.IsNullOrEmpty(QueueId)) return;
            QueueDays -= 1f;
            if (QueueDays > 0f) return;
            Done.Add(QueueId);
            var t = Find(QueueId);
            if (QueueId == "pax") Doctrine = Doctrine.PaxHaven;
            if (QueueId == "iron") Doctrine = Doctrine.IronHaven;
            QueueId = null;
            sim.ToastNow((t != null ? t.Name : "Tech") + " is live.");
        }

        public float WaterUseMul => Owns("ration") ? 0.88f : 1f;
        public float WaterProdAdd => Owns("filters") ? 4f : 0f;
        public float FoodProdAdd => Owns("greenhouse") ? 6f : 0f;
        public float StrikeOddsAdd => Owns("breach") ? 0.08f : 0f;
        public int OrderAdd => Owns("radio") ? 10 : 0;
    }
}
