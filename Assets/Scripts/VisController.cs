using System.Collections;
using UnityEditor.MemoryProfiler;
using UnityEngine;

public class VisController : MonoBehaviour
{
    public float lowProfile, highProfile, rotationSpeed;
    private float _currentProfile, _tilt;
    private int _idleActive, _walkActive, _runActive, _attackActive;
    private bool _recharging;
    private Animator _animator;
    public GameObject projectile;
    public AudioClip shot, blast;

    private void Start()
    {
        _animator = GetComponent<Animator>();

        _idleActive = Animator.StringToHash("IdleActive");
        _walkActive = Animator.StringToHash("WalkActive");
        _runActive = Animator.StringToHash("RunActive");
        _attackActive = Animator.StringToHash("AttackActive");
    }

    private void FixedUpdate()
    {
        MainProfiles();
        AttackMode();
        
        // Tilt camera left and right.
        //_tilt += rotationSpeed * Input.GetAxisRaw("Mouse X");
        //transform.eulerAngles = new Vector3(0, _tilt, 0);
    }

    private void AnimationState(bool idle, bool walk, bool run, bool attack)
    {
        _animator.SetBool(_idleActive, idle);
        _animator.SetBool(_walkActive, walk);
        _animator.SetBool(_runActive, run);
        _animator.SetBool(_attackActive, attack);
    }

    private void MainProfiles()
    {
        // allow character to walk (using y axis keys).
        var z = Input.GetAxis("Vertical") * _currentProfile;
        transform.Translate(0, 0, z);
        // allow character to rotate (using y axis keys).
        var y = Input.GetAxis("Horizontal") * rotationSpeed;
        transform.Rotate(0, y, 0);

        var forwardPressed = Input.GetKey(KeyCode.W);
        var upArrow = Input.GetKey(KeyCode.UpArrow);
        var hpPressed = Input.GetKey(KeyCode.LeftShift);

        if (!hpPressed)
        {
            if (forwardPressed || upArrow)
                AnimationState(false, true, false, false); // walk
            else
                AnimationState(true, false, false, false); // idle

            _currentProfile = lowProfile;
        }
        else
        {
            if (forwardPressed || upArrow)
                AnimationState(false, false, true, false); // run
            else
                AnimationState(true, false, false, false); // idle

            _currentProfile = highProfile;
        }
    }

    private void AttackMode()
    {
        if (Input.GetKey(KeyCode.Mouse0) && !_recharging)
        {
            AnimationState(false, false, false, true);
            StartCoroutine(ShootAndRecharge());
        }
    }

    private IEnumerator ShootAndRecharge()
    {
        // shoot projectile.
        yield return new WaitForSeconds(0.85f);
        projectile.SetActive(true);
        // recharge.
        yield return new WaitForSeconds(0.4f);
        _recharging = true;
        print("Recharging...");
        // destroy projectile.
        yield return new WaitForSeconds(1.5f);
        projectile.SetActive(false);
        _recharging = false;
        print("Recharged!");
    }
}