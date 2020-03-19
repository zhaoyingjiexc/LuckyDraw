using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 工具名称：跑马灯单次抽奖系统客户端控制
/// 
/// tips:请将item的命名方式改为以"_item"结尾方式
///     设置介绍：
///         startButton : 开始按钮（NGUI制作，必须带有UIButton组件）
///         selectBox : 选中框，可以是图片特效等
///         speed   ：   选中框初速度
///         minSpeed  ：  选中框最小运行速度
///         addSpeed    ：   选中框运行加速度速度
///         cylinderNumber  ：   最少循环圈数
///         selectIndex  ：  抽取到的物品索引，需从服务器获取
/// </summary>
public class LuckDrawTools : MonoBehaviour {

    List<Transform> itemList = new List<Transform>();

    bool isStart = false;
    bool isOver = true;

    public GameObject startButton;    //开始按钮
    public GameObject selectBox;  //选中框

    public float speed = 40f;  //初速度

    float runSpeed;  //运行速度

    public float minSpeed = 3f;  //最小运行速度

    public float addSpeed = 1f;    //减少的加速度

    public int cylinderNumber = 3;  //循环圈数

    public int selectIndex;    //抽到的奖品索引    

    private void Awake()
    {
        Transform[] tmpList = GetComponentsInChildren<Transform>();

        //获取所有的item
        foreach (var item in tmpList)
        {

            if (item.name.EndsWith("_item"))
            {
                itemList.Add(item);
            }
        }

        UIEventListener.Get(startButton).onClick = OnStartBtnClick;

        selectBox.SetActive(false);
    }

    int distance = 0;
    float oneTime = 0;
    int i = 0;

  
    private void Update()
    {
        if (isStart == true && isOver)
        {

            isStart = false;
            isOver = false;
            //重置数据
            distance = 0;
            oneTime = 0;
            i = 0;
            runSpeed = speed;
        }

        if (isOver == false)
        {
            if (distance <= selectIndex + itemList.Count * cylinderNumber)  
            {

                //每隔1s/speed秒执行一次
                if (oneTime < 1f)
                {
                    selectBox.transform.position = itemList[i].position;
                }
                else
                {
                    if (runSpeed > minSpeed)
                        runSpeed -= addSpeed;
                    oneTime = 0;
                    i = (i + 1) % itemList.Count;
                    distance++;
                }
            }
            else
            {
                isOver = true;
            }

            oneTime += Time.deltaTime * runSpeed;
        }
    }

    /// <summary>
    /// 开始按钮点击事件
    /// </summary>
    /// <param name="go"></param>
    void OnStartBtnClick(GameObject go)
    {
        if (isOver)
        {
            selectBox.SetActive(true);
            isStart = true;
        }
    }
}
