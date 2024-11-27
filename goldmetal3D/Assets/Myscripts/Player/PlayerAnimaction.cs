using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimaction : MonoBehaviour
{
    /// <summary>
    /// 플레이어 애니메이션을 담당하는 스크립트 입니다.
    /// 플레이어 스크립트 부분에서 작동예정
    /// </summary>
    private Animator anim;
    private Player player;
    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        player = GetComponent<Player>();
    }

    public void IsRun()
    {
        anim.SetBool("isRun", player.moveVec!= Vector3.zero);
    }

    public void IsWalk()
    {
        anim.SetBool("isWalk", player._wDown);
    }
    public void DoSwing()
    {
        anim.SetTrigger("doSwing");
    }
    public void DoSwap()
    {
        anim.SetTrigger("DoSwap");
    }
    public void DoDodge()
    {
        anim.SetTrigger("DoDodge");
    }
    public void DoShut()
    {
        anim.SetTrigger("doShot");
    }
    public void DoReload()
    {
        anim.SetTrigger("doReload");
    }

   
}
