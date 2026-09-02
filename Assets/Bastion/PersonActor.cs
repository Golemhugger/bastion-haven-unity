using UnityEngine;

namespace Bastion
{
    public enum Role { Warden, Civilian, Raider }
    public enum Job { Idle, Patrol, Work, Build, Strike, Possessed, Down }

    public sealed class PersonActor : MonoBehaviour
    {
        public string DisplayName;
        public Role Role;
        public Job Job;
        public float Phase;
        public Vector3 Home;
        public Vector3 Target;
        public bool HasTarget;
        public float Speed = 3.1f;

        Transform _lLeg, _rLeg, _lArm, _rArm, _body;

        static readonly string[] WardenNames =
        {
            "Mara Voss", "Eli Rourke", "Jun Hale", "Sable Orr", "Tomas Wren", "Nia Beck",
            "Calder Voss", "Rhee Dolan", "Kell Rane", "Ivo Hart", "Sera Quill", "Dax Wren"
        };
        static readonly string[] CivNames =
            { "Pax Mill", "Oren Vale", "Tila Moss", "Bek Yarrow", "Suni Kade", "Harl Penn", "Mio Glass", "Ryn Cobb" };
        static readonly string[] RaiderNames =
            { "Grit", "Hook", "Ash-Tooth", "Red Wire", "Cinder", "Nail" };
        static readonly Color[] CivPalette =
        {
            new Color(0.77f, 0.71f, 0.63f), new Color(0.42f, 0.49f, 0.42f),
            new Color(0.54f, 0.42f, 0.33f), new Color(0.29f, 0.35f, 0.38f),
            new Color(0.69f, 0.54f, 0.29f), new Color(0.48f, 0.42f, 0.48f)
        };

        public static PersonActor Spawn(Transform parent, Role role, Vector3 pos, int index)
        {
            var go = new GameObject(role.ToString());
            go.transform.SetParent(parent);
            go.transform.position = pos;
            var p = go.AddComponent<PersonActor>();
            p.Role = role;
            p.Home = pos;
            p.Phase = Random.Range(0f, 6f);
            p.BuildMesh(index);
            var col = go.AddComponent<CapsuleCollider>();
            col.height = 1.7f;
            col.radius = 0.28f;
            col.center = new Vector3(0f, 0.85f, 0f);
            return p;
        }

        void BuildMesh(int index)
        {
            Color body, dark, accent;
            float scale = 1f;
            if (Role == Role.Warden)
            {
                DisplayName = WardenNames[index % WardenNames.Length];
                body = new Color(0.24f, 0.36f, 0.36f);
                dark = new Color(0.10f, 0.14f, 0.16f);
                accent = new Color(0.49f, 0.78f, 0.78f);
                scale = 1.08f;
                Speed = 3.2f;
                Job = Job.Patrol;
            }
            else if (Role == Role.Raider)
            {
                DisplayName = RaiderNames[index % RaiderNames.Length];
                body = new Color(0.42f, 0.23f, 0.16f);
                dark = new Color(0.16f, 0.10f, 0.08f);
                accent = new Color(0.55f, 0.22f, 0.14f);
                scale = 0.96f;
                Speed = 2.8f;
                Job = Job.Idle;
            }
            else
            {
                DisplayName = CivNames[index % CivNames.Length] + " " + (index + 1);
                body = CivPalette[index % CivPalette.Length];
                dark = new Color(0.18f, 0.14f, 0.12f);
                accent = body * 0.7f;
                Speed = 2.4f;
                Job = Job.Work;
            }

            transform.localScale = Vector3.one * scale;
            _body = Part("Torso", new Vector3(0f, 1.05f, 0f), new Vector3(0.42f, 0.62f, 0.28f), body);
            Part("Head", new Vector3(0f, 1.52f, 0.02f), new Vector3(0.28f, 0.28f, 0.28f), body * 1.15f);
            if (Role == Role.Warden)
            {
                Part("Helm", new Vector3(0f, 1.62f, 0.02f), new Vector3(0.36f, 0.18f, 0.32f), dark);
                Part("Visor", new Vector3(0f, 1.58f, 0.16f), new Vector3(0.30f, 0.08f, 0.08f), accent);
                Part("CoatL", new Vector3(-0.22f, 0.85f, -0.06f), new Vector3(0.12f, 0.42f, 0.08f), dark);
                Part("CoatR", new Vector3(0.22f, 0.85f, -0.06f), new Vector3(0.12f, 0.42f, 0.08f), dark);
                Part("Baton", new Vector3(0.32f, 0.95f, 0.18f), new Vector3(0.05f, 0.05f, 0.55f), new Color(0.12f, 0.12f, 0.12f));
            }
            else if (Role == Role.Raider)
            {
                transform.rotation = Quaternion.Euler(8f, 0f, 0f);
                Part("Spike", new Vector3(-0.28f, 1.28f, 0f), new Vector3(0.08f, 0.28f, 0.08f), dark);
            }
            else
            {
                Part("Hair", new Vector3(0f, 1.66f, 0f), new Vector3(0.26f, 0.10f, 0.26f), dark);
                Part("Pack", new Vector3(0f, 1.05f, -0.20f), new Vector3(0.22f, 0.24f, 0.12f), accent);
            }

            _lLeg = Part("LLeg", new Vector3(-0.12f, 0.32f, 0f), new Vector3(0.12f, 0.46f, 0.12f), dark);
            _rLeg = Part("RLeg", new Vector3(0.12f, 0.32f, 0f), new Vector3(0.12f, 0.46f, 0.12f), dark);
            _lArm = Part("LArm", new Vector3(-0.30f, 1.12f, 0f), new Vector3(0.10f, 0.42f, 0.10f), body * 0.85f);
            _rArm = Part("RArm", new Vector3(0.30f, 1.12f, 0f), new Vector3(0.10f, 0.42f, 0.10f), body * 0.85f);
        }

        Transform Part(string n, Vector3 local, Vector3 scale, Color c)
        {
            var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
            go.name = n;
            go.transform.SetParent(transform, false);
            go.transform.localPosition = local;
            go.transform.localScale = scale;
            go.GetComponent<Renderer>().sharedMaterial = BastionGfx.Mat(c, n == "Visor" ? 1.4f : 0f);
            Object.Destroy(go.GetComponent<Collider>());
            return go.transform;
        }

        public void SetTarget(Vector3 world)
        {
            Target = world;
            HasTarget = true;
        }

        public void Tick(float dt, Vector3 possessMove)
        {
            if (Job == Job.Down)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(90f, transform.eulerAngles.y, 0f), dt * 4f);
                return;
            }

            Vector3 move = Vector3.zero;
            if (Job == Job.Possessed) move = possessMove;
            else if (HasTarget)
            {
                var d = Target - transform.position;
                d.y = 0f;
                if (d.magnitude < 0.55f) HasTarget = false;
                else move = d.normalized;
            }
            else if (Job == Job.Patrol || Job == Job.Work || Job == Job.Build || Job == Job.Strike)
            {
                if (Random.value < dt * 0.35f)
                {
                    Vector3 o = Job == Job.Strike ? Target : Home;
                    Target = o + new Vector3(Random.Range(-6f, 6f), 0f, Random.Range(-6f, 6f));
                    HasTarget = true;
                }
            }

            float spd = Speed * (Job == Job.Possessed ? 1.15f : 1f);
            if (move.sqrMagnitude > 0.01f)
            {
                move.y = 0f;
                move.Normalize();
                transform.position += move * spd * dt;
                var look = Quaternion.LookRotation(move, Vector3.up);
                transform.rotation = Quaternion.Slerp(transform.rotation, look, dt * 8f);
                Phase += dt * 9f;
                float s = Mathf.Sin(Phase);
                var tp = transform.position;
                transform.position = new Vector3(tp.x, 0.02f + Mathf.Abs(s) * 0.06f, tp.z);
                if (_lLeg)
                {
                    _lLeg.localRotation = Quaternion.Euler(s * 32f, 0f, 0f);
                    _rLeg.localRotation = Quaternion.Euler(-s * 32f, 0f, 0f);
                    _lArm.localRotation = Quaternion.Euler(-s * 22f, 0f, 0f);
                    _rArm.localRotation = Quaternion.Euler(s * 22f, 0f, 0f);
                }
            }
            else
            {
                var tp = transform.position;
                transform.position = new Vector3(tp.x, 0.02f, tp.z);
                Phase += dt * 1.2f;
                if (_body) _body.localPosition = new Vector3(0f, 1.05f + Mathf.Sin(Phase) * 0.015f, 0f);
                if (_lLeg)
                {
                    _lLeg.localRotation = Quaternion.Slerp(_lLeg.localRotation, Quaternion.identity, dt * 6f);
                    _rLeg.localRotation = Quaternion.Slerp(_rLeg.localRotation, Quaternion.identity, dt * 6f);
                    _lArm.localRotation = Quaternion.Slerp(_lArm.localRotation, Quaternion.identity, dt * 6f);
                    _rArm.localRotation = Quaternion.Slerp(_rArm.localRotation, Quaternion.identity, dt * 6f);
                }
            }
        }

        public void Drop()
        {
            Job = Job.Down;
            HasTarget = false;
        }
    }
}
