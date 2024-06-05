using System;
using System.Collections;
using UnityEngine;

namespace Dragoncraft
{
    [RequireComponent(typeof(SphereCollider), typeof(Rigidbody))]
    public class CollisionComponent : MonoBehaviour
    {
        private BaseCharacter _character;
        private SphereCollider _sphereCollider;
        private Rigidbody _rigidbody;
        private Coroutine _dealDamageCoroutine;
        private string _targetId;

        public Action<Transform> OnStartAttacking;
        public Action<Transform, bool> OnStopAttacking;

        public void Initialize(BaseCharacter character)
        {
            _character = character;
            _sphereCollider = GetComponent<SphereCollider>();
            _sphereCollider.radius = character.ColliderSize;
            _rigidbody = GetComponent<Rigidbody>();
            _rigidbody.isKinematic = true;
        }

        private void FixedUpdate()
        {
            if (_character != null && _character.IsDead)
            {
                _sphereCollider.radius = 0;
                return;
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.TryGetComponent<BaseCharacter>(out var opponent))
            {
                if (string.IsNullOrEmpty(_targetId))
                {
                    _targetId = opponent.ID;
                }

                if (_targetId.Equals(opponent.ID))
                {
                    OnStartAttacking(collision.transform);
                    _dealDamageCoroutine = StartCoroutine(TakeDamageOverTime(opponent));
                }
            }

            if (collision.gameObject.TryGetComponent<ProjectileComponent>(out var projectile))
            {
                collision.transform.gameObject.SetActive(false);
                TakeDamageFromProjectile(projectile.Damage, collision.transform);
            }
        }

        private void OnCollisionExit(Collision collision)
        {
            if (collision.gameObject.TryGetComponent<BaseCharacter>(out var opponent))
            {
                if (!string.IsNullOrEmpty(_targetId) && _targetId.Equals(opponent.ID))
                {
                    StopAttacking(collision.transform, opponent.IsDead);
                }
            }
        }

        private IEnumerator TakeDamageOverTime(BaseCharacter opponent)
        {
            while (!opponent.IsDead && !_character.IsDead)
            {
                float damage = _character.Attack - opponent.Defense;

                MessageQueueManager.Instance.SendMessage(
                    new DamageFeedbackMessage()
                    {
                        Damage = damage,
                        Position = opponent.GetDamageFeedbackPosition()
                    }
                );

                if (damage <= 0 || opponent.TakeDamage(damage))
                {
                    yield break;
                }

                yield return new WaitForSeconds(_character.AttackSpeed);
            }
        }

        private void TakeDamageFromProjectile(float opponentAttack, Transform target)
        {
            float damage = opponentAttack - _character.Defense;

            MessageQueueManager.Instance.SendMessage(
                new DamageFeedbackMessage()
                {
                    Damage = damage,
                    Position = _character.GetDamageFeedbackPosition()
                }
            );

            _character.TakeDamage(damage);
            StopAttacking(target, false);
        }

        private void StopAttacking(Transform target, bool opponentIsDead)
        {
            _targetId = null;

            if (_dealDamageCoroutine != null)
            {
                StopCoroutine(_dealDamageCoroutine);
                _dealDamageCoroutine = null;
            }

            OnStopAttacking(target, opponentIsDead);
        }
    }
}
