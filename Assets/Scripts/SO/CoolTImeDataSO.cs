using System.Collections;
using System.Collections.Generic;
using UnityEngine;





[CreateAssetMenu(fileName ="Cooltime Data",menuName ="Create Cooltime Data")]
public class CoolTImeDataSO : ScriptableObject
{
    public float searchBuffCooltime;
    public float movementBuffCooltime;
    public float attackBuffCooltime;
}
