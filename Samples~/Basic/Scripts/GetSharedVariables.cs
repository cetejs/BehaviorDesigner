using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner;
using UnityEngine;

[TaskGroup("Test")]
public class GetSharedVariables : Action
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
        Debug.Log("SharedAnimationCurve : " + string.Join(",", v101.Value.keys));
        Debug.Log("SharedBehaviour : " + v102.Value);
        Debug.Log("SharedBool : " + v103.Value);
        Debug.Log("SharedColor : " + v104.Value);
        Debug.Log("SharedDouble : " + v105.Value);
        Debug.Log("SharedFloat : " + v106.Value);
        Debug.Log("SharedGameObject : " + v107.Value);
        Debug.Log("SharedInt : " + v108.Value);
        Debug.Log("SharedLayerMask : " + v109.Value.value);
        Debug.Log("SharedLong : " + v110.Value);
        Debug.Log("SharedObject : " + v111.Value);
        Debug.Log("SharedQuaternion : " + v112.Value);
        Debug.Log("SharedRect : " + v113.Value);
        Debug.Log("SharedString : " + v114.Value);
        Debug.Log("SharedTransform : " + v115.Value);
        Debug.Log("SharedVector2 : " + v116.Value);
        Debug.Log("SharedVector2Int : " + v117.Value);
        Debug.Log("SharedVector3 : " + v118.Value);
        Debug.Log("SharedVector3Int : " + v119.Value);
        Debug.Log("SharedVector4 : " + v120.Value);
        Debug.Log("SharedTag : " + v121.Value);

        Debug.Log("SharedGameObjectList : " + string.Join(",", v201.Value));
        Debug.Log("SharedObjectList : " + string.Join(",", v202.Value));
        Debug.Log("SharedTransformList : " + string.Join(",", v203.Value));
    }
}