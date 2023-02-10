using System;
using UnityEngine;

namespace BehaviorDesigner
{
    [Serializable]
    public abstract class SharedVariable
    {
        [SerializeField]
        private string name;
        [SerializeField]
        private bool isShared;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public bool IsShared
        {
            get { return isShared; }
            set { isShared = value; }
        }

        public abstract object GetValue();

        public abstract void SetValue(object value);

        public abstract void Bind(SharedVariable other);
    }

    [Serializable]
    public abstract class SharedVariable<T> : SharedVariable
    {
        [SerializeField]
        protected T value;
        private Func<T> getter;
        private Action<T> setter;

        public T Value
        {
            get
            {
                if (getter != null)
                {
                    return getter();
                }

                return value;
            }
            set
            {
                if (setter != null)
                {
                    setter(value);
                }
                else
                {
                    this.value = value;
                }
            }
        }

        public override object GetValue()
        {
            return value;
        }

        public override void SetValue(object value)
        {
            if (setter != null)
            {
                setter((T) value);
            }
            else
            {
                this.value = (T) value;
            }
        }

        public override void Bind(SharedVariable other)
        {
            getter = () =>
            {
                return (T)other.GetValue();
            };

            setter = value =>
            {
                other.SetValue(value);
            };
        }
    }
}