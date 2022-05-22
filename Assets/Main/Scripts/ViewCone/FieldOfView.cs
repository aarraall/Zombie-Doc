using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class FieldOfView : MonoBehaviour
{
    [SerializeField] bool initOnStart, drawCone;
    [SerializeField] private float _viewRadius; public float ViewRadius => _viewRadius;
    [SerializeField][Range(0, 360)] private float _viewAngle; public float ViewAngle { get => _viewAngle; set => _viewAngle = value; }
    [SerializeField] private LayerMask _targetLayer; public LayerMask TargetLayer => _targetLayer;
    [SerializeField] private LayerMask _obstacleLayer; public LayerMask ObstacleLayer => _obstacleLayer;

    [SerializeField] private float meshResolution;

    [SerializeField] private int edgeResolveIterations;

    [SerializeField] private float edgeDistanceThreshold;

    [SerializeField] private MeshFilter viewMeshFilter;

    private List<Transform> _visibleTargets = new List<Transform>(); public List<Transform> VisibleTargets => _visibleTargets;


    private bool _isExecuting; public bool IsExecuting => _isExecuting;
    private Mesh _viewMesh;

    public event Action<List<Transform>> OnTargetsFound;


    void Start()
    {
        if (!initOnStart) return;

        Initialize();
        Execute();
    }

    public void Initialize()
    {
        CreateMesh();
    }
    public void Initialize(float viewRadius, float viewAngle, LayerMask targetMask, LayerMask obstacleMask)
    {
        CreateMesh();
        Construct(viewRadius, viewAngle, targetMask, obstacleMask);
    }

    public void Execute()
    {
        gameObject.SetActive(true);
        _isExecuting = true;
    }

    public void Stop()
    {
        gameObject.SetActive(false);
        _isExecuting = false;
    }
    private void CreateMesh()
    {
        if (_viewMesh != null) return;

        _viewMesh = new Mesh();
        _viewMesh.name = "View Mesh";
        viewMeshFilter.mesh = _viewMesh;
    }

    private void Construct(float viewRadius, float viewAngle, LayerMask targetMask, LayerMask obstacleMask)
    {
        _viewRadius = viewRadius;
        _viewAngle = viewAngle;
        _targetLayer = targetMask;
        _obstacleLayer = obstacleMask;
    }
    private IEnumerator FindTargetsWithDelay(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            FindVisibleTargets();
        }
    }

    private void FixedUpdate()
    {
        if (!_isExecuting) return;
        FindVisibleTargets();
    }

    private void LateUpdate()
    {
        if (!drawCone) return;
        DrawFieldOfView();
    }
    private void FindVisibleTargets()
    {
        _visibleTargets.Clear();
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, _viewRadius, _targetLayer);

        for (int i = 0; i < targetsInViewRadius.Length; i++)
        {
            Transform target = targetsInViewRadius[i].transform;
            Vector3 dirToTarget = (target.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, dirToTarget) < _viewAngle / 2)
            {
                float dstToTarget = Vector3.Distance(transform.position, target.position);
                if (!Physics.Raycast(transform.position, dirToTarget, dstToTarget, _obstacleLayer))
                {
                    _visibleTargets.Add(target);
                }
            }
        }
        if (_visibleTargets.Count > 0)
            OnTargetsFound?.Invoke(_visibleTargets);

    }
    private void DrawFieldOfView()
    {
        int stepCount = Mathf.RoundToInt(_viewAngle * meshResolution);

        float stepAngleSize = _viewAngle / stepCount;

        List<Vector3> viewPoints = new List<Vector3>();
        ViewCastInfo oldViewCast = new ViewCastInfo();


        for (int i = 0; i <= stepCount; i++)
        {
            float angle = transform.eulerAngles.y - _viewAngle / 2 + stepAngleSize * i;
            ViewCastInfo newViewCast = ViewCast(angle);

            if (i > 0)
            {
                bool edgeDstThresholdExceeded = Mathf.Abs(oldViewCast.Distance - newViewCast.Distance) > edgeDistanceThreshold;
                if (oldViewCast.Hit != newViewCast.Hit || (oldViewCast.Hit && newViewCast.Hit && edgeDstThresholdExceeded))
                {
                    EdgeInfo edge = FindEdge(oldViewCast, newViewCast);
                    if (edge.PointA != Vector3.zero)
                    {
                        viewPoints.Add(edge.PointA);
                    }
                    if (edge.PointB != Vector3.zero)
                    {
                        viewPoints.Add(edge.PointB);
                    }
                }

            }


            viewPoints.Add(newViewCast.Point);
            oldViewCast = newViewCast;
        }

        int vertexCount = viewPoints.Count + 1;
        Vector3[] vertices = new Vector3[vertexCount];
        int[] triangles = new int[(vertexCount - 2) * 3];

        vertices[0] = Vector3.zero;
        for (int i = 0; i < vertexCount - 1; i++)
        {
            vertices[i + 1] = transform.InverseTransformPoint(viewPoints[i]);

            if (i < vertexCount - 2)
            {
                triangles[i * 3] = 0;
                triangles[i * 3 + 1] = i + 1;
                triangles[i * 3 + 2] = i + 2;
            }
        }

        _viewMesh.Clear();
        _viewMesh.vertices = vertices;
        _viewMesh.triangles = triangles;
        _viewMesh.RecalculateNormals();
    }

    private EdgeInfo FindEdge(ViewCastInfo minViewCast, ViewCastInfo maxViewCast)
    {
        float minAngle = minViewCast.Angle;
        float maxAngle = maxViewCast.Angle;
        Vector3 minPoint = Vector3.zero;
        Vector3 maxPoint = Vector3.zero;

        for (int i = 0; i < edgeResolveIterations; i++)
        {
            float angle = (minAngle + maxAngle) / 2;
            ViewCastInfo newViewCast = ViewCast(angle);

            bool edgeDstThresholdExceeded = Mathf.Abs(minViewCast.Distance - newViewCast.Distance) > edgeDistanceThreshold;
            if (newViewCast.Hit == minViewCast.Hit && !edgeDstThresholdExceeded)
            {
                minAngle = angle;
                minPoint = newViewCast.Point;
            }
            else
            {
                maxAngle = angle;
                maxPoint = newViewCast.Point;
            }
        }

        return new EdgeInfo(minPoint, maxPoint);
    }
    private ViewCastInfo ViewCast(float globalAngle)
    {
        Vector3 dir = GetDirectionFromAngle(globalAngle, true);
        RaycastHit hit;

        if (Physics.Raycast(transform.position, dir, out hit, _viewRadius, _obstacleLayer))
        {
            return new ViewCastInfo(true, hit.point, hit.distance, globalAngle);
        }
        else
        {
            return new ViewCastInfo(false, transform.position + dir * _viewRadius, _viewRadius, globalAngle);
        }
    }
    public Vector3 GetDirectionFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }


    #region Helper Structs
    public struct ViewCastInfo
    {
        private bool _hit; public bool Hit => _hit;
        private Vector3 _point; public Vector3 Point => _point;
        public float _distance; public float Distance => _distance;
        public float _angle; public float Angle => _angle;

        public ViewCastInfo(bool hit, Vector3 point, float distance, float angle)
        {
            _hit = hit;
            _point = point;
            _distance = distance;
            _angle = angle;
        }
    }
    public struct EdgeInfo
    {
        private Vector3 _pointA; public Vector3 PointA => _pointA;
        private Vector3 _pointB; public Vector3 PointB => _pointB;

        public EdgeInfo(Vector3 pointA, Vector3 pointB)
        {
            _pointA = pointA;
            _pointB = pointB;
        }
    }

    #endregion

}
