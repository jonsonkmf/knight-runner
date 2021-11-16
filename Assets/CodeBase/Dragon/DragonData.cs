using ScriptableSystem;
using Sirenix.OdinInspector;
using UnityEngine;

[InlineEditor()]
[CreateAssetMenu(
    menuName = SOConstants.DataSubmenu + "DragonAttack",
    fileName = "DragonAttackData",
    order = SOConstants.AssetMenuOrder)]
public class DragonData: ScriptableObject
{
    [Range(0.0f, 1.0f)] 
    [SerializeField] private int _scoreCount;
    [SerializeField] private GameObject _template;

    public int scoreCount => _scoreCount;

    public GameObject template => _template;
}