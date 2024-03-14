using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float _speed = 10f;

    Vector3 _destPos;

    private void Start()
    {
        Managers.Input.MouseAction -= OnMouseClicked;
        Managers.Input.MouseAction += OnMouseClicked;
    }

    public enum PlayerState
    {
        Die,
        Moving,
        Idle,
    }

    PlayerState _state = PlayerState.Idle;

    private void UpdateDie()
    {

    }

    private void UpdateMoving()
    {
        Vector3 dir = _destPos - transform.position;
        if (dir.magnitude < 0.0001f)
        {
            _state = PlayerState.Idle;
        }
        else
        {
            float moveDist = Mathf.Clamp(Time.deltaTime * _speed, 0, dir.magnitude);

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 20 * Time.deltaTime);
            transform.position += dir.normalized * moveDist;
        }

        // 애니메이션
        Animator anim = GetComponent<Animator>();
        anim.SetFloat("speed", _speed);
    }

    private void UpdateIdle()
    {
        // 애니메이션
        Animator anim = GetComponent<Animator>();
        anim.SetFloat("speed", 0);
    }

    private void Update()
    {
        switch (_state)
        {
            case PlayerState.Die:
                UpdateDie();
                break;
            case PlayerState.Moving:
                UpdateMoving();
                break;
            case PlayerState.Idle:
                UpdateIdle();
                break;
        }
    }

    private void OnMouseClicked(Define.MouseEvent evt)
    {
        if (_state == PlayerState.Die)
            return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        Debug.DrawRay(Camera.main.transform.position, ray.direction * 100f, Color.red, 1f);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100f, LayerMask.GetMask("Ground")))
        {
            _destPos = hit.point;
            _state = PlayerState.Moving;
        }
    }
}
