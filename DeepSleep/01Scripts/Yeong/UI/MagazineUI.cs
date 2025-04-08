using System.Text;
using TMPro;
using UnityEngine;
using YH.Players;

public class MagazineUI : MonoBehaviour
{
    [SerializeField] private PlayerManagerSO _playerManagerSO;
    private Player _player;
    private TextMeshProUGUI _magazineTxt;
    private StringBuilder _ammoTxt;
    private PlayerAttackCompo _attackCompo;
    private void Awake()
    {
        _playerManagerSO.SetUpPlayerEvent += HandlePlayerSetUp;
        _magazineTxt = GetComponentInChildren<TextMeshProUGUI>();
        _ammoTxt = new StringBuilder();
    }

    private void HandlePlayerSetUp()
    {
        _player = _playerManagerSO.Player;
        _attackCompo = _player.GetCompo<PlayerAttackCompo>();

        _attackCompo.FireEvent += HandleFireEvent;
        _attackCompo.UIReloadEvent += HandleReloadEvent;
        _ammoTxt.Append(_attackCompo.maxAmmo).Append(" / ").Append(_attackCompo.maxAmmo);
        _magazineTxt.text = _ammoTxt.ToString();
    }


    private void OnDestroy()
    {
        _playerManagerSO.SetUpPlayerEvent -= HandlePlayerSetUp;
        _player.GetCompo<PlayerAttackCompo>().FireEvent -= HandleFireEvent;
        _player.GetCompo<PlayerAttackCompo>().UIReloadEvent -= HandleReloadEvent;
    }
    private void HandleReloadEvent(int maxAmmo, bool reloading)
    {
        if (reloading)
        {
            _ammoTxt.Clear();
            _ammoTxt.Append("Reloading...");
            _magazineTxt.text = _ammoTxt.ToString();
        }
        else
        {
            _ammoTxt.Clear();
            _ammoTxt.Append(maxAmmo).Append(" / ").Append(maxAmmo);
            _magazineTxt.text = _ammoTxt.ToString();
        }
    }

    private void HandleFireEvent(int bulletMagazine, int maxAmmo)
    {
        _ammoTxt.Clear();
        _ammoTxt.Append(bulletMagazine).Append(" / ").Append(maxAmmo);
        _magazineTxt.text = _ammoTxt.ToString();
    }
}
