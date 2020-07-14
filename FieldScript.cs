using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

//親　総てを司るファイル
[System.Serializable]
public class PlayerState
{
    public int playerIndex;
    //わからない
    [FormerlySerializedAs("agentRB")]
    public Rigidbody agentRb;
    public Vector3 startingPos;
    public CubeAgent agentScript;
    public OniScript oniScript;
}

public class FieldScript : MonoBehaviour
{

    //public CanMoveCube canMoveCube;
    //[HideInInspector]
    //public Rigidbody canMoveCubeRb;

    public List<PlayerState> playerStates = new List<PlayerState>();

    private void Awake()
    {
        //this.canMoveCubeRb = this.canMoveCube.canMoveCubeRb;
    }
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    public void Update()
    {
        foreach (var ps in playerStates)
        {
            if (ps.agentScript != null)
            {
                //子供は時間が経つごとに報酬を
                // max step == 3000 だから。
              ps.agentScript.AddReward(1f / 3000f);
            }
            else
            {
              ps.oniScript.AddReward(-1f / 3000f);
            }

        }
    }

    public void OniWin()
    {
    print("oni win");
        //接触したオブジェクトのタグが"Player"のとき

            //オブジェクトの色を赤に変更する
            foreach (var ps in playerStates)
            {
                print("finish");
                if (ps.agentScript != null)
                {
                    //子供の負け
                    ps.agentScript.AddReward(-1);
                    ps.agentScript.Done();
                }
                else
                {
                //鬼の勝利なので
                    ps.oniScript.AddReward(1);
                    ps.oniScript.Done();
                //ps.agentScript.AddReward(1);
            }

                 //all agents need to be reset                           

            }
    }
}
