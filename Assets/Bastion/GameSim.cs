using System;
using System.Collections.Generic;
using UnityEngine;

namespace Bastion
{
    public enum Speed : int { Pause = 0, One = 1, Two = 2, Four = 4 }

    public sealed class Mission
    {
        public string Id;
        public string Title;
        public int DaysLeft;
        public int Wardens;
        public bool Prejudice;
        public float Odds;
    }

    public sealed class GameSim
    {
        public int Day = 1;
        public float DayT;
        public const float DaySeconds = 5f;

        public float Food = 36f;
        public float Water = 14f;
        public float Power = 22f;
        public float Scrap = 48f;
        public float Morale = 58f;
        public int Pop = 28;
        public int WardensIdle = 12;
        public int Peacekeepers;
        public int OnStrike;

        public int OrderAsh = 34;
        public bool CisternQueued;
        public bool CisternBuilt;
        public float CisternDays;
        public bool AshPosted;
        public bool StrikeOffered;
        public bool RaidHit;

        public readonly List<Mission> Missions = new List<Mission>();
        public readonly List<string> Log = new List<string>();

        public Speed Speed = Speed.One;
        public string Toast;
        public float ToastT;
        public Action<string> OnToast;
        public Action<string, string, string, Action, Action> OnModal;

        public float FoodPerDay => 8f + (CisternBuilt ? 0f : 0f) + 6f;
        public float WaterPerDay => (CisternBuilt ? 16f : 5f);
        public float FoodUse => Pop * 0.45f;
        public float WaterUse => Pop * 0.52f;

        public void ToastNow(string msg)
        {
            Toast = msg;
            ToastT = 4.2f;
            Log.Insert(0, $"[Day {Day}] {msg}");
            if (Log.Count > 24) Log.RemoveAt(Log.Count - 1);
            OnToast?.Invoke(msg);
        }

        public void Tick(float dt)
        {
            if (Speed == Speed.Pause) return;
            DayT += dt * (int)Speed;
            if (ToastT > 0f) ToastT -= dt;
            if (DayT < DaySeconds) return;
            DayT -= DaySeconds;
            AdvanceDay();
        }

        void AdvanceDay()
        {
            Day++;
            Food += FoodPerDay - FoodUse;
            Water += WaterPerDay - WaterUse;
            Power += 3f - Pop * 0.08f;
            Morale += (AshPosted ? 1.2f : -0.6f) + (Water < 0f ? -3f : 0.4f);
            if (!AshPosted) OrderAsh = Mathf.Max(0, OrderAsh - 6);
            else OrderAsh = Mathf.Min(100, OrderAsh + 8);

            if (CisternQueued && !CisternBuilt)
            {
                CisternDays += 1f;
                if (CisternDays >= 2f)
                {
                    CisternBuilt = true;
                    CisternQueued = false;
                    ToastNow("Water holds. For now.");
                }
            }

            for (int i = Missions.Count - 1; i >= 0; i--)
            {
                var m = Missions[i];
                m.DaysLeft--;
                if (m.DaysLeft > 0) continue;
                Resolve(m);
                Missions.RemoveAt(i);
            }

            if (Water < 0f) ToastNow("Ration line forming.");
            if (Food < 0f) ToastNow("Stores are thin.");
            Food = Mathf.Clamp(Food, -20f, 200f);
            Water = Mathf.Clamp(Water, -20f, 200f);
            Power = Mathf.Clamp(Power, 0f, 200f);
            Morale = Mathf.Clamp(Morale, 0f, 100f);
        }

        public bool QueueCistern()
        {
            if (CisternBuilt || CisternQueued) return false;
            if (Scrap < 18f)
            {
                ToastNow("Need 18 scrap for a cistern.");
                return false;
            }
            Scrap -= 18f;
            CisternQueued = true;
            CisternDays = 0f;
            ToastNow("Crew on Wire Street.");
            return true;
        }

        public bool PostAsh(int n = 2)
        {
            if (AshPosted) return false;
            if (WardensIdle < n)
            {
                ToastNow("No free Wardens.");
                return false;
            }
            WardensIdle -= n;
            Peacekeepers += n;
            AshPosted = true;
            ToastNow("Beat's quiet.");
            return true;
        }

        public bool LaunchStrike(int n, bool prejudice)
        {
            if (OnStrike > 0) return false;
            if (WardensIdle < n)
            {
                ToastNow("Column needs more Wardens.");
                return false;
            }
            WardensIdle -= n;
            OnStrike = n;
            float odds = 0.38f + n * 0.09f + (prejudice ? 0.18f : 0f) + (AshPosted ? 0.04f : 0f);
            Missions.Add(new Mission
            {
                Id = "camp-west",
                Title = "Camp West",
                DaysLeft = 3,
                Wardens = n,
                Prejudice = prejudice,
                Odds = Mathf.Clamp01(odds)
            });
            ToastNow(prejudice ? "Column is moving. Prejudice standing." : "Column is moving.");
            return true;
        }

        void Resolve(Mission m)
        {
            bool win = UnityEngine.Random.value < m.Odds;
            OnStrike = 0;
            WardensIdle += Mathf.Max(0, m.Wardens - (win ? 0 : 1));
            if (win)
            {
                Scrap += m.Prejudice ? 22f : 18f;
                Morale += m.Prejudice ? -4f : 3f;
                OrderAsh = Mathf.Min(100, OrderAsh + 8);
                ToastNow(m.Prejudice
                    ? "Camp West broken. No one left standing. Scrap +22."
                    : "Camp West broken. Scrap +18. Order +8.");
            }
            else
            {
                Morale -= 6f;
                Food -= 8f;
                ToastNow("They held the ruin. We lost a Warden. Food -8.");
            }
        }

        public void MaybeRaid()
        {
            if (RaidHit) return;
            if (Day < 3) return;
            if (Missions.Exists(m => m.Id == "camp-west")) return;
            RaidHit = true;
            Food -= 12f;
            Morale -= 5f;
            ToastNow("They hit the stores. Food -12.");
        }
    }
}
