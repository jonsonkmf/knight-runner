using UnityEngine;

public class EnemyChangeShield : MonoBehaviour
{
    [SerializeField] private GameObject[] _shields = new GameObject[]{};
    public MeshRenderer CurrentShield;
    void Start()
    {
        var i = Random.Range(0, _shields.Length);
        _shields[i].SetActive(true);
        CurrentShield = _shields[i].GetComponent<MeshRenderer>();
    }


}
