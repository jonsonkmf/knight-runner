using CodeBase.EnemyLogic;
using CodeBase.ScriptableObjects.GameVariables;
using Dreamteck.Splines;
using Sirenix.OdinInspector;
using Unity.Mathematics;
using UnityEngine;

public class DragonLifeTime1 : MonoBehaviour
{
    [SerializeField] private float _timeLife;

    [SerializeField] private SplineFollower _splineFollower;

    [SerializeField] private ParticleSystem _fireParticels;
    [SerializeField] private Collider _fireTrigger;
    [SerializeField] private GameObject _blackRoad;
    [SerializeField] private Transform _transformBone;
    private Transform _head;
    private float _startTime = 0;
    private float _blackRoadYPose;
    private int rayCount;
    private float timerscale;


    private void Start()
    {
//        Debug.Log(_spline.Value.gameObject.name);
        //_splineFollower.spline = _spline.Value;
        this.InvokeDelegate(() => { Destroy(this.gameObject); }, 15); //уничтожение врага по задержке
        _fireParticels.Stop();
        _head = _transformBone;
        _splineFollower.motion.offset = new Vector2(0, _splineFollower.motion.offset.y+0.5f);
        _splineFollower.motion.rotationOffset = new Vector3(45, 0, 0);
    }

    private void OnEnable()
    {
        _startTime = 0;
    }

    private void OnDisable() => _splineFollower.startPosition = 0;
    

    private void Update()
    {
        if (_splineFollower.startPosition != 0)
        {
            _startTime += Time.deltaTime;

            if (_startTime >= _timeLife)
            {
                _fireTrigger.enabled = false;
                _fireParticels.Stop();
                //Destroy(this.gameObject);

                if (_splineFollower.motion.rotationOffset.x > -60)
                    _splineFollower.motion.rotationOffset = new Vector3(
                        _splineFollower.motion.rotationOffset.x - Time.deltaTime * 20,
                        _splineFollower.motion.rotationOffset.y + Time.deltaTime * 15,
                        _splineFollower.motion.rotationOffset.z + Time.deltaTime * 10); /**/
                if (_splineFollower.motion.rotationOffset.x < -5)
                    _splineFollower.motion.offset =
                        new Vector2(_splineFollower.motion.offset.x + Time.deltaTime * 8,
                            _splineFollower.motion.offset.y + Time.deltaTime * 6);
                //_splineFollower.motion.rotationOffset = new Vector3()
            }
        }
        
        var offsetAngle = _splineFollower.motion.rotationOffset;
        var offset = _splineFollower.motion.offset;
        
        
        if (_startTime < 2)
        {
            if (offset.y>3.2)
            {
                _splineFollower.motion.offset = new Vector2(offset.x, offset.y-Time.deltaTime*2);
            }
            
            _splineFollower.followSpeed = 11.5f;
            
            if (offsetAngle.x>0)
            {
                _splineFollower.motion.rotationOffset = new Vector3(offsetAngle.x-Time.deltaTime*10, 0, 0);
            }
        }

        if (_startTime > 2)
        {
            if (offsetAngle.x>0)
            {
                _splineFollower.motion.rotationOffset = new Vector3(offsetAngle.x-Time.deltaTime*20, 0, 0);
            }
            

            if (offset.x<1)
            {
                _splineFollower.motion.offset = new Vector2(offset.x+Time.deltaTime, offset.y);
            }
            if (_splineFollower.followSpeed < 20)
            {
                _splineFollower.followSpeed += Time.deltaTime * 20;
            }
        }

        if (_startTime > 4 && _startTime < _timeLife)
        {
            _fireParticels.Play();
            _fireTrigger.enabled = true;
            rayCount++;
            if (rayCount % 3 == 0)
            {
                DoRaycast1();
            }
        }
    }


    private void DoRaycast1()
    {
        timerscale += Time.deltaTime*5;
//        Debug.Log($"Рейкаст");
        RaycastHit hit;
        Debug.DrawLine(_head.position, _head.position + _head.forward * 100, Color.red);
        Physics.Raycast(_head.position, _head.forward, out hit, 100, -1, QueryTriggerInteraction.Ignore);
        {
            // if (hit.collider.gameObject.TryGetComponent<SplineMesh>(out SplineMesh road))
            Debug.Log(hit);
            if (hit.collider != null)
            {
//                Debug.Log($"Рейкаст попал по коллайдеру {hit.collider.gameObject.name}");
                if (hit.collider.gameObject.TryGetComponent<SplineMesh>(out SplineMesh road))
                {
                    var instans = Instantiate(_blackRoad, new Vector3(hit.point.x, hit.point.y, hit.point.z), Quaternion.identity,
                        road.transform);

                    instans.transform.localScale = new Vector3(instans.transform.localScale.x - timerscale,
                        instans.transform.localScale.y, instans.transform.localScale.z);
                    _blackRoadYPose = hit.point.y;
                    //Debug.Log(hit.point.y);
                }

                else
                {
                    if (road != null)
                        Instantiate(_blackRoad, new Vector3(hit.point.x, _blackRoadYPose, hit.point.z),
                            Quaternion.identity, road.transform);
                }
            }
        }
    }
}