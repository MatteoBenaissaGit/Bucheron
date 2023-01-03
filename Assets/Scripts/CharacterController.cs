using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UIElements;

namespace DefaultNamespace
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(SpriteView))]
    public class CharacterController : MonoBehaviour
    {
        [SerializeField, Range(0,20)] private float _speed = 5;
        [SerializeField, Range(0,1)] private float _deceleration = 0.5f;
        [SerializeField, Range(0,1)] private float _acceleration = 0.5f;
        [SerializeField, Range(0, 3)] private float _timeToSetAttackBoolean = 1.5f;
        
        [ReadOnly] public bool CanMove = true;
        [ReadOnly] public bool CanAttack = true;
        [ReadOnly] public bool IsAttacking;
        private Vector2 _inputs;
        private Rigidbody2D _rigidbody;
        private SpriteView _spriteView;
        private CharacterWater _characterWater;
        private Animator _dustAnimator;

        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _spriteView = GetComponent<SpriteView>();
            _dustAnimator = GetComponent<Animator>();
            _characterWater = GetComponent<CharacterWater>();
            
            _spriteView.OnActionEnd.AddListener(EndAction);
        }

        private void OnDestroy()
        {
            _spriteView.OnActionEnd.RemoveListener(EndAction);
        }

        private void Update()
        {
            Attack();
            CheckAttackDestructible();
        }

        private void FixedUpdate()
        {
            HandleMovementInputs();
            ApplyAnimation();
            ApplyMovement();
        }

        private void ApplyMovement()
        {
            float speed = CanMove && _inputs.magnitude > 0.1f ? _speed : 0;
            _rigidbody.velocity = _inputs.normalized * speed;
        }

        private void HandleMovementInputs()
        {
            //get raw inputs
            float rawInputX = Input.GetAxisRaw("Horizontal");
            float rawInputY = Input.GetAxisRaw("Vertical");
            //lerp current value toward raw
            float lerpValue = Mathf.Abs(rawInputX) + Mathf.Abs(rawInputY) < 0.1f ? _deceleration : _acceleration;
            float lerpX = Mathf.Lerp(_inputs.x, rawInputX, lerpValue);
            float lerpY = Mathf.Lerp(_inputs.y, rawInputY, lerpValue);
            //assign input value
            _inputs = new Vector2(lerpX, lerpY);
        }
        
        private void ApplyAnimation()
        {
            _spriteView.PlayState(_rigidbody.velocity.magnitude > 0.1f ? "Walk" : "Idle");
            _dustAnimator.SetTrigger(_rigidbody.velocity.magnitude > 0.1f ? "Launch" : "Stop");
            
            //side
            if (_inputs.magnitude > 0.1f && IsAttacking == false)
            {
                Vector3 scale = transform.localScale;
                const float lerpForce = 0.15f;
                float scaleX = Mathf.Lerp(scale.x, _inputs.x < 0 ? -1 : 1, lerpForce);
                transform.localScale = new Vector3(scaleX, scale.y, scale.z);
            }
            else if (IsAttacking || transform.localScale.x < 0.99f)
            {
                Vector3 scale = transform.localScale;
                const float lerpForce = 0.3f;
                float scaleX = Mathf.Lerp(scale.x, Mathf.Sign(scale.x) < 0 ? -1 : 1, lerpForce);
                transform.localScale = new Vector3(scaleX, scale.y, scale.z);
            }
        }

        private void Attack()
        {
            if (Input.GetMouseButtonDown(0) && CanMove && IsAttacking == false && CanAttack && _characterWater.IsWatering == false)
            {
                transform.DOScaleX(1 * Mathf.Sign(transform.localScale.x), 0.25f);
                _spriteView.PlayAction("Axe");
                CanMove = false;
                StartCoroutine(SetAttack());
            }
        }

        private IEnumerator SetAttack()
        {
            yield return new WaitForSeconds(_timeToSetAttackBoolean);
            IsAttacking = true;
        }

        private void EndAction()
        {
            IsAttacking = false;
            CanMove = true;
        }

        private void CheckAttackDestructible()
        {
            if (IsAttacking == false)
            {
                return;
            }
            
            Vector2 origin = transform.position + new Vector3((transform.localScale.x), 0.25f, 0);
            Vector2 size = new Vector2(2, 1f);
            RaycastHit2D[] raycastHits = Physics2D.BoxCastAll(origin, size, 0, transform.forward, Single.PositiveInfinity);

            foreach (RaycastHit2D raycastHit in raycastHits)
            {
                Destructible destructible = raycastHit.collider.gameObject.GetComponent<Destructible>();
                if (destructible != null)
                {
                    destructible.TakeDamage();
                    IsAttacking = false;
                }
            }
        }
        
#if UNITY_EDITOR

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Vector2 origin = transform.position + new Vector3((transform.localScale.x), 0.25f, 0);
            Gizmos.DrawWireCube(origin, new Vector3(2,0.25f,1));
        }

#endif
    }
}