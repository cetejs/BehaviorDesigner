using System.Reflection;
using UnityEngine.UIElements;

namespace BehaviorDesigner.Editor
{
    public class BoolResolver : FieldResolver<Toggle, bool>
    {
        public BoolResolver(FieldInfo fieldInfo, BehaviorWindow window) : base(fieldInfo, window)
        {
        }

        protected override Toggle CreateEditorField(FieldInfo fieldInfo)
        {
            return new Toggle(fieldInfo.Name);
        }

        public static bool IsAcceptable(FieldInfo info)
        {
            return info.FieldType == typeof(bool);
        }
    }
}