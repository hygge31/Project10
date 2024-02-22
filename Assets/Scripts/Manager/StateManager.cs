using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum BuffType
{
    Search,
    Move,
    Attack

}

public class StateManager : MonoBehaviour
{
    private static StateManager instance;
    public static StateManager Instance { get { return instance; } }
    public CoolTImeDataSO cooltimeData;

    [Header("Trail Mesh")]
    float refreshRate = 0.05f;
    bool isTrailActive;
    SkinnedMeshRenderer skinnedMeshRenderer;
    public Material mat;

    Pooling pooling;
    Transform poolingBox;



    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    private void Start()
    {
        pooling = GetComponent<Pooling>();
        poolingBox = GameManager.instance.poolingBox;
        pooling.CreatePoolItem(poolingBox);
    }




    public void MoveBuffEffect(float cooltime)
    {
        StartCoroutine(ActiveTrailCo(cooltime));
    }

    IEnumerator ActiveTrailCo(float activeTime)
    {
        GameManager.instance.player.GetComponent<PlayerMovement>().runSpeed = 5;
        GameManager.instance.player.GetComponent<PlayerMovement>().isRuning = true;
        skinnedMeshRenderer = GameManager.instance.player.transform.Find("Body").GetComponent<SkinnedMeshRenderer>();
        while (activeTime > 0)
        {
            activeTime -= Time.deltaTime;

            GameObject gObj = pooling.GetPoolItem("TrailMat");
            gObj.transform.SetPositionAndRotation(GameManager.instance.player.transform.position, GameManager.instance.player.transform.rotation);
            MeshRenderer mr = gObj.GetComponent<MeshRenderer>();
            MeshFilter mf = gObj.GetComponent<MeshFilter>();

            Mesh mesh = new Mesh();
            skinnedMeshRenderer.BakeMesh(mesh);
            mf.mesh = mesh;
            mr.material = mat;
            gObj.SetActive(true);

            pooling.Destroy(gObj, 0.5f);

            yield return null;
        }
        GameManager.instance.player.GetComponent<PlayerMovement>().runSpeed = 1;
        GameManager.instance.player.GetComponent<PlayerMovement>().isRuning = false;
    }
}

