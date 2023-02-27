using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner;
using UnityEngine;

[TaskCategory("Test")]
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
    public Material v111;
    public Object v112;
    public Quaternion v113;
    public Rect v114;
    public string v115;
    public Tag v116;
    public Transform v117;
    public Vector2 v118;
    public Vector2Int v119;
    public Vector3 v120;
    public Vector3Int v121;
    public Vector4 v122;

    public List<AnimationCurve> v201;
    public List<Behaviour> v202;
    public List<bool> v203;
    public List<Color> v204;
    public List<double> v205;
    public List<float> v206;
    public List<GameObject> v207;
    public List<int> v208;
    public List<long> v209;
    public List<Material> v210;
    public List<Object> v211;
    public List<Quaternion> v212;
    public List<Rect> v213;
    public List<string> v214;
    public List<Transform> v215;
    public List<Vector2> v216;
    public List<Vector2Int> v217;
    public List<Vector3> v218;
    public List<Vector3Int> v219;
    public List<Vector4> v220;

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
        Debug.Log("Object : " + v112);
        Debug.Log("Quaternion : " + v113);
        Debug.Log("Rect : " + v114);
        Debug.Log("string : " + v115);
        Debug.Log("Tag : " + v116);
        Debug.Log("Transform : " + v117);
        Debug.Log("Vector2 : " + v118);
        Debug.Log("Vector2Int : " + v119);
        Debug.Log("Vector3 : " + v120);
        Debug.Log("Vector3Int : " + v121);
        Debug.Log("Vector4 : " + v122);
        
        Debug.Log("AnimationCurveList : " + string.Join(",", v201));
        Debug.Log("BehaviourList : " + string.Join(",", v202));
        Debug.Log("BoolList : " + string.Join(",", v203));
        Debug.Log("ColorList : " + string.Join(",", v204));
        Debug.Log("DoubleList : " + string.Join(",", v205));
        Debug.Log("FloatList : " + string.Join(",", v206));
        Debug.Log("GameObjectList : " + string.Join(",", v207));
        Debug.Log("IntList : " + string.Join(",", v208));
        Debug.Log("LongList : " + string.Join(",", v209));
        Debug.Log("MaterialList : " + string.Join(",", v210));
        Debug.Log("ObjectList : " + string.Join(",", v211));
        Debug.Log("QuaternionList : " + string.Join(",", v212));
        Debug.Log("RectList : " + string.Join(",", v213));
        Debug.Log("StringList : " + string.Join(",", v214));
        Debug.Log("TransformList : " + string.Join(",", v215));
        Debug.Log("Vector2List : " + string.Join(",", v216));
        Debug.Log("Vector2IntList : " + string.Join(",", v217));
        Debug.Log("Vector3List : " + string.Join(",", v218));
        Debug.Log("Vector3IntList : " + string.Join(",", v219));
        Debug.Log("Vector4List : " + string.Join(",", v220));
    }
}