using System;
using System.Collections.Generic;
using UnityEngine;

namespace Bastion
{
    public sealed class StrikeDirector : MonoBehaviour
    {
        public bool Active;
        public bool Running { get => Active; set => Active = value; }
        public bool Resolved;
        public event Action OnResolved;

        public Transform Camp;
        public readonly List<PersonActor> Column = new List<PersonActor>();

        public void Begin(GameSim sim, IList<PersonActor> people, Transform camp, bool prejudice)
        {
            Launch(people, camp, Mathf.Max(3, sim != null ? sim.OnStrike : 4));
        }

        public void Launch(IList<PersonActor> people, Transform camp, int need)
        {
            Camp = camp;
            Column.Clear();
            Resolved = false;
            Active = false;
            if (people == null || camp == null) return;
            foreach (var p in people)
            {
                if (p == null || p.Role != Role.Warden || p.Job == Job.Possessed) continue;
                p.Job = Job.Strike;
                p.SetTarget(camp.position);
                Column.Add(p);
                if (Column.Count >= need) break;
            }
            Active = Column.Count > 0;
        }

        public void Tick(float dt, IList<PersonActor> people)
        {
            Tick(people);
        }

        public void Tick(IList<PersonActor> people)
        {
            if (!Active || Camp == null) return;
            int here = 0;
            for (int i = 0; i < Column.Count; i++)
            {
                if (Column[i] == null) continue;
                Column[i].SetTarget(Camp.position);
                if ((Column[i].transform.position - Camp.position).sqrMagnitude < 16f) here++;
            }
            if (here < Mathf.Min(3, Column.Count)) return;
            Active = false;
            Resolved = true;
            Flash(Camp.position + Vector3.up);
            int dropped = 0;
            if (people != null)
            {
                for (int i = 0; i < people.Count; i++)
                {
                    var p = people[i];
                    if (p == null || p.Role != Role.Raider || p.Job == Job.Down) continue;
                    if (dropped < 3) { p.Drop(); dropped++; }
                    else p.SetTarget(Camp.position + new Vector3(8f, 0f, 6f));
                }
            }
            OnResolved?.Invoke();
        }

        static void Flash(Vector3 pos)
        {
            var go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            go.transform.position = pos;
            go.transform.localScale = Vector3.one * 0.5f;
            go.GetComponent<Renderer>().sharedMaterial = BastionGfx.Mat(new Color(1f, 0.85f, 0.5f), 5f);
            UnityEngine.Object.Destroy(go.GetComponent<Collider>());
            UnityEngine.Object.Destroy(go, 0.12f);
        }
    }
}
