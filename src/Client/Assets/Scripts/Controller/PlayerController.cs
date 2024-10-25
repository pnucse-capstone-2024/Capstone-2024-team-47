using Google.Protobuf.Protocol;
using System;
using System.Collections;
using UnityEngine;

public class PlayerController : CreatureController
{
    public float Speed = 5.0f;
    public float JumpForce = 7.5f;
    public LayerMask groundLayer;
    public Transform groundCheck;

    Rigidbody2D _rigidbody;
    float _verticalVelocity;

    protected Coroutine _coSkill;

    protected override void Init()
    {
        base.Init();
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    protected override void UpdateAnimation()
    {
        switch (BaseState)
        {
            case BaseState.Idle:
                ProcessIdleState();
                break;
            case BaseState.Moving:
                ProcessMovingState();
                break;
            case BaseState.Skill:
                ProcessSkillState();
                break;
            case BaseState.Dead:
                ProcessDeadState();
                break;
            case BaseState.Jump:
                ProcessJumpState();
                break;
        }
    }

    void ProcessIdleState()
    {
        try
        {
            // 자꾸 버그 생김.
            _animator.Play("HeroKnight_Idle");
            switch (Dir)
            {
                case MoveDir.Left:
                    _spriteRenderer.flipX = true;
                    break;
                case MoveDir.Right:
                    _spriteRenderer.flipX = false;
                    break;
            }
        }
        catch (Exception e)
        {
            // Debug.LogException(e);
        }

    }
    void ProcessMovingState()
    {
        _animator.Play("HeroKnight_Run");
        switch (Dir)
        {
            case MoveDir.Left:
                _spriteRenderer.flipX = true;
                break;
            case MoveDir.Right:
                _spriteRenderer.flipX = false;
                break;
        }
    }
    void ProcessSkillState()
    {
        _animator.Play("HeroKnight_Attack1");
        switch (Dir)
        {
            case MoveDir.Left:
                _spriteRenderer.flipX = true;
                break;
            case MoveDir.Right:
                _spriteRenderer.flipX = false;
                break;
        }
    }
    void ProcessDeadState()
    {

    }
    void ProcessJumpState()
    {
        _animator.Play("HeroKnight_Jump");
        _verticalVelocity = JumpForce;
    }

    protected override void UpdateIdle()
    {
    }

    protected override void UpdateDead()
    {
    }

    protected override void UpdateMoving()
    {
        float moveConstant = (Dir == MoveDir.Left) ? -1.0f : 1.0f;

        transform.position += new Vector3(moveConstant * Speed * Time.deltaTime, 0.0f, 0.0f);
        Position = transform.position;
    }

    protected override void UpdateSkill()
    {
    }

    protected override void UpdateJump()
    {
        if (!_isGrounded)
        {
            float moveConstant = (Dir == MoveDir.Left) ? -1.0f : 1.0f;

            _verticalVelocity += -9.8f * Time.deltaTime;

            transform.position += new Vector3(moveConstant * Speed * Time.deltaTime, _verticalVelocity * Time.deltaTime, 0.0f);
            Position = transform.position;

            if (Time.time - _lastJumpTime > 0.2f)
            {
                _isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);
                if (_isGrounded)
                {
                    BaseState = BaseState.Idle;
                }
            }
        }
    }

    public virtual void UseSkill()
    {
        _coSkill = StartCoroutine("CoStartSkill");
    }

    IEnumerator CoStartSkill()
    {
        BaseState = BaseState.Skill;
        yield return new WaitForSeconds(0.4f);
        BaseState = BaseState.Idle;
        _coSkill = null;
    }
}
