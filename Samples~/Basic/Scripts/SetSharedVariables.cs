using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner;
using UnityEngine;

[TaskCategory("Test")]
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
    public SharedMaterial v111;
    public SharedObject v112;
    public SharedQuaternion v113;
    public SharedRect v114;
    public SharedString v115;
    public SharedTag v116;
    public SharedTransform v117;
    public SharedVector2 v118;
    public SharedVector2Int v119;
    public SharedVector3 v120;
    public SharedVector3Int v121;
    public SharedVector4 v122;

    public SharedAnimationCurveList v201;
    public SharedBehaviourList v202;
    public SharedBoolList v203;
    public SharedColorList v204;
    public SharedDoubleList v205;
    public SharedFloatList v206;
    public SharedGameObjectList v207;
    public SharedIntList v208;
    public SharedLongList v209;
    public SharedMaterialList v210;
    public SharedObjectList v211;
    public SharedQuaternionList v212;
    public SharedRectList v213;
    public SharedStringList v214;
    public SharedTransformList v215;
    public SharedVector2List v216;
    public SharedVector2IntList v217;
    public SharedVector3List v218;
    public SharedVector3IntList v219;
    public SharedVector4List v220;

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
        v111.Value = new Material(Shader.Find("Standard"));
        v112.Value = new GameObject("123");
        v113.Value = Quaternion.Euler(1, 2, 3);
        v114.Value = new Rect(1, 2, 3, 4);
        v115.Value = "123";
        v116.Value = "123";
        v117.Value = new GameObject("123").transform;
        v118.Value = new Vector2(1, 2);
        v119.Value = new Vector2Int(1, 2);
        v120.Value = new Vector3(1, 2, 3);
        v121.Value = new Vector3Int(1, 2, 3);
        v122.Value = new Vector4(1, 2, 3, 4);

        v201.Value.Add(new AnimationCurve(new Keyframe[]
        {
            new Keyframe(0, 0),
            new Keyframe(0.5f, 0.5f),
            new Keyframe(1, 0)
        }));
        v201.Value.Add(new AnimationCurve(new Keyframe[]
        {
            new Keyframe(0, 0),
            new Keyframe(0.5f, 0.5f),
            new Keyframe(1, 0)
        }));
        v201.Value.Add(new AnimationCurve(new Keyframe[]
        {
            new Keyframe(0, 0),
            new Keyframe(0.5f, 0.5f),
            new Keyframe(1, 0)
        }));
        v202.Value.Add(owner);
        v202.Value.Add(owner);
        v202.Value.Add(owner);
        v203.Value.Add(true);
        v203.Value.Add(false);
        v203.Value.Add(true);
        v204.Value.Add(Color.white);
        v204.Value.Add(Color.black);
        v204.Value.Add(Color.red);
        v205.Value.Add(1);
        v205.Value.Add(12);
        v205.Value.Add(123);
        v206.Value.Add(1);
        v206.Value.Add(12);
        v206.Value.Add(123);
        v207.Value.Add(new GameObject("1"));
        v207.Value.Add(new GameObject("12"));
        v207.Value.Add(new GameObject("123"));
        v208.Value.Add(1);
        v208.Value.Add(12);
        v208.Value.Add(123);
        v209.Value.Add(1);
        v209.Value.Add(12);
        v209.Value.Add(123);
        v210.Value.Add(new Material(Shader.Find("Standard")));
        v210.Value.Add(new Material(Shader.Find("Standard")));
        v210.Value.Add(new Material(Shader.Find("Standard")));
        v211.Value.Add(new GameObject("1"));
        v211.Value.Add(new GameObject("12"));
        v211.Value.Add(new GameObject("123"));
        v212.Value.Add(Quaternion.Euler(1, 2, 3));
        v212.Value.Add(Quaternion.Euler(4, 5, 6));
        v212.Value.Add(Quaternion.Euler(7, 8, 9));
        v213.Value.Add(new Rect(1, 2, 3, 4));
        v213.Value.Add(new Rect(5, 6, 7, 8));
        v213.Value.Add(new Rect(9, 10, 11, 12));
        v214.Value.Add("1");
        v214.Value.Add("12");
        v214.Value.Add("123");
        v215.Value.Add(new GameObject("1").transform);
        v215.Value.Add(new GameObject("12").transform);
        v215.Value.Add(new GameObject("123").transform);
        v216.Value.Add(new Vector2(1, 1));
        v216.Value.Add(new Vector2(12, 12));
        v216.Value.Add(new Vector2(123, 123));
        v217.Value.Add(new Vector2Int(1, 1));
        v217.Value.Add(new Vector2Int(12, 12));
        v217.Value.Add(new Vector2Int(123, 123));
        v218.Value.Add(new Vector3(1, 1, 1));
        v218.Value.Add(new Vector3(12, 12, 12));
        v218.Value.Add(new Vector3(123, 123, 123));
        v219.Value.Add(new Vector3Int(1, 1, 1));
        v219.Value.Add(new Vector3Int(12, 12, 12));
        v219.Value.Add(new Vector3Int(123, 123, 123));
        v220.Value.Add(new Vector4(1, 1, 1, 1));
        v220.Value.Add(new Vector4(12, 12, 12, 12));
        v220.Value.Add(new Vector4(123, 123, 123, 123));
    }
}