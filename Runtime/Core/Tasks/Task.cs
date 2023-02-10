using System;
using System.Collections;
using UnityEngine;

namespace BehaviorDesigner
{
    [Serializable]
    public abstract class Task
    {
#if UNITY_EDITOR
        [HideInInspector]
        public string guid;
        [HideInInspector]
        public Rect graphPosition = new Rect(400f, 300f, 100f, 100f);
        [HideInInspector]
        public string comment;
        public Action<TaskStatus> UpdateNotifyOnEditor { get; set; }
#endif
        [HideInInspector]
        [SerializeField]
        protected bool isDisabled;
        protected GameObject gameObject;
        protected Transform transform;
        protected Behavior owner;
        protected TaskStatus currentStatus;

        public TaskStatus CurrentStatus
        {
            get { return currentStatus; }
        }

        public bool IsDisabled
        {
            get { return isDisabled; }
            set { isDisabled = value; }
        }

        public virtual void Init(Behavior behavior)
        {
            owner = behavior;
            gameObject = behavior.gameObject;
            transform = behavior.transform;
            behavior.BindVariables(this);
            OnAwake();
        }

        public virtual void Restart()
        {
            OnStart();
            currentStatus = TaskStatus.Inactive;
#if UNITY_EDITOR
            UpdateNotifyOnEditor?.Invoke(currentStatus);
#endif
        }

        public virtual void OnAwake()
        {
        }

        public TaskStatus Update()
        {
            currentStatus = OnUpdate();
#if UNITY_EDITOR
            UpdateNotifyOnEditor?.Invoke(currentStatus);
#endif
            return currentStatus;
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

        public virtual void OnCollisionStay(Collision collision)
        {
        }

        public virtual void OnCollisionExit(Collision collision)
        {
        }

        public virtual void OnTriggerEnter(Collider other)
        {
        }

        public virtual void OnTriggerStay(Collider other)
        {
        }

        public virtual void OnTriggerExit(Collider other)
        {
        }

        public virtual void OnTriggerEnter2D(Collider2D other)
        {
        }

        public virtual void OnTriggerStay2D(Collider2D other)
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

        public virtual void OnPause(bool isPaused)
        {
        }

        public virtual void OnAbort()
        {
        }

        public virtual void OnReset()
        {
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