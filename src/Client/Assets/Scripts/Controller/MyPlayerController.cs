using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Google.Protobuf.Protocol;

public class MyPlayerController : PlayerController
{
    bool _moveUpdated = false;

    public override MoveDir Dir 
    {   
        get { return base.Dir; }
        set
        {
            if (base.Dir == value)
                return;

            base.Dir = value;
            _moveUpdated = false;
        }
    }

    void GetDirInput()
    {
        if (BaseState != BaseState.Skill && _isGrounded)
        {
            ProcessMoveInput();
        }

        if (BaseState != BaseState.Skill && _isGrounded && Input.GetKeyDown(KeyCode.C))
        {
            ProcessJumpInput();
        }

        if (BaseState != BaseState.Skill && _isGrounded && Input.GetKeyDown(KeyCode.A))
        {
            ProcessSkillInput();
        }
    }

    private void ProcessJumpInput()
    {
        if (BaseState == BaseState.Idle || BaseState == BaseState.Moving)
        {
            SendJumpPacket();
        }
    }

    private void ProcessMoveInput()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            if (!_moveUpdated)
            {
                SendMoveStartPacket();
                _moveUpdated = true;
            }
            Dir = MoveDir.Left;
            BaseState = BaseState.Moving;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            if (!_moveUpdated)
            {
                SendMoveStartPacket();
                _moveUpdated = true;
            }
            Dir = MoveDir.Right;
            BaseState = BaseState.Moving;
        }
        else
        {
            if (_moveUpdated)
            {
                SendMoveEndPacket();
                _moveUpdated = false;
            }
            BaseState = BaseState.Idle;
        }
    }

    private void ProcessSkillInput()
    {
        if (BaseState == BaseState.Idle || BaseState == BaseState.Moving)
        {
            SendSkillPacket();
        }
    }

    protected override void UpdateBehavior()
    {
        GetDirInput();

        base.UpdateBehavior();
    }

    protected override void UpdateIdle()
    {

    }

    void SendMoveStartPacket()
    {
        C_MoveStart moveStartPkt = new C_MoveStart();
        moveStartPkt.PlayerState = PlayerState;
        Manager.NetworkManager.Send(moveStartPkt);
    }

    void SendMoveEndPacket()
    {
        C_MoveEnd moveEndPkt = new C_MoveEnd();
        moveEndPkt.PlayerState = PlayerState;
        Manager.NetworkManager.Send(moveEndPkt);
    }
    
    void SendJumpPacket()
    {
        C_Jump jumpPkt = new C_Jump();
        jumpPkt.PlayerState = PlayerState;  
        Manager.NetworkManager.Send(jumpPkt);
    }

    protected override void UpdateJump()
    {
        base.UpdateJump();

        if (_isGrounded && BaseState == BaseState.Idle)
        {
            C_JumpEnd jumpEndPkt = new C_JumpEnd();
            jumpEndPkt.PlayerState = PlayerState;
            Manager.NetworkManager.Send(jumpEndPkt);

            if (_moveUpdated)
            {
                SendMoveStartPacket();
            }
        }
    }

    void SendSkillPacket()
    {
        C_Skill skillPkt = new C_Skill();   
        skillPkt.PlayerState = PlayerState;
        Manager.NetworkManager.Send(skillPkt);
    }

    public override void UseSkill()
    {
        base.UseSkill();
    }

    IEnumerator CoStartSkill()
    {
        BaseState = BaseState.Skill;
        yield return new WaitForSeconds(0.4f);
        BaseState = BaseState.Idle;
        _coSkill = null;

        SendMoveEndPacket();
        _moveUpdated = false;
    }
}
