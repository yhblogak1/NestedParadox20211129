using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using Cysharp.Threading.Tasks;

namespace NestedParadox.Monsters
{
    public class SniperK : MonsterBase, IApplyDamage
    {
        [SerializeField] Animator animator;
        [SerializeField] GameObject bulletPrefab;
        [SerializeField] GameObject explosionPrefab;
        [SerializeField] float shotPower;
        [SerializeField] Vector3 shotPosition;
        [SerializeField] float attackSpan;
        private float attackTime;
        // Start is called before the first frame update
        void Start()
        {
            attackTime = 0;            
        }

        // Update is called once per frame
        void Update()
        {
            attackTime += Time.deltaTime;
            if(attackTime > attackSpan)
            {
                Attack();
            }
        }

        public override void Damaged(int damage)
        {
            hp -= damage;
        }

        private void Shot()
        {
            int[] randomAngles = {Random.Range(0, 91), Random.Range(0, 91)};           
            foreach(int randomAngle in randomAngles)
            {     
                Vector3 shotVector = new Vector3(shotPower * Mathf.Cos(randomAngle), shotPower * Mathf.Sin(randomAngle), 0);
                GameObject bullet_clone = Instantiate(bulletPrefab, transform.position + shotPosition, Quaternion.identity);
                bullet_clone.GetComponent<Rigidbody2D>().AddForce(shotVector);
                bullet_clone.GetComponent<Collider2D>().OnTriggerEnter2DAsObservable()
                                                       .Subscribe(other =>
                                                       {
                                                            GameObject explosion_clone = Instantiate(explosionPrefab, bullet_clone.transform.position, Quaternion.identity);
                                                            explosion_clone.GetComponent<Collider2D>().OnTriggerEnter2DAsObservable()
                                                                                                      .Subscribe(other =>
                                                                                                      {
                                                                                                          EnemyBase enemy;
                                                                                                          other.TryGetComponent<EnemyBase>(out enemy);
                                                                                                          if (enemy != null)
                                                                                                          {
                                                                                                              enemy.Damaged(attackValue);
                                                                                                          }
                                                                                                      })
                                                                                                      .AddTo(explosion_clone);                                                             
                                                       })
                                                       .AddTo(bullet_clone);
            }            
        }

        private async void Attack()
        {
            animator.SetTrigger("AttackTrigger");
            await UniTask.WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).length > 1.18f);
            Shot();
            attackTime = 0;
        }
    }
}