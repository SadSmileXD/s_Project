using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public GameObject MenuCam;
    public GameObject gameCam;
    public GameObject GameOverPanel;
    public Player player;
    public Boss boss;
    public int stage;
    public float playerTime;
    public bool isBattle;
    public int enemyCntA;
    public int enemyCntB;
    public int enemyCntC;
    public int enemyCntD;
    [Header("소비,장비,시작존")]
    public GameObject itemShop;
    public GameObject WeaponShop;
    public GameObject StartZone;

    public GameObject menuPanel;
    public GameObject GamePanel;
 
    public Text maxScoreTxt;
    public Text scoreTxt;
    public Text stageTxt;
    public Text playTimeTxt;
    public Text playerHealthTxt;
    public Text playerAmmo;
    public Text playerCoinTxt;
    public Text curScoreText;
    public Text bestScoreText;
    public Image weapon1Img;
    public Image weapon2Img;
    public Image weapon3Img;
    public Image weapon4Img;
    public Image weapon5Img;

    public Text enemyAtxt;
    public Text enemyBtxt;
    public Text enemyCtxt;

    public RectTransform bossHealthGroup;
    public RectTransform bossHealthBar;

    public Transform[] enemyzones;
    public GameObject[] enemies;

    public List<int> enemyList;
    // Update is called once per frame
    void Update()
    {
        

        if(isBattle)
        {
            playerTime += Time.deltaTime;
        }
          
    }

    //
    private void Awake()
    {
        enemyList= new List<int>();
        maxScoreTxt.text = string.Format("{0:n0}", PlayerPrefs.GetInt("Max Score Text"));  
    }

    //
    public void StageStart()
    {
        itemShop.SetActive(false);
        WeaponShop.SetActive(false);
        StartZone.SetActive(false);

        foreach(Transform item in enemyzones)
        {
             item.gameObject.SetActive(true);
        }
        isBattle = true;
        StartCoroutine(InBattle());
    }

    //
    public void StageEnd()
    {
        player.transform.position = Vector3.up * 0.8f;
        isBattle = false;
        itemShop.SetActive(true);
        WeaponShop.SetActive(true);
        StartZone.SetActive(true);
        foreach (Transform item in enemyzones)
        {
            item.gameObject.SetActive(false);
        }
        stage++;
    }
    //
    IEnumerator InBattle()
    {
        if(stage % 5 ==0)
        {
            enemyCntC++;
            GameObject instantEnemy = Instantiate(enemies[3], enemyzones[0].position+new Vector3(0,0,+20), enemyzones[0].rotation);
            var enemy = instantEnemy.GetComponent<Enemy>();
            boss= instantEnemy.GetComponent<Boss>();
            enemy.manager = this;
        }
        else
        {
            for (int index = 0; index < stage; index++)
            {
                int ran = UnityEngine.Random.Range(0, 3);
                enemyList.Add(ran);
                switch (ran)
                {
                    case 0:
                        enemyCntA++;
                        break;
                    case 1:
                        enemyCntB++;
                        break;
                    case 2:
                        enemyCntC++;
                        break;

                }

            }
            while (enemyList.Count > 0)
            {
                int ranZone = Random.Range(0, 4);
                GameObject instantEnemy = Instantiate(enemies[enemyList[0]], enemyzones[ranZone].position, enemyzones[ranZone].rotation);
                enemyList.RemoveAt(0);
                var enemy=instantEnemy.GetComponent<Enemy>();
                enemy.manager = this;
                yield return new WaitForSeconds(4f);
            }
        }
       
       while(enemyCntA+ enemyCntB+ enemyCntC+ enemyCntD>0)
        {
            yield return null;
        }
        yield return new WaitForSeconds(4f);
        boss = null; 
        StageEnd();

    }

    //
    public void GameStart()//버튼
    {//게임을 시작하는 버튼을 누르면 처음 UI비활성화 시키고
     //카메라 비활성화, 게임UI 활성화 및 플레이어 활성화
        MenuCam.SetActive(false);
        menuPanel.GetComponentInParent<Canvas>().gameObject.SetActive(false); 
        GamePanel.SetActive(true);
        player.gameObject.SetActive(true);
        
    }

    public void GameOver()
    {
        GamePanel.SetActive(false );
        GameOverPanel.SetActive(true);
        curScoreText.text=scoreTxt.text;

        int maxScore = PlayerPrefs.GetInt("Max Score Text");
        if(player.score > maxScore)
        {
            bestScoreText.gameObject.SetActive(true);
            PlayerPrefs.SetInt("Max Score Text",player.score);
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene(0);
        player.isDead = false;
    }
    //
    private void LateUpdate()
    {
        stageTxt.text = "STAGE " + stage;

        int hour = (int)(playerTime / 3600);
        int min = (int)((playerTime - hour *3600)/ 60);
        int second = (int)(playerTime % 60);
        playTimeTxt.text = string.Format("{0:00}", hour)+":"+
            string.Format("{0:00}", min) + ":" +
            string.Format("{0:00}", second);

        scoreTxt.text=string.Format("{0:n0}",player.score);

        playerHealthTxt.text = player.health + " / " + player.maxHealth;

        playerCoinTxt.text= string.Format("{0:n0}", player.coin);

        if(player._equipWeapon ==null)//무기 없을 때
        {
            playerAmmo.text = "- / " + player.ammo;

        }
        else if(player._equipWeapon.type == Weapon.Type.Melee)//근접 무기 일 때
        {
            playerAmmo.text = "- / " + player.ammo;
        }
        else//원거리 일때
        {
            playerAmmo.text = player._equipWeapon.curAmmo + " / " + player.ammo;
        }

        //UI에서 가지고 있는 것들만 알파값 올려서 보이게하기
        weapon2Img.color = new Color(1, 1, 1, player.hasWeapons[0] ? 1 : 0);
        weapon3Img.color = new Color(1, 1, 1, player.hasWeapons[1] ? 1 : 0);
        weapon4Img.color = new Color(1, 1, 1, player.hasWeapons[2] ? 1 : 0);
        weapon5Img.color = new Color(1, 1, 1, player.hasGrenades>0 ? 1 : 0);
         
        //적 인원 문자형을 변경
        enemyAtxt.text=enemyCntA.ToString();
        enemyBtxt.text=enemyCntB.ToString();
        enemyCtxt.text=enemyCntC.ToString();

        
        //보스 HP조절
        if(boss != null)
        {
            bossHealthGroup.anchoredPosition = Vector3.down * 30;
             bossHealthBar.localScale = new Vector3((float)boss.curHealth / boss.maxHealth, 1, 1);
        }
        else
        {
            bossHealthGroup.anchoredPosition = Vector3.up* 200;
        }
    }
}
