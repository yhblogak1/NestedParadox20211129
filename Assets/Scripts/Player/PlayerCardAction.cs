using UniRx;
using UnityEngine;
using NestedParadox.Cards;

namespace NestedParadox.Players
{
    public class PlayerCardAction : MonoBehaviour
    {
        // [SerializeField]NestedParadox.Cards.CardManager _cardmanager;
        private PlayerInput _playerinput;
        private PlayerCore _playercore;
        NestedParadox.Cards.CardManager _cardmanager;

        void Start()
        {
            //カードマネージャのキャッシュ
            _cardmanager = NestedParadox.Cards.CardManager.I;
            _playerinput = GetComponent<PlayerInput>();
            _playercore = GetComponent<PlayerCore>();

            _playerinput.OnPlayCard
            .Subscribe(_=> _cardmanager.Play())
            .AddTo(this);

            _playerinput.OnDrawCard
            .Where(_ => _playercore.PlayerDrawEnergy.Value ==10)//ドロエナジーの確認
            .Subscribe(_=> CardActionDraw())
            .AddTo(this);

            _playerinput.OnChangeHandR
            .Where(_ => _cardmanager.Hand.Count != 0)//手札があるときのみ実行
            .Subscribe(_=> _cardmanager.publicRotateHand(1))
            .AddTo(this);

            _playerinput.OnChangeHandL
            .Where(_ => _cardmanager.Hand.Count != 0)//手札があるときのみ実行
            .Subscribe(_=> _cardmanager.publicRotateHand(-1))
            .AddTo(this);
        }

        private void CardActionDraw(){
            _cardmanager.Draw();
            _playercore.ResetDrawEnergy();

        }
    }
}
