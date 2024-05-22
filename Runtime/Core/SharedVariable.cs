using System;
using UnityEngine;
using UnityEngine.Serialization;

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

        public SharedVariable Clone()
        {
            return MemberwiseClone() as SharedVariable;
        }
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

        public override void SetValue(object newValue)
        {
            if (setter != null)
            {
                setter((T) newValue);
            }
            else
            {
                value = (T) newValue;
            }
        }

        public override void Bind(SharedVariable other)
        {
            getter = () =>
            {
                return (T) other.GetValue();
            };

            setter = newValue =>
            {
                other.SetValue(newValue);
            };
        }
    }
}