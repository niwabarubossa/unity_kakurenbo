﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;

public class OniScript : Agent
{
    // Start is called before the first frame update

    public enum Team
    {
        Blue = 0, //子供
        Purple = 1  //鬼
    }


    [HideInInspector]
    public Team team;
    [HideInInspector]
    public Rigidbody agentRb;
    //public Transform Target;
    public Transform Kodomo;
    BehaviorParameters m_BehaviorParameters;
    Vector3 m_Transform;
    public FieldScript area;
    int m_PlayerIndex;
    int directionX;
    int directionZ;
    int now_step;


    public override void InitializeAgent()
    {

        m_BehaviorParameters = gameObject.GetComponent<BehaviorParameters>();
        //outlet したteam idから、初期ポジションを場合分け
        if (m_BehaviorParameters.m_TeamID == (int)Team.Blue)
        {
            team = Team.Blue;
            m_Transform = new Vector3(transform.localPosition.x - 4f, .5f, transform.localPosition.z);
        }
        else
        {
            team = Team.Purple;
            m_Transform = new Vector3(transform.localPosition.x + 4f, .5f, transform.localPosition.z);
        }

        agentRb = GetComponent<Rigidbody>();
        agentRb.maxAngularVelocity = 500;

        var playerState = new PlayerState
        {
            agentRb = agentRb,
            startingPos = transform.localPosition,
            oniScript = this,
        };
        area.playerStates.Add(playerState);
        m_PlayerIndex = area.playerStates.IndexOf(playerState);
        playerState.playerIndex = m_PlayerIndex;
    }



    public override void AgentReset()
    {
        this.now_step = 0;
        float x = Random.Range(-4.3f, 4.3f);
        float z = Random.Range(-4.3f, 4.3f);
        this.agentRb.angularVelocity = Vector3.zero;
        this.agentRb.velocity = Vector3.zero;
        this.transform.localPosition = new Vector3(x, 0.5f, z);
    }

    public float speed = 1;
    public override void AgentAction(float[] vectorAction)
    {
        this.now_step++;
        area.Update();
        // Fell off platform
        // Rewards

        if (now_step < 700) {
            //かくれんぼだから、最初は待機
            return;
        }
        float distanceToTarget = Vector3.Distance(this.transform.localPosition,
                                                  Kodomo.localPosition);
        // Reached target
        if (distanceToTarget < 1.00f)
        {
            area.OniWin();
        }

        // Fell off platform
        if (this.transform.localPosition.y < 0)
        {
            Done();
        }

        MoveAgent(vectorAction);
    }

    public void MoveAgent(float[] act)
    {
        var dirToGo = Vector3.zero;
        var rotateDir = Vector3.zero;

        var forwardAxis = (int)act[0];
        var rightAxis = (int)act[1];
        var rotateAxis = (int)act[2];

        switch (forwardAxis)
        {
            case 1:
                dirToGo = transform.forward * 1f;
                break;
            case 2:
                dirToGo = transform.forward * -1f;
                break;
        }

        switch (rightAxis)
        {
            case 1:
                dirToGo = transform.right * 0.3f;
                break;
            case 2:
                dirToGo = transform.right * -0.3f;
                break;
        }

        switch (rotateAxis)
        {
            case 1:
                rotateDir = transform.up * -1f;
                break;
            case 2:
                rotateDir = transform.up * 1f;
                break;
        }


        transform.Rotate(rotateDir, Time.deltaTime * 100f);
        agentRb.AddForce(dirToGo * this.speed,
            ForceMode.VelocityChange);
    }

    public override float[] Heuristic()
    {
        //動作テスト　キーボード→アクションパラメーター→AgentActoin　→MoveAgentへいき、実際に動く
        var action = new float[4];
        //forward
        if (Input.GetKey(KeyCode.W))
        {
            action[0] = 1f;
        }
        if (Input.GetKey(KeyCode.S))
        {
            action[0] = 2f;
        }
        //rotate
        if (Input.GetKey(KeyCode.A))
        {
            action[2] = 1f;
        }
        if (Input.GetKey(KeyCode.D))
        {
            action[2] = 2f;
        }
        //right
        if (Input.GetKey(KeyCode.E))
        {
            action[1] = 1f;
        }
        if (Input.GetKey(KeyCode.Q))
        {
            action[1] = 2f;
        }

        return action;
    }
}
