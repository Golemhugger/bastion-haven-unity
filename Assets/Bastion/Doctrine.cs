using UnityEngine;

namespace Bastion
{
    public static class DoctrineStatus
    {
        public static string Status(TechTree tech, GameSim sim)
        {
            if (tech == null || sim == null) return "Doctrine: None";
            if (tech.Doctrine == Doctrine.PaxHaven) return "Doctrine: Pax Haven — order from trust";
            if (tech.Doctrine == Doctrine.IronHaven) return "Doctrine: Iron Haven — order from fear";
            return "Doctrine: None";
        }

        public static string Ending(TechTree tech, GameSim sim)
        {
            if (sim == null || sim.Day < 20) return null;
            if (tech != null && tech.Doctrine == Doctrine.PaxHaven && sim.Morale >= 60)
                return "Haven holds. People stay because they choose to.";
            if (tech != null && tech.Doctrine == Doctrine.IronHaven && sim.OrderAsh >= 70)
                return "Haven holds. No one tests the line twice.";
            if (sim.Water < 0f && sim.Food < 0f)
                return "The ration line broke. The city did not.";
            return null;
        }
    }
}
