using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner;
using UnityEngine;

[TaskGroup("Test")]
public class SerializeFields : Action
{
    public AnimationCurve v101;
    public Behaviour v102;
    public bool v103;
    public Color v104;
    public double v105;
    public float v106;
    public GameObject v107;
    public int v108;
    public LayerMask v109;
    public long v110;
    public Object v111;
    public Quaternion v112;
    public Rect v113;
    public string v114;
    public Transform v115;
    public Vector2 v116;
    public Vector2Int v117;
    public Vector3 v118;
    public Vector3Int v119;
    public Vector4 v120;
    public Tag v121;

    public List<AnimationCurve> v201;
    public List<Behaviour> v202;
    public List<bool> v203;
    public List<Color> v204;
    public List<float> v205;
    public List<GameObject> v206;
    public List<int> v207;
    public List<Material> v208;
    public List<Object> v209;
    public List<Quaternion> v210;
    public List<string> v211;
    public List<Transform> v212;
    public List<Vector2> v213;
    public List<Vector3> v214;
    public List<Vector4> v215;

    public override void OnStart()
    {
        Debug.Log("AnimationCurve : " + v101);
        Debug.Log("Behaviour : " + v102);
        Debug.Log("Bool : " + v103);
        Debug.Log("Color : " + v104);
        Debug.Log("Double : " + v105);
        Debug.Log("Float : " + v106);
        Debug.Log("GameObject : " + v107);
        Debug.Log("Int : " + v108);
        Debug.Log("LayerMask : " + v109);
        Debug.Log("Long : " + v110);
        Debug.Log("Material : " + v111);
        Debug.Log("Object : " + v111);
        Debug.Log("Quaternion : " + v112);
        Debug.Log("Rect : " + v113);
        Debug.Log("string : " + v114);
        Debug.Log("Transform : " + v115);
        Debug.Log("Vector2 : " + v116);
        Debug.Log("Vector2Int : " + v117);
        Debug.Log("Vector3 : " + v118);
        Debug.Log("Vector3Int : " + v119);
        Debug.Log("Vector4 : " + v120);
        Debug.Log("Tag : " + v121);
        
        Debug.Log("AnimationCurveList : " + string.Join(",", v201));
        Debug.Log("BehaviourList : " + string.Join(",", v202));
        Debug.Log("BoolList : " + string.Join(",", v203));
        Debug.Log("ColorList : " + string.Join(",", v204));
        Debug.Log("FloatList : " + string.Join(",", v205));
        Debug.Log("GameObjectList : " + string.Join(",", v206));
        Debug.Log("IntList : " + string.Join(",", v207));
        Debug.Log("MaterialList : " + string.Join(",", v208));
        Debug.Log("ObjectList : " + string.Join(",", v209));
        Debug.Log("QuaternionList : " + string.Join(",", v210));
        Debug.Log("StringList : " + string.Join(",", v211));
        Debug.Log("TransformList : " + string.Join(",", v212));
        Debug.Log("Vector2List : " + string.Join(",", v213));
        Debug.Log("Vector3List : " + string.Join(",", v214));
        Debug.Log("Vector4List : " + string.Join(",", v215));
    }
}