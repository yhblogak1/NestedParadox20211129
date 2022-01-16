using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
// using UniRxSampleGame.Damages;
using UnityEngine;

namespace NestedParadox.Players
{
    // プレイヤの攻撃処理の管理コンポーネント
    public class PlayerAttack : MonoBehaviour
    {
        // 現在の攻撃力
        [SerializeField] private int _attackPower = 1;


        //外部参照
        private PlayerInput _playerinput;
        private PlayerAnimation _playerAnimation;
        private PlayerMove _playerMove;

        //購読用に用意する
        private readonly ReactiveProperty<bool> _isInAttack = new ReactiveProperty<bool>(false);

        // ヒエラルキー上で子要素として存在する攻撃判定用コライダ
        [SerializeField] private Collider2D _attackCollider1;
        [SerializeField] private Collider2D _attackCollider2;



        private void Start()
        {
            _isInAttack.AddTo(this);

            _playerinput = GetComponent<PlayerInput>();
            _playerAnimation = GetComponent<PlayerAnimation>();
            _playerMove = GetComponent<PlayerMove>();

            // 操作イベントの購読
            SubscribeInputEvent();

            // アニメーションイベントの購読
            SubscribeAnimationEvent();

            // 衝突イベントの購読
            SubscribeColliderEvent();

            // 攻撃中は移動不可フラグを立てる
            _isInAttack
                .Subscribe(x => _playerMove.BlockMove(x))
                .AddTo(this);

            OnAttackEndEvent();
        }

        #region 各種購読

        private void SubscribeInputEvent()
        {
            // 弱攻撃イベント
            _playerinput.OnNormalAttack
                // 接地中なら攻撃ができる
                .Where(_ => _playerMove.IsGrounded.Value)
                .Subscribe(_ => _playerAnimation.NormalAttack())
                .AddTo(this);

            // 強攻撃イベント
            _playerinput.OnChargeAttack
                // 接地中なら攻撃ができる
                .Where(_ => _playerMove.IsGrounded.Value)
                .Subscribe(_ => _playerAnimation.ChargeAttack())
                .AddTo(this);
        }

        // アニメーションイベントを購読する
        private void SubscribeAnimationEvent(){

        }


        // 各種衝突判定を購読する
        private void SubscribeColliderEvent(){
        }

        #endregion



        #region 攻撃の核となるメソッド

        // 攻撃モーションに合わせて当たり判定をON/OFFする
        public void OnNormalAttackEvent()
        {
            _attackCollider1.enabled = true;
            _attackPower = 1; // 弱攻撃は攻撃力1
        }

        public void OnChargeAttackEvent()
        {
            _attackCollider1.enabled = true;
            _attackCollider2.enabled = true;
            _attackPower = 2; // 強攻撃は攻撃力2
        }

        //当たり判定を消す
        public void OnAttackEndEvent()
        {
            _attackCollider1.enabled = false;
            _attackCollider2.enabled = false;
            _attackPower = 0;
        }

        #endregion

    }
}
