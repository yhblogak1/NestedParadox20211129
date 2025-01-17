using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NestedParadox.Players;
using UniRx;

namespace NestedParadox.Players{
    public class PlayerDummy : MonoBehaviour
    {
        [SerializeField] PlayerInput _playerinput;

        [SerializeField] GameObject Prefab;

        void Start(){
            _playerinput.OnDebug
            .Subscribe(_ => Test())
            .AddTo(this);
        }

        private void Test(){
            Prefab.SetActive(true);
            Prefab.GetComponent<ParticleSystem>().Play();
        }

        //メモゾーン


    }
}
