using System;

namespace BehaviorDesigner
{
    [Serializable]
    public class SharedDouble : SharedVariable<double>
    {
        public static implicit operator SharedDouble(double value)
        {
            return new SharedDouble {Value = value};
        }
    }
}