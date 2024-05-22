using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner;
using UnityEngine;

[TaskGroup("Test")]
public class SetSharedVariables : Action
{
    public SharedAnimationCurve v101;
    public SharedBehaviour v102;
    public SharedBool v103;
    public SharedColor v104;
    public SharedDouble v105;
    public SharedFloat v106;
    public SharedGameObject v107;
    public SharedInt v108;
    public SharedLayerMask v109;
    public SharedLong v110;
    public SharedObject v111;
    public SharedQuaternion v112;
    public SharedRect v113;
    public SharedString v114;
    public SharedTransform v115;
    public SharedVector2 v116;
    public SharedVector2Int v117;
    public SharedVector3 v118;
    public SharedVector3Int v119;
    public SharedVector4 v120;
    public SharedTag v121;

    public SharedGameObjectList v201;
    public SharedObjectList v202;
    public SharedTransformList v203;

    public override void OnStart()
    {
        v101.Value = new AnimationCurve(new Keyframe[]
        {
            new Keyframe(0, 0),
            new Keyframe(0.5f, 0.5f),
            new Keyframe(1, 0)
        });
        v102.Value = owner;
        v103.Value = false;
        v104.Value = Color.blue;
        v105.Value = 123;
        v106.Value = 123;
        v107.Value = new GameObject("123");
        v108.Value = 123;
        v109.Value = 123;
        v110.Value = 123;
        v111.Value = new GameObject("123");
        v112.Value = Quaternion.Euler(1, 2, 3);
        v113.Value = new Rect(1, 2, 3, 4);
        v114.Value = "123";
        v115.Value = new GameObject("123").transform;
        v116.Value = new Vector2(1, 2);
        v117.Value = new Vector2Int(1, 2);
        v118.Value = new Vector3(1, 2, 3);
        v119.Value = new Vector3Int(1, 2, 3);
        v120.Value = new Vector4(1, 2, 3, 4);
        v121.Value = "Player";

        v201.Value.Add(new GameObject("1"));
        v201.Value.Add(new GameObject("12"));
        v201.Value.Add(new GameObject("123"));
        v202.Value.Add(new GameObject("1"));
        v202.Value.Add(new GameObject("12"));
        v202.Value.Add(new GameObject("123"));
        v203.Value.Add(new GameObject("1").transform);
        v203.Value.Add(new GameObject("12").transform);
        v203.Value.Add(new GameObject("123").transform);
    }
}