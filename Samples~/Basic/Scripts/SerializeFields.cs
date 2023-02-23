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

    public override void OnStart()
    {
        Debug.Log("AnimationCurve : " + v101);
        Debug.Log("Behaviour : " + v102);
        Debug.Log("bool : " + v103);
        Debug.Log("Color : " + v104);
        Debug.Log("double : " + v105);
        Debug.Log("float : " + v106);
        Debug.Log("GameObject : " + v107);
        Debug.Log("int : " + v108);
        Debug.Log("LayerMask : " + v109);
        Debug.Log("long : " + v110);
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
    }
}