using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace BehaviorDesigner
{
    public class BehaviorTree : MonoBehaviour, IBehavior
    {
        [SerializeField]
        private UpdateType updateType;
        [SerializeField]
        private bool restartWhenComplete;
        [SerializeField]
        private bool resetValuesOnRestart;
        [SerializeField]
        private ExternalBehavior externalBehavior;
        [SerializeField]
        private BehaviorSource source = new BehaviorSource();
        private ExternalBehavior instanceBehavior;
        private TaskStatus status;
        private bool isInit;
        private bool isCompleted;

        public event Action<BehaviorTree> OnBehaviorStart;
        public event Action<BehaviorTree> OnBehaviorRestart;
        public event Action<BehaviorTree> OnBehaviorEnd;

        public ExternalBehavior ExternalBehavior
        {
            get { return externalBehavior; }
            set
            {
                if (externalBehavior != value)
                {
                    externalBehavior = value;
                    isInit = false;
                }
            }
        }

        public Object Object
        {
            get
            {
                if (instanceBehavior != null)
                {
                    return instanceBehavior.Object;
                }

                if (externalBehavior != null)
                {
                    return externalBehavior.Object;
                }

                return this;
            }
        }

        public BehaviorSource Source
        {
            get
            {
                if (instanceBehavior != null)
                {
                    return instanceBehavior.Source;
                }

                if (externalBehavior != null)
                {
                    return externalBehavior.Source;
                }

                return source;
            }
        }

        public Root Root
        {
            get { return Source.root; }
        }

        public bool RestartWhenComplete
        {
            get { return restartWhenComplete; }
            set { restartWhenComplete = value; }
        }

        public bool ResetValuesOnRestart
        {
            get { return resetValuesOnRestart; }
            set { resetValuesOnRestart = value; }
        }

        public void Restart()
        {
            if (resetValuesOnRestart)
            {
                BehaviorSource sampleSource = source;
                if (externalBehavior != null)
                {
                    sampleSource = externalBehavior.Source;
                }

                Source.ReloadVariables(sampleSource);
            }

            isCompleted = false;
            Root.OnStart();
            OnBehaviorRestart?.Invoke(this);
        }

        public void Tick()
        {
            if (!isInit)
            {
                Init();
            }

            if (isCompleted && restartWhenComplete)
            {
                Restart();
            }

            if (!isInit || isCompleted || Root.IsDisabled)
            {
                return;
            }

            status = Root.OnUpdate(false);
            if (status != TaskStatus.Running)
            {
                isCompleted = true;
                OnBehaviorEnd?.Invoke(this);
            }
        }

        public void LateTick()
        {
            if (!isInit || isCompleted || Root.IsDisabled)
            {
                return;
            }

            Root.OnLateUpdate();
        }

        public void FixedTick()
        {
            if (!isInit || isCompleted || Root.IsDisabled)
            {
                return;
            }

            Root.OnFixedUpdate();
        }

        private void Init()
        {
            if (isInit || Source == null || Root == null)
            {
                return;
            }

            if (externalBehavior != null)
            {
                instanceBehavior = externalBehavior.Clone();
            }

            isInit = true;
            isCompleted = false;
            Source.UpdateVariables();
            Root.Bind(Source);
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
            if (!isInit || isCompleted || Root.IsDisabled)
            {
                return;
            }

            if (Application.isPlaying)
            {
                Root.OnDrawGizmos();
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (!isInit || isCompleted || Root.IsDisabled)
            {
                return;
            }

            Root.OnCollisionEnter(collision);
        }

        private void OnCollisionExit(Collision collision)
        {
            if (!isInit || isCompleted || Root.IsDisabled)
            {
                return;
            }

            Root.OnCollisionExit(collision);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!isInit || isCompleted || Root.IsDisabled)
            {
                return;
            }

            Root.OnTriggerEnter(other);
        }

        private void OnTriggerExit(Collider other)
        {
            if (!isInit || isCompleted || Root.IsDisabled)
            {
                return;
            }

            Root.OnTriggerExit(other);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (!isInit || isCompleted || Root.IsDisabled)
            {
                return;
            }

            Root.OnCollisionEnter2D(collision);
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            if (!isInit || isCompleted || Root.IsDisabled)
            {
                return;
            }

            Root.OnCollisionExit2D(collision);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!isInit || isCompleted || Root.IsDisabled)
            {
                return;
            }

            Root.OnTriggerEnter2D(other);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!isInit || isCompleted || Root.IsDisabled)
            {
                return;
            }

            Root.OnTriggerExit2D(other);
        }

        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            if (!isInit || isCompleted || Root.IsDisabled)
            {
                return;
            }

            Root.OnControllerColliderHit(hit);
        }

        private void OnAnimatorIK(int layerIndex)
        {
            if (!isInit || isCompleted || Root.IsDisabled)
            {
                return;
            }

            Root.OnAnimatorIK(layerIndex);
        }
    }
}