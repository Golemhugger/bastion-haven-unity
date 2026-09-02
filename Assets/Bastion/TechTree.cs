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
        public Doctrine RequiresDoctrine;
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
            new TechDef { Id = "ration", Name = "Ration Discipline", Blurb = "Water use -12%. Morale +2 while dry.", Days = 2, Scrap = 8 },
            new TechDef { Id = "filters", Name = "Silt Filters", Blurb = "Cistern output +4 water/day.", Days = 3, Scrap = 12, Requires = "ration" },
            new TechDef { Id = "radio", Name = "Beat Radios", Blurb = "Peacekeepers hold +10 order.", Days = 3, Scrap = 10 },
            new TechDef { Id = "breach", Name = "Breach Drills", Blurb = "Strike odds +8%.", Days = 3, Scrap = 14 },
            new TechDef { Id = "pax", Name = "Pax Haven", Blurb = "Doctrine: order from trust. Morale floor rises.", Days = 4, Scrap = 16, Requires = "radio" },
            new TechDef { Id = "iron", Name = "Iron Haven", Blurb = "Doctrine: order from fear. Prejudice is cheaper.", Days = 4, Scrap = 16, Requires = "breach" },
            new TechDef { Id = "greenhouse", Name = "Glass Bay", Blurb = "Hydro food +6/day.", Days = 4, Scrap = 18, Requires = "filters" },
            new TechDef { Id = "archive", Name = "The Archive", Blurb = "Events show one extra choice. Science +1/day.", Days = 5, Scrap = 20 }
        };

        public bool Owns(string id) => Done.Contains(id);

        public bool Queue(GameSim sim, string id)
        {
            if (QueueId != null) { sim.ToastNow("Lab is busy."); return false; }
            var t = Find(id);
            if (t == null) return false;
            if (Done.Contains(id)) { sim.ToastNow("Already known."); return false; }
            if (t.Requires != null && !Done.Contains(t.Requires)) { sim.ToastNow("Missing 