using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimaction : MonoBehaviour
{
    /// <summary>
    /// �÷��̾� �ִϸ��̼��� ����ϴ� ��ũ��Ʈ �Դϴ�.
    /// �÷��̾� ��ũ��Ʈ �κп��� �۵�����
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
