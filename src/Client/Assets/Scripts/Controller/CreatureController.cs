using Google.Protobuf.Protocol;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public abstract class CreatureController : MonoBehaviour
{
    protected Animator _animator;
    protected SpriteRenderer _spriteRenderer;

    public int Id { get; set; }
    public string Name { get; set; }

    protected float _movecConstant;
    protected float _lastJumpTime;
    protected bool _isGrounded = true;

    PlayerState _playerState = new PlayerState();

    public virtual BaseState BaseState
    {
        get { return _playerState.BaseState; }
        set
        {
            if (_playerState.BaseState == value)
                return;

            _playerState.BaseState = value;
            if (BaseState == BaseState.Jump)
            {
                _lastJumpTime = Time.time;
                _isGrounded = false;
            }
                

            UpdateAnimation();
        }
    }

    public virtual MoveDir Dir
    {
        get { return PlayerState.MoveDir; }
        set
        {
            if (PlayerState.MoveDir == value) 
                return;

            PlayerState.MoveDir = value;
            UpdateAnimation();
        }
    }

    public virtual Vector2 Position
    {
        get { return new Vector2(PlayerState.PosX, PlayerState.PosY); }
        set
        {
            if (PlayerState.PosX == value.x && PlayerState.PosY == value.y)
                return;

            PlayerState.PosX = value.x;
            PlayerState.PosY = value.y;
            transform.position = (Vector3)value;

        }
    }

    public virtual PlayerState PlayerState
    {
        get { return _playerState; }
        set
        {
            if (_playerState.Equals(value))
                return;

            BaseState = value.BaseState;
            Dir = value.MoveDir;
            Position = new Vector2(value.PosX, value.PosY);

            UpdateAnimation();
        }
    }

    protected abstract void UpdateAnimation();

    protected virtual void UpdateBehavior()
    {
        switch (BaseState)
        {
            case BaseState.Idle:
                UpdateIdle();
                break;
            case BaseState.Moving:
                UpdateMoving();
                break;
            case BaseState.Skill:
                UpdateSkill();
                break;
            case BaseState.Dead:
                UpdateDead();
                break;
            case BaseState.Jump:
                UpdateJump();
                break;
        }
    }

    protected abstract void UpdateIdle();

    protected abstract void UpdateMoving();

    protected abstract void UpdateSkill();

    protected abstract void UpdateDead();

    protected abstract void UpdateJump();

    protected virtual void Init()
    {
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        UpdateAnimation();
    }

    void Start()
    {
        Init();
    }

    void Update()
    {
        UpdateBehavior();
    }
}
