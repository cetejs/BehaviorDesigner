using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace BehaviorDesigner
{
    public abstract class Behavior : MonoBehaviour, IBehavior
    {
        [SerializeField]
        private UpdateType updateType;
        [SerializeField]
        private bool restartWhenComplete;
        [SerializeField]
        private bool resetValuesOnRestart;
        [SerializeField]
        private BehaviorSource source;
        [SerializeField]
        private ExternalBehavior external;

        private TaskStatus status;
        private bool isInit;
        private bool isCompleted;

        public event Action<Behavior> OnBehaviorStart;
        public event Action<Behavior> OnBehaviorRestart;
        public event Action<Behavior> OnBehaviorEnd;

        public Root Root
        {
            get { return Source.Root; }
        }

        public BehaviorSource Source
        {
            get
            {
                if (external != null)
                {
                    return external.Source;
                }

                if (source == null)
                {
                    source = new BehaviorSource();
                }

                return source;
            }
        }

        public int InstanceID
        {
            get { return GetInstanceID(); }
        }

        public Object Object
        {
            get
            {
                if (external != null)
                {
                    return external.Object;
                }

                return this;
            }
        }

#if UNITY_EDITOR
        public void ClearSource()
        {
            source = new BehaviorSource();
        }
#endif

        public void SetExternalBehavior(ExternalBehavior behavior)
        {
            external = behavior;
            isInit = false;
        }

        public void Restart()
        {
            if (resetValuesOnRestart)
            {
                Source.ReloadVariables();
                Root?.Bind(this);
            }

            isCompleted = false;
            Root?.OnStart();
            OnBehaviorRestart?.Invoke(this);
        }

        public void Tick()
        {
            if (!isInit)
            {
                Init();
            }

            if (isCompleted)
            {
                if (restartWhenComplete)
                {
                    Restart();
                }

                return;
            }

            status = Root.Update();
            if (status != TaskStatus.Running)
            {
                isCompleted = true;
                OnBehaviorEnd?.Invoke(this);
            }
        }

        public void LateTick()
        {
            if (!isInit || isCompleted)
            {
                return;
            }

            Root?.OnLateUpdate();
        }

        public void FixedTick()
        {
            if (!isInit || isCompleted)
            {
                return;
            }

            Root?.OnFixedUpdate();
        }

        public void BindVariables(Task task)
        {
            Source.BindVariables(task);
        }

        private void Init()
        {
            isInit = true;
            isCompleted = false;
            Source.Load();
            Root.Bind(this);
            Root.Init(this);
            OnBehaviorStart?.Invoke(this);
        }

        private void Update()
        {
            if (updateType == UpdateType.Auto)
            {
                Tick();
            }
        }

        private void LateUpdate()
        {
            if (updateType == UpdateType.Auto)
            {
                LateTick();
            }
        }

        private void FixedUpdate()
        {
            if (updateType == UpdateType.Auto)
            {
                FixedTick();
            }
        }

        private void OnDrawGizmos()
        {
            if (!isInit || isCompleted)
            {
                return;
            }

            if (Application.isPlaying)
            {
                Root?.OnDrawGizmos();
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (!isInit || isCompleted)
            {
                return;
            }

            Root?.OnCollisionEnter(collision);
        }

        private void OnCollisionStay(Collision collision)
        {
            if (!isInit || isCompleted)
            {
                return;
            }

            Root?.OnCollisionStay(collision);
        }

        private void OnCollisionExit(Collision collision)
        {
            if (!isInit || isCompleted)
            {
                return;
            }

            Root?.OnCollisionExit(collision);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!isInit || isCompleted)
            {
                return;
            }

            Root?.OnTriggerEnter(other);
        }

        private void OnTriggerStay(Collider other)
        {
            if (!isInit || isCompleted)
            {
                return;
            }

            Root?.OnTriggerStay(other);
        }

        private void OnTriggerExit(Collider other)
        {
            if (!isInit || isCompleted)
            {
                return;
            }

            Root?.OnTriggerExit(other);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!isInit || isCompleted)
            {
                return;
            }

            Root?.OnTriggerEnter2D(other);
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if (!isInit || isCompleted)
            {
                return;
            }

            Root?.OnTriggerStay2D(other);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!isInit || isCompleted)
            {
                return;
            }

            Root?.OnTriggerExit2D(other);
        }

        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            if (!isInit || isCompleted)
            {
                return;
            }

            Root?.OnControllerColliderHit(hit);
        }

        private void OnAnimatorIK(int layerIndex)
        {
            if (!isInit || isCompleted)
            {
                return;
            }

            Root?.OnAnimatorIK(layerIndex);
        }
    }
}