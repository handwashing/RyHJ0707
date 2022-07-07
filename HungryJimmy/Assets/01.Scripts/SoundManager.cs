using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound //다른 객체에 컴퍼넌트 추가할 수 없음(BCZ / MonoBehaviour 상속 x)
{
    public string name; //곡의 이름
    public AudioClip clip; //곡
}

public class SoundManager : MonoBehaviour
{//사운드 매니저가 복수 생성되는 것을 방지하기 위해 싱글턴화 -> 기존의 사운드 매니저를 살려두고
//새로 생성된 (씬에) 것은 파괴한다. => 어느 씬에 있던 하나의 사운드 매니저만을 유지할 수 있다.
    
    static public SoundManager instance; //자기 자신을(SoundManager)를 instance로 만드는 것 / 어디서든지 접근 가능하고 공유 자원으로 쓰도록 static처리
    //싱글턴 
    #region singleton
    void Awake() //객체 생성시 최초 실행
    {
        if(instance == null) //instance에 들어있는 것이 없다면...
        {
            instance = this; //최초 실행시 자기 자신을 (껍데기에)넣어 채워주기...
            DontDestroyOnLoad(gameObject); //자기 자신은 파괴시키지 말아라...
        }
        else
            Destroy(gameObject); //이미 존재한다면 새로 생성된 것을 파괴
    }
    #endregion singleton

    public AudioSource[] audioSourceEffects; //동시에 여러개 재생할 수 있도록 배열 사용
    public AudioSource audioSourceBgm; //Bgm은 하나만 필요 -> 배열 필요x

    public string[] playSoundName; //특정한 곡만 정지시킬때...

    public Sound[] effectSounds; //<- 이와 같이 class Sound의 내용을 변수로 만들어 가져다 쓰면 된다...
    public Sound[] bgmSounds;

    void Start() //게임이 시작할 때 자동으로 오디오 소스의 개수와 string배열의 개수가 일치되게...
    {
        playSoundName = new string[audioSourceEffects.Length]; //playSoundName 배열 개수 생성 (audioSourceEffects의 개수만큼...)
    }

    public void PlaySE(string _name)
    {//_name이 Sound의 name과 일치한다면 -> clip을 AudioSource에 넣어 재생시킬 것

     //Sound에 일치하는 이름이 있는지 확인
        for (int i = 0; i < effectSounds.Length; i++)
        {
            if(_name == effectSounds[i].name) //_name와 effectSounds의 i번째 name과 일치하다면
            {//재생중이지 않은 오디오소스를 찾아 재생시키기
                for (int j = 0; j < audioSourceEffects.Length; j++)
                {
                    if(!audioSourceEffects[j].isPlaying) //audioSourceEffects의 배열 j번째가 재생중이 아니라면...
                    {
                        playSoundName[j] = effectSounds[i].name;  //효과음의 이름을 playSoundName[j]스트링에 넣어준 것... /(재생중인 오디오 소스가 있으면) playSoundName[j]와 이름을 일치시킨 것...                
                        audioSourceEffects[j].clip = effectSounds[i].clip; //j번째의 곡을 i번째 클립으로 교체해준다
                        audioSourceEffects[j].Play(); //Play
                        return;
                    }
                }
                Debug.Log("모든 가용 AudioSource가 사용중임");
                return;
            }
        }                                 
        Debug.Log(_name + "사운드가 SoundManager에 등록되지 않았음");
    }

    public void StopAllSE() //모든 효과음 재생 정지
    {
        for (int i = 0; i < audioSourceEffects.Length; i++) //오디오소스 개수만큼 반복
        {
            audioSourceEffects[i].Stop(); //오디오소스의 i번째를 전부 Stop 시키기...
        }       
    }

     //특정한 곡만 재생 정지
     public void StopSE(string _name)
     {
        for (int i = 0; i < audioSourceEffects.Length; i++) 
        {
            if(playSoundName[i] == _name)    //playSoundName i번째와 일치한게 있다면
            {
                audioSourceEffects[i].Stop(); //정지시키기
                return;
            }
        } 
        Debug.Log("재생 중인" + _name + "사운드가 없음");
     }
}
