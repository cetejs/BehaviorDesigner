using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner;
using UnityEngine;

[TaskCategory("Test")]
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
        Debug.Log("SharedMaterial : " + v111.Value);
        Debug.Log("SharedObject : " + v112.Value);
        Debug.Log("SharedQuaternion : " + v113.Value);
        Debug.Log("SharedRect : " + v114.Value);
        Debug.Log("SharedString : " + v115.Value);
        Debug.Log("SharedTag : " + v116.Value);
        Debug.Log("SharedTransform : " + v117.Value);
        Debug.Log("SharedVector2 : " + v118.Value);
        Debug.Log("SharedVector2Int : " + v119.Value);
        Debug.Log("SharedVector3 : " + v120.Value);
        Debug.Log("SharedVector3Int : " + v121.Value);
        Debug.Log("SharedVector4 : " + v122.Value);

        Debug.Log("SharedAnimationCurveList : " + string.Join(",", v201.Value));
        Debug.Log("SharedBehaviourList : " + string.Join(",", v202.Value));
        Debug.Log("SharedBoolList : " + string.Join(",", v203.Value));
        Debug.Log("SharedColorList : " + string.Join(",", v204.Value));
        Debug.Log("SharedDoubleList : " + string.Join(",", v205.Value));
        Debug.Log("SharedFloatList : " + string.Join(",", v206.Value));
        Debug.Log("SharedGameObjectList : " + string.Join(",", v207.Value));
        Debug.Log("SharedIntList : " + string.Join(",", v208.Value));
        Debug.Log("SharedLongList : " + string.Join(",", v209.Value));
        Debug.Log("SharedMaterialList : " + string.Join(",", v120.Value));
        Debug.Log("SharedObjectList : " + string.Join(",", v211.Value));
        Debug.Log("SharedQuaternionList : " + string.Join(",", v212.Value));
        Debug.Log("SharedRectList : " + string.Join(",", v213.Value));
        Debug.Log("SharedStringList : " + string.Join(",", v214.Value));
        Debug.Log("SharedTransformList : " + string.Join(",", v215.Value));
        Debug.Log("SharedVector2List : " + string.Join(",", v216.Value));
        Debug.Log("SharedVector2IntList : " + string.Join(",", v217.Value));
        Debug.Log("SharedVector3List : " + string.Join(",", v218.Value));
        Debug.Log("SharedVector3IntList : " + string.Join(",", v219.Value));
        Debug.Log("SharedVector4List : " + string.Join(",", v220.Value));
    }
}