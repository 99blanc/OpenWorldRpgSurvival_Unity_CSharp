using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FieldOfView : MonoBehaviour
{
    [SerializeField] protected float viewAngle;
    [SerializeField] protected float viewDistance;
    [SerializeField] protected LayerMask targetMask;

    private NavMeshAgent NavMeshAgent;
    private ThirdPersonController ThirdPersonController;

    public void Start()
    {
        ThirdPersonController = FindObjectOfType<ThirdPersonController>();
    }

    public Vector3 GetTargetPosition()
    {
        return ThirdPersonController.transform.position;
    }

    public bool View()
    {
        Collider[] _target = Physics.OverlapSphere(transform.position, viewDistance, targetMask);

        for (int i = 0; i < _target.Length; i++)
        {
            Transform _targetPos = _target[i].transform;

            if (_targetPos.tag == "Player")
            {
                Vector3 _direction = (_targetPos.position - transform.position).normalized;
                float _angle = Vector3.Angle(_direction, transform.forward);

                if (_angle < viewAngle * 0.5f)
                {
                    RaycastHit _hit;

                    if (Physics.Raycast(transform.position + transform.up, _direction, out _hit, viewDistance))
                    {
                        if (_hit.transform.tag == "Player")
                        {
                            return true;
                        }
                    }
                }
            }
        }

        try
        {
            if (ThirdPersonController.GetSprint())
            {
                if (CalcPathLength(ThirdPersonController.transform.position) <= viewDistance)
                {
                    return true;
                }
            }

            return false;
        }
        catch (NullReferenceException)
        {

        }

        return false;
    }

    private float CalcPathLength(Vector3 targetPos)
    {
        NavMeshPath _path = new NavMeshPath();

        try
        {
            NavMeshAgent.CalculatePath(targetPos, _path);
        }
        catch (NullReferenceException)
        {

        }

        Vector3[] _wayPoint = new Vector3[_path.corners.Length + 2];

        _wayPoint[0] = transform.position;
        _wayPoint[_path.corners.Length + 1] = targetPos;

        float _pathLength = 0;

        for (int i = 0; i < _path.corners.Length; i++)
        {
            _wayPoint[i + 1] = _path.corners[i];
            _pathLength += Vector3.Distance(_wayPoint[i], _wayPoint[i + 1]);
        }

        return _pathLength;
    }
}
