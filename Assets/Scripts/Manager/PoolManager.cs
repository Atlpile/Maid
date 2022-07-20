using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PoolManager : MonoBehaviour
{
    //创建列表，用于存储对象池的Prefab
    public List<GameObject> poolPrefabs;

    //创建对象池列表：用于存储多个列表中的对象池
    private List<ObjectPool<GameObject>> poolEffectList = new List<ObjectPool<GameObject>>();

    private void OnEnable()
    {
        EventHandler.InitSoundEffect += InitSoundEffect;
    }
    private void OnDisable()
    {
        EventHandler.InitSoundEffect -= InitSoundEffect;

    }

    private void OnInitSoundEffect(SoundDetails obj)
    {

    }

    private void Start()
    {
        CreatePool();
    }

    /// <summary>
    /// 延迟返回对象池
    /// </summary>
    /// <param name="pool">创建的对象池</param>
    /// <param name="obj">对象池中生成的物品</param>
    /// <returns></returns>
    private IEnumerator ReleaseRoutine(ObjectPool<GameObject> pool, GameObject obj)
    {
        //1.5s后，将该GameObject返回对象池
        yield return new WaitForSeconds(1.5f);
        pool.Release(obj);
    }

    /// <summary>
    /// 生成对象池
    /// </summary>
    private void CreatePool()
    {
        //遍历所有用于创建对象池的Prefab
        foreach (GameObject item in poolPrefabs)
        {
            //为对象池生成空的父物体（根据Prefab名字创建）
            Transform parent = new GameObject(item.name).transform;
            //设置对象池的父物体，为PoolManager的子物体
            parent.SetParent(transform);

            //创建新对象池
            var newPool = new ObjectPool<GameObject>(
                () => Instantiate(item, parent),            //创建对象池的物品
                e => { e.SetActive(true); },                //获取对象池的物品时，执行的方法
                e => { e.SetActive(false); },               //释放对象池时，执行的方法
                e => { Destroy(e); }                        //销毁对象池
            );

            //将创建出对象池的物品返回至对象池列表中
            poolEffectList.Add(newPool);
        }
    }


    private void InitSoundEffect(SoundDetails soundDetails)
    {
        ObjectPool<GameObject> pool = poolEffectList[0];
        var obj = pool.Get();

        obj.GetComponent<Sound>().SetSound(soundDetails);
        StartCoroutine(DisableSound(pool, obj, soundDetails));
    }

    private IEnumerator DisableSound(ObjectPool<GameObject> pool, GameObject obj, SoundDetails soundDetails)
    {
        yield return new WaitForSeconds(soundDetails.soundClip.length);
        pool.Release(obj);
    }
}
