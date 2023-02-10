using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace BehaviorDesigner
{
    [Serializable]
    [VariablePriority(1)]
    public class SharedMaterialList : SharedList<Material>
    {
    }
}