﻿/*
    게임 버튼 매니저
    버튼을 눌러서 동작하는 기능들이 들어 있는 스크립트
    아이템 사용, 점프, 일시정지 버튼이 여기 해당함
                -by 이희찬
*/
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class IngameButtonManager : MonoBehaviour
{
    private static IngameButtonManager instance = null;
    public static IngameButtonManager Instance
    {
        get
        {
            if (instance)
                return instance;
            else
                return instance = new GameObject("*Manager").AddComponent<IngameButtonManager>();
        }
    }

    private void Awake()
    {
        instance = this;
    }

    [SerializeField]
    private JoystickController m_joystickController;
    [SerializeField]
    private GameObject PauseWindow;
    [SerializeField]
    private GameObject clearWindow;
    [SerializeField]
    private GameObject Buttons;
    [SerializeField]
    private GameObject JumpActionButton;
    [SerializeField]
    private GameObject Joystick;
    [SerializeField]
    private GameObject Item1;
    [SerializeField]
    private GameObject Item2;
    [SerializeField]
    private GameObject LeftHand;
    [SerializeField]
    private GameObject OptionButton;
    
    [SerializeField]
    private Sprite Jump;
    [SerializeField]
    private Sprite Action;
  

    public Vector3 playerposition;
    public int Itemusenum;
    public int Inkusenum;
    public int Blackusenum;
    public int crashnum;
    private int m_whatitem = 0;
    private bool m_JumpActionButton = false;
    private bool m_ItemButton1 = false;
    private bool m_ItemButton2 = false;
    private bool m_PauseButton = false;
    private bool m_istouchbutton = false;
    private bool m_isaction = false;
    private bool m_ItemButton1active = false;
    private bool m_ItemButton2active = false;
    private bool m_isitemUse = false;
    private bool m_isitemUsed = false;
    private bool m_isHavewhite = false;
   
    public bool canAction
    {
        set
        {
            m_isaction = value;
        }
    }
    public bool IshaveWhite
    {
        get
        {
            return m_isHavewhite;
        }
        set
        {
            m_isHavewhite = value;
        }
    }
    public int WhatItemUse
    {
        get
        {
            return m_whatitem;
        }
       
        
    }


    [SerializeField]
    private PlayerManager playerManager;
    [SerializeField]
    private InGameManager ingameManager;
    private PlayerAnimation playerstatus;
    private void Update()
    {
        playerposition = playerManager.gameObject.transform.position;
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Ison_pausebutton();
           
        }

    }
    void Start()
    {
        Itemusenum = 0;
        Inkusenum = 0;
        Blackusenum = 0;
        crashnum = 0;
        m_whatitem = 0;
        m_JumpActionButton = false;
        m_ItemButton1 = false;
        m_ItemButton2 = false;
        m_PauseButton = false;
        m_istouchbutton = false;
        m_isaction = false;
        m_ItemButton1active = false;
        m_ItemButton2active = false;
        m_isitemUse = false;
        m_isitemUsed = false;
        m_isHavewhite = false;
        playerstatus = playerManager.gameObject.GetComponent<PlayerAnimation>();
        init_buttonPos();
        var itemStream = Observable.EveryUpdate()
                .Where(_ => (Input.GetMouseButtonUp(0)|| Input.GetMouseButtonUp(1)) && 
                            m_whatitem !=0 &&
                            !m_PauseButton);
        var clickStream = Observable.EveryUpdate()
         .Where(_ => m_istouchbutton);
        var isAction = Observable.EveryUpdate();
        // 이미지 변경
        isAction
            .Where(_ => m_isaction)
            .Subscribe(_ =>
            {
                JumpActionButton.GetComponent<Image>().sprite = Action;
            });
        isAction
            .Where(_ => !m_isaction)
            .Subscribe(_ =>
            {
                JumpActionButton.GetComponent<Image>().sprite = Jump;
            });
        itemStream
            .Where(_=> !m_isitemUsed)
            .Subscribe(_ =>
            {
                ItemUse();
            });


        // 점프 , 액션 버튼 눌렀을때
        clickStream
            .Where(_ => m_JumpActionButton)
            .Subscribe(_ => {

                if (m_isaction)
                {

                   playerManager.Interaction();

             
                  

                }
                else if(playerstatus.status == PlayerState.Move && playerManager.IsGround == true)
                {
                    playerManager.Jump();
                 
                }
                else if (playerstatus.status == PlayerState.Idle && playerManager.IsGround == true)
                {
                    playerManager.Jump();
                  
                }
                m_JumpActionButton = false;
                m_istouchbutton = false;
            });

        // 아이템1 버튼 눌렀을때
        clickStream
         .Where(_ => m_ItemButton1)
         .Subscribe(_ => {
             //item1
             if (!m_ItemButton1active) // 활성화
             {
                 PlayerPrefs.SetInt(Prefstype.Item1Use, 1);
                 Joystick.SetActive(false);
                 JumpActionButton.SetActive(false);
                 Item1.SetActive(false);
                 Item2.SetActive(false);
                 OptionButton.SetActive(false);
                 m_ItemButton1active = true;
                 m_whatitem = 1;
             }
         
             m_ItemButton1 = false;
             m_istouchbutton = false;
         });

        // 아이템2 버튼 눌렀을때
        clickStream
         .Where(_ => m_ItemButton2)
         .Subscribe(_ => {
             //item2
             if (!m_ItemButton2active)
             {
                 PlayerPrefs.SetInt(Prefstype.Item2Use, 1);
                 Joystick.SetActive(false);
                 JumpActionButton.SetActive(false);
                 Item1.SetActive(false);
                 Item2.SetActive(false);
                 OptionButton.SetActive(false);
                 m_ItemButton2active = true;
                 m_whatitem = 2;
             }
       

             m_ItemButton2 = false;
             m_istouchbutton = false;
         });
        // 일시정지 버튼 눌렀을때
        clickStream
         .Where(_ => m_PauseButton)
         .Subscribe(_ => {

             PauseWindow.SetActive(true);
             Buttons.SetActive(false);
             Time.timeScale = 0;
             m_istouchbutton = false;
         });
        // 게임으로 돌아가기
        clickStream
         .Where(_ => !m_PauseButton)
         .Subscribe(_ => {
            
             PauseWindow.SetActive(false);
             Buttons.SetActive(true);
             m_istouchbutton = false;
             Time.timeScale = 1;
            
         });



       
    }
    
    private void ItemUse()
    {

        // 아이템 사용되는곳
        if (m_whatitem !=0)
        {
        
    
            //item1
            if (m_whatitem == 1 && m_isitemUse)  //item1 use
            {
               
                ItemUsed();
            }
            else if (m_whatitem == 2 && m_isitemUse) // item2 use
            {
                Inkusenum++;
                ItemUsed();
            }
            else if (!m_isitemUse)
                ItemUsed();
        }

    }
    public void useInk()
    {

    }
    public void ItemUsed()
    {

        SoundManager.instance.PlaySound();
        m_isitemUse = false;
        m_ItemButton1active = false;
        m_ItemButton2active = false;
        Joystick.SetActive(true);
        Item1.SetActive(true);
        Item2.SetActive(true);
        OptionButton.SetActive(true);
        JumpActionButton.SetActive(true);
        m_whatitem = 0;
        m_joystickController.InitPos();
        Itemusenum ++;
    }
    public void Ison_jumpaction()
    {
        m_JumpActionButton = true;
        m_istouchbutton = true;
    }
    public void Ison_item1()
    {
        SoundManager.instance.PlaySound();
        m_ItemButton1 = true;
        m_istouchbutton = true;

    }
    public void Ison_item2()
    {
        SoundManager.instance.PlaySound();
        m_ItemButton2 = true;
        m_istouchbutton = true;

    }
    public void Ison_pausebutton()
    {
        SoundManager.instance.PlaySound();
        m_PauseButton = true;
        m_istouchbutton = true;
        ItemUsed();
    }
    public void Ison_closepausebutton()
    {
        SoundManager.instance.PlaySound();
        m_PauseButton = false;
        m_istouchbutton = true;
        m_joystickController.InitPos();
    }
    public void Ison_retrybutton()
    {
        SoundManager.instance.PlaySound();
        Time.timeScale = 1;
        PlayerPrefs.SetInt(Prefstype.IsToMain, 0);
        SceneChange.Change(SceneType.Loading);
    }
    public void Ison_tomainscene()
    {
        SoundManager.instance.PlaySound();
        Time.timeScale = 1;
        PlayerPrefs.SetInt(Prefstype.IsToMain, 1);
        SceneChange.Change(SceneType.Loading);
    }
    public void ClearWindow()
    {
        Time.timeScale = 0;
        
        clearWindow.SetActive(true);
        Buttons.SetActive(false);
    }
    private void init_buttonPos()
    {
        Joystick.transform.localPosition = 
            new Vector3(PlayerPrefs.GetInt(Prefstype.JoystickxPos, -624),
                        PlayerPrefs.GetInt(Prefstype.JoystickyPos, -284), 0.0f);
        JumpActionButton.transform.localPosition = 
            new Vector3(PlayerPrefs.GetInt(Prefstype.JumpButtonxPos, 634),
                        PlayerPrefs.GetInt(Prefstype.JumpButtonyPos, -300), 0.0f);
        Item1.transform.localPosition = 
            new Vector3(PlayerPrefs.GetInt(Prefstype.Item1xPos, 439),
                        PlayerPrefs.GetInt(Prefstype.Item1yPos, -175), 0.0f);
        Item2.transform.localPosition = 
            new Vector3(PlayerPrefs.GetInt(Prefstype.Item2xPos, 629),
                        PlayerPrefs.GetInt(Prefstype.Item2yPos, -61), 0.0f);
    }
   


}

