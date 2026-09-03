using UnityEngine;

namespace Bastion
{
    public static class EventDeck
    {
        public static void Maybe(GameSim sim)
        {
            if (sim == null) return;
            int d = sim.Day;
            if (d == 3 && !sim.CisternQueued && !sim.CisternBuilt)
                sim.ToastNow("Water dies in days. Raise a cistern on Wire Street.");
            else if (d == 6 && !sim.CisternBuilt)
                sim.ToastNow("The well rope is wet with dust. Cistern or ration riot.");
            else if (d == 8 && !sim.AshPosted)
                sim.ToastNow("Ash Row is loud. Post a beat or lose the street.");
            else if (d == 12 && sim.OnStrike == 0 && !sim.RaidHit)
                sim.ToastNow("Camp West is still out there. The column is waiting.");
            else if (d == 16 && sim.Water > 20f && sim.Food > 20f && sim.Pop < 40)
            {
                sim.Pop += 1;
                sim.ToastNow("A family asks for a floor. Pop +1.");
            }
            else if (d % 7 == 0 && sim.CisternBuilt)
            {
                sim.Scrap += 4f;
                sim.ToastNow("Scavengers drag a cart up Wire Street. Scrap +4.");
            }
            else if (d % 9 == 0 && sim.AshPosted)
            {
                sim.Food += 3f;
                sim.ToastNow("Lamp stall pays the beat in bread. Food +3.");
            }
            else if (d >= 45 && !sim.AshPosted && sim.OrderAsh <= 0)
                sim.ToastNow("The Ward still walks. The city does not follow.");
        }
    }
}
