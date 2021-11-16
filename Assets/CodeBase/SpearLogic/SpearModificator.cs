using ScriptableSystem.GameEvent;
using UnityEngine;

public class SpearModificator : MonoBehaviour
{
    [SerializeField] private int _maxCountModifier;
    [SerializeField] private GameObject[] _spears;
    [SerializeField] private int[] _numberChangeSpear;
    [SerializeField] private int[] _numberAddTrinkets;
    [SerializeField] private GameEvent _modifyEvent;
    [SerializeField] private float _multiply;
    [SerializeField] private GameObject[] _trinkets;

    private int _currentSpear;
    private int _numberTrinket;
    private int _countMultiply;

    private void Start()
    {
        _currentSpear = 0;
        _spears[_currentSpear].SetActive(true);
    }

    private void OnEnable() => _modifyEvent.AddAction(Upgrade);

    private void OnDisable() => _modifyEvent.RemoveAction(Upgrade);

    private void Upgrade()
    {
        if (_countMultiply > _maxCountModifier) return;
        
        var scale = this.gameObject.transform.localScale;
        _countMultiply++;
        TryChangeSpear();
        TryAddTrinkets();
            //Handheld.Vibrate();

        this.gameObject.transform.localScale = new Vector3(scale.x, scale.y, scale.z * _multiply);

    }

    private void TryChangeSpear()
    {
        if (_currentSpear==_numberChangeSpear.Length)
        {
            return;
        }
        if (_countMultiply >= _numberChangeSpear[_currentSpear])
        {
            _spears[_currentSpear].SetActive(false);
            _currentSpear++;
            _spears[_currentSpear].SetActive(true);
        }
    }

    private void TryAddTrinkets()
    {
        for (int i = 0; i < _numberAddTrinkets.Length; i++)
        {
            if (_countMultiply  == _numberAddTrinkets[i])
            {
                _trinkets[_numberTrinket].SetActive(true);
                _numberTrinket++;
            }
        }

    }
}