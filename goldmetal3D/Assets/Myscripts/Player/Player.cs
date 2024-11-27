using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR;



public class Player : MonoBehaviour
{
    private delegate void  ItemAction(Item item);

    private Dictionary<Item.Type, ItemAction> _itemHandle;

    private Animator anim;
    private PlayerAnimaction playerAni;
    private Rigidbody rigid;

    public bool[] hasWeapons;
  
    public int hasGrenades;
 
    public int ammo;
    public int coin;
    public int health;
    public int maxAmmo;
    public int maxCoin;
    public int maxHealth;
    public int maxHasGrenades;
    private int _equipWeaponIndex = -1;

    private float hAxis;
    private float vAxis;
    private float speed=10;
    private float fireDelay;

    public  Vector3 moveVec;
    private Vector3 dodgeVec;

    public bool _wDown;
    private bool _jDown;
    private bool _isJump;
    private bool _isDodge;
    private bool _iDown;
    private bool _sDown1;
    private bool _sDown2;
    private bool _sDown3;
    private bool _isSwap;
    private bool _fDown;
    private bool isFireReady = true;
    private bool _rDown;
    private bool _isReload;
    private bool _isBorder;
    private bool _gDown;
    private bool _isDamage;

    private GameObject _nearObject;
    public GameObject[] grenades;
    public GameObject[] Weapons;
    public GameObject grenadeObj;

    private Weapon _equipWeapon;

    public CinemachineVirtualCamera [] virtualCamera;

    public Camera followCamera;

    MeshRenderer[] meshs;

    private void StopToWall()
    {
        Debug.DrawRay(transform.position, transform.forward * 5, Color.green);
        _isBorder = Physics.Raycast(transform.position, transform.forward, 5, LayerMask.GetMask("Wall"));
    }
    private void FixedUpdate()
    {
        rigid.angularVelocity = Vector3.zero;
        StopToWall();
    }
    private void Awake()
    {
        meshs = GetComponentsInChildren<MeshRenderer>();
          //cam = GetComponent<Camera[]>();
          playerAni = GetComponent<PlayerAnimaction>();
        

        anim =GetComponentInChildren<Animator>();

        rigid=GetComponent<Rigidbody>();

        _itemHandle = new Dictionary<Item.Type, ItemAction>
        {
            { Item.Type.Ammo, HandleAmmo },
            { Item.Type.Coin, HandleCoin },
            { Item.Type.Heart, HandleHeart },
            { Item.Type.Grenade, HandleGrenade }
        };
    }

    // Update is called once per frame
    void Update()
    {
    
        GetInput();
        Move();
        Jump();
        grenade();
        Dodge();
        Attack();
        Reload();
        CameraChange();//카메라 1,2,3 누르면 변환
        Turn();
        Interation();

        Swap();
    }

    private void grenade()
    {
        if (hasGrenades == 0)
            return;

        if(_gDown && !_isReload && !_isSwap)
        {
            Ray ray = followCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit rayHit;
            if (Physics.Raycast(ray, out rayHit, 100))
            {
                Vector3 nextVec = rayHit.point - transform.position;
                nextVec.y = 10;
                GameObject instanceGrenade = Instantiate(grenadeObj, transform.position, transform.rotation);
                Rigidbody rigidGenade= instanceGrenade.GetComponent<Rigidbody>();
                rigidGenade.AddForce(nextVec,ForceMode.Impulse);
                rigidGenade.AddTorque(Vector3.back * 10, ForceMode.Impulse);
                hasGrenades--;
                grenades[hasGrenades].SetActive(false);
            }
        }
    }
    private void Move()
    {
        moveVec = new Vector3(hAxis, 0, vAxis).normalized;
        if(_isDodge)
        {
            moveVec = dodgeVec;
        }
        if (_isSwap ||_isReload || !isFireReady)
            moveVec = Vector3.zero;
        
        if(!_isBorder)
        transform.position += moveVec * speed * (_wDown ? 0.5f : 2.0f) * Time.deltaTime;



        // anim.SetBool("isRun", moveVec != Vector3.zero);
        //anim.SetBool("isWalk", _wDown);
        playerAni.IsRun();
        playerAni.IsWalk();
    }

    private void Turn()
    {
        //  키보드에 의한 회전
        transform.LookAt(transform.position + moveVec);
        //마우스에 의한 회전
        if(_fDown)
        {
            Ray ray = followCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit rayHit;
            if (Physics.Raycast(ray, out rayHit, 100))
            {
                Vector3 nextVec = rayHit.point - transform.position;
                nextVec.y = 0;
                transform.LookAt(transform.position + nextVec);
            }
        }
        

    }


    void Attack()
    {
        if (_equipWeapon == null)
            return;

        fireDelay += Time.deltaTime;
        isFireReady = _equipWeapon.rate < fireDelay;
         if (_fDown && isFireReady && !_isDodge && !_isSwap)
        {
            _equipWeapon.Use();
            // anim.SetTrigger("doSwing");
            if (_equipWeapon.type == Weapon.Type.Melee)
            {
                playerAni.DoSwing(); 
            }
            else
            {
                playerAni.DoShut();

            }
                fireDelay = 0;
        }
          
       
        
    }
    private void GetInput()
    {
       

        //float
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");

        //bool
        _wDown = Input.GetButton("Walk");
        _jDown = Input.GetButtonDown("Jump");
        _iDown = Input.GetButtonDown("Interation");//e키 누르면 활성화
        _fDown = Input.GetButton("Fire1");//공격
        _rDown= Input.GetButton("Reload");//공격
        _gDown= Input.GetButtonDown("Fire2");
        //무기교체
        _sDown1 = Input.GetButtonDown("Swap1");
        _sDown2 = Input.GetButtonDown("Swap2");
        _sDown3 = Input.GetButtonDown("Swap3");
    }

    void Reload()
    {
        if (_equipWeapon == null)
            return;
        
        if (_equipWeapon.type == Weapon.Type.Melee)
            return;

        if (ammo == 0)
            return;

        if(_rDown && !_isJump && !_isDodge && !_isSwap && isFireReady)
        {
            playerAni.DoReload();
            _isReload = true;
            Invoke("ReloadOut", 3.0f);
        }
    }
    void ReloadOut()
    {
        int reAmmo = ammo < _equipWeapon.maxAmmo ? ammo : _equipWeapon.maxAmmo - _equipWeapon.curAmmo;
        _equipWeapon.curAmmo += reAmmo;
        ammo -= reAmmo;
        _isReload = false;
    }
    private void CameraChange()
    {
        //카메라 전환
        //if (Input.GetKey(KeyCode.Alpha1))
        //{
        //    cam[0].GetComponent<Camera>().enabled = true;
        //    cam[1].GetComponent<Camera>().enabled = false;
        //    cam[2].GetComponent<Camera>().enabled = false;
        //}
        //else if (Input.GetKey(KeyCode.Alpha2))
        //{
        //    cam[0].GetComponent<Camera>().enabled = false;
        //    cam[1].GetComponent<Camera>().enabled = true;
        //    cam[2].GetComponent<Camera>().enabled = false;
        //}
        //else if (Input.GetKey(KeyCode.Alpha3))
        //{
        //    cam[0].GetComponent<Camera>().enabled = false;
        //    cam[1].GetComponent<Camera>().enabled = false;
        //    cam[2].GetComponent<Camera>().enabled = true;
        //}
    }

    void Jump()
    {
        // 점프 버튼을 누르고 , 멈춘상태이고 , 점프가 아닌상태이고 ,isDodge 아닌상태만
        if (_jDown &&  Vector3.zero== moveVec  && !_isJump && !_isDodge && !_isSwap)
        {
            rigid.AddForce(Vector3.up * 15, ForceMode.Impulse);
            anim.SetBool("isJump", true);
            anim.SetTrigger("DoJump");
            _isJump = true;
        }
    }

    void Dodge()
    {
        //  점프고 움직이는 상태이고 , 점프가 아니고 isDodge상태가 아닌경우
        if (_jDown && moveVec !=Vector3.zero && !_isJump && !_isDodge && !_isSwap)
        {
            dodgeVec = moveVec;
            speed *= 2;
            //anim.SetTrigger("DoDodge");
            playerAni.DoDodge();
            _isDodge = true;

            Invoke("DodgeOut", 0.6f);//Invoke (함수이름,딜레이시간)
         
        }
    }
    void DodgeOut()
    {
        speed *= 0.5f;
        _isDodge = false;
    }

  

    void Swap()
    {
        bool flag = IsSwap();//IsSwap(ref _sDown1, ref _sDown2,ref  _sDown3);
        if (!flag) return;

        int weaponindex = _sDown1 ? 0 : _sDown2 ? 1 : _sDown3 ? 2 : -1;


        if ((_sDown1 || _sDown2 || _sDown3) && !_isJump && !_isDodge)
        {
            if (_equipWeapon != null)
              
                _equipWeapon.gameObject.SetActive(false);

            _equipWeapon = Weapons[weaponindex].GetComponent<Weapon>();
            _equipWeaponIndex = weaponindex;
            _equipWeapon.gameObject.SetActive(true);
            // anim.SetTrigger("DoSwap");
            playerAni.DoSwap();
            _isSwap = true;

            Invoke("SwapOut", 0.4f);
        }
    }
    void SwapOut()
    {

        _isSwap = false;
    }

    private bool IsSwap(ref bool sDown1 , ref bool sDown2, ref bool sDown3)
    {//이미 들고 있는 무기 이거나 없는 무기 이면 false
        bool[] swapKeys = { sDown1, sDown2, sDown3 };
       
        for (int i = 0; i < swapKeys.Length; i++)
        {
            if (swapKeys[i] && (!hasWeapons[i] || _equipWeaponIndex == i))
                return false;
        }
        return true;
    }
   
     private bool IsSwap()
    {//이미 들고 있는 무기 이거나 없는 무기 이면 false
        bool[] swapKeys = { _sDown1, _sDown2, _sDown3 };
       
        for (int i = 0; i < swapKeys.Length; i++)
        {
            if (swapKeys[i] && (!hasWeapons[i] || _equipWeaponIndex == i))
                return false;
        }
        return true;
    }


    void Interation()
    {
        if (_iDown && _nearObject != null  &&  !_isJump && !_isDodge)
        {
            if(_nearObject.tag =="Weapon")
            {
                Item item = _nearObject.GetComponent<Item>();
                int weaponidex = item.value;
                hasWeapons[weaponidex] = true;
                Destroy(_nearObject);
            }
        }

    }
    private void HandleAmmo(Item item)
    {
        ammo += item.value;
        if (ammo > maxAmmo)
            ammo = maxAmmo;
    }

    private void HandleCoin(Item item)
    {
        coin += item.value;
        if (coin > maxCoin)
            coin = maxCoin;
    }

    private void HandleHeart(Item item)
    {
        health += item.value;
        if (health > maxHealth)
            health = maxHealth;
    }

    private void HandleGrenade(Item item)
    {
        if (hasGrenades == maxHasGrenades)
            return;

        grenades[hasGrenades].SetActive(true);
        hasGrenades += item.value;
    }
    private void OnTriggerEnter(Collider other)
    {
      
        if (other.tag == "Item")
        {
            Item item = other.GetComponent<Item>();
            if (item != null && _itemHandle.TryGetValue(item.type, out ItemAction handler))
            {
                handler(item);
                Destroy(other.gameObject);
            }
        }
        else if(other.tag == "EnemyBullet")
        {
            if(!_isDamage)
            {
                Bullet enemyBullet = other.GetComponent<Bullet>();
                health -= enemyBullet.damage;

                bool isBooAtk = other.name == "Boss melee Area";
                StartCoroutine(OnDamage(isBooAtk));
            }
            if (other.GetComponent<Rigidbody>() != null)
            {
                Destroy(other.gameObject);
            }

        }
    }
    IEnumerator OnDamage(bool isBooAtk)
    {// 무적 시간
        _isDamage = true;
        foreach(var mesh in meshs)
        {
            mesh.material.color = Color.yellow;
        }

        if(isBooAtk)
        {
            rigid.AddForce(transform.forward*-25,ForceMode.Impulse);
        }
        yield return new WaitForSeconds(1f);
        if (isBooAtk)
        {
            rigid.velocity = Vector3.zero;  
        }
            _isDamage = false;
        foreach (var mesh in meshs)
        {
            mesh.material.color = Color.white;
        }

    }
    private void OnCollisionEnter(Collision collision)
    {
       
        //충돌 했을 때
        if (collision.gameObject.tag == "Floor") //충돌한 넘이 바닥이면
        {
            anim.SetBool("isJump", false);//애니메이션 파라미터 false로 전환하고
            _isJump = false;//점프상태 false값으로
        }
     
    }

    private void OnTriggerExit(Collider other)
    {
        if ("Weapon" == other.tag)
        {
            _nearObject = null;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if ("Weapon" == other.tag)
        {
            _nearObject = other.gameObject;
            Debug.Log(_nearObject.name);
        }
         
    }

  
}
