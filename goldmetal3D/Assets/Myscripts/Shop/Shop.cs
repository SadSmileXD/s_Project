using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Shop : MonoBehaviour
{
    public RectTransform UIGroup;
    public Animator anim;
    Player enterPlayer;
    public string[] talkData;
    public GameObject[] itemObj;
    public int[] itemPrice;
    public Transform[] itemPos;
    public Text talkText;
    public GameObject Panel;
    public void Enter(Player player )
    {
        Panel.SetActive( false );
        enterPlayer = player;
        UIGroup.anchoredPosition = Vector3.zero;
    }
    public void Exit()
    {
        Panel.SetActive(true);//리 게임 할때 Pannel 땜에 상점 버튼 안눌림
        anim.SetTrigger("doHello");
        UIGroup.anchoredPosition = Vector3.down *10000;
    }
    public void Buy(int index)
    {
        int price = itemPrice[index];
        if (price >enterPlayer.coin)
        {
            StopCoroutine(Talk());
            StartCoroutine(Talk());
            return;
        }
        enterPlayer.coin -= price;
        Vector3 ranVec = Vector3.right * Random.Range(-3, 3)
                         + Vector3.forward * Random.Range(-3, 3);
        Instantiate(itemObj[index], itemPos[index].position + ranVec, itemPos[index].rotation);

    }
    IEnumerator Talk()
    {
        talkText.text =talkData[1];
        yield return new WaitForSeconds(2f);
        talkText.text = talkData[0];
    }
}
