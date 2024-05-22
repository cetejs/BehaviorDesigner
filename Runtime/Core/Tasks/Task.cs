using System;
using System.Collections;
using UnityEngine;

namespace BehaviorDesigner
{
    [Serializable]
    public class Task : IEquatable<Task>
    {
        [SerializeField]
        private string name;
        [SerializeField] [TextArea]
        private string comment;
        [SerializeField] [HideInInspector]
        public Rect graphPosition = new Rect(400f, 300f, 100f, 100f);
        [SerializeField] [HideInInspector]
        private string guid;
        [SerializeField] [HideInInspector]
        private bool disabled;
        [SerializeField] [HideInInspector]
        private bool breakpoint;
        protected GameObject gameObject;
        protected Transform transform;
        protected BehaviorTree owner;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public Rect Position
        {
            get { return graphPosition; }
            set { graphPosition = value; }
        }

        public string Guid
        {
            get { return guid; }
            set { guid = value; }
        }

        public bool IsDisabled
        {
            get { return disabled; }
            set { disabled = value; }
        }

        public bool IsBreakpoint
        {
            get { return breakpoint; }
            set { breakpoint = value; }
        }

        public TaskStatus CurrentStatus { get; private set; }
#if UNITY_EDITOR
        public Action<bool> UpdateNotifyWithAbort { get; set; }
#endif

        public virtual void Bind(BehaviorSource source)
        {
            source.BindVariables(this);
        }

        public virtual void Init(BehaviorTree behavior)
        {
            owner = behavior;
            gameObject = behavior.gameObject;
            transform = behavior.transform;
            OnAwake();
        }

        public virtual void OnAwake()
        {
        }

        public virtual TaskStatus OnUpdate(bool isUpdateAbort)
        {
            CurrentStatus = OnUpdate();
#if UNITY_EDITOR
            UpdateNotifyWithAbort?.Invoke(isUpdateAbort);
#endif
            return CurrentStatus;
        }

        public virtual TaskStatus OnUpdate()
        {
            return TaskStatus.Success;
        }

        public virtual void OnLateUpdate()
        {
        }

        public virtual void OnFixedUpdate()
        {
        }

        public virtual void OnDrawGizmos()
        {
        }

        public virtual void OnCollisionEnter(Collision collision)
        {
        }

        public virtual void OnCollisionExit(Collision collision)
        {
        }

        public virtual void OnTriggerEnter(Collider other)
        {
        }

        public virtual void OnTriggerExit(Collider other)
        {
        }

        public virtual void OnCollisionEnter2D(Collision2D collision)
        {
        }

        public virtual void OnCollisionExit2D(Collision2D collision)
        {
        }

        public virtual void OnTriggerEnter2D(Collider2D other)
        {
        }

        public virtual void OnTriggerExit2D(Collider2D other)
        {
        }

        public virtual void OnControllerColliderHit(ControllerColliderHit hit)
        {
        }

        public virtual void OnAnimatorIK(int layerIndex)
        {
        }

        public virtual void OnStart()
        {
        }

        public virtual void OnEnd()
        {
        }

        public virtual void OnAbort()
        {
        }

        public virtual void OnReset()
        {
        }

        public bool Equals(Task other)
        {
            if (other == null)
            {
                return false;
            }

            return guid == other.guid;
        }

        protected Coroutine StartCoroutine(IEnumerator routine)
        {
            return owner.StartCoroutine(routine);
        }

        protected void StopCoroutine(IEnumerator routine)
        {
            owner.StopCoroutine(routine);
        }

        protected void StopAllCoroutines()
        {
            owner.StopAllCoroutines();
        }

        protected T GetComponent<T>()
        {
            return gameObject.GetComponent<T>();
        }

        protected T GetComponent<T>(GameObject go)
        {
            if (go)
            {
                return go.GetComponent<T>();
            }

            return GetComponent<T>();
        }

        protected Component GetComponent(Type type)
        {
            return gameObject.GetComponent(type);
        }

        protected bool TryGetComponent<T>(out T component)
        {
            return gameObject.TryGetComponent(out component);
        }

        protected bool GetComponent(Type type, out Component component)
        {
            return gameObject.TryGetComponent(type, out component);
        }
    }
}