using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : BaseManager<AudioManager>
{
    //Separate audio sources for bgm and sfx
    [SerializeField] private AudioSource attachBgmSource;
    [SerializeField] private AudioSource attachSeSource;
    [SerializeField] private AudioSource attachVcSource;

    private Dictionary<string, AudioClip> bgmDic, seDic, vcDic; //Keep all audio

    private bool canPlaySe = true;
    private bool canPlayVc = true;
    private bool isFadeOut = false; //Is the hightlight bgm fading out?
    private float bgmFadeSpeedRate = CONST.BGM_FADE_SPEED_RATE_HIGH;
    private string nextBgmName;
    private string nextSeName;
    private string nextVcName;

    public AudioSource AttachBGMSource { get => this.attachBgmSource; }
    public AudioSource AttachSESource { get => this.attachSeSource; }
    public AudioSource AttachVCSource { get => this.attachVcSource; }

    //protected override void LoadComponent()
    //{
    //    base.LoadComponent();
    //    if (this.attachBgmSource == null)
    //        this.attachBgmSource = transform.Find("BGMSource").GetComponent<AudioSource>();

    //    if (this.attachSeSource == null)
    //        this.attachSeSource = transform.Find("SESource").GetComponent<AudioSource>();

    //    if (this.attachVcSource == null)
    //        this.attachVcSource = transform.Find("VCSource").GetComponent<AudioSource>();
    //}

    protected override void Awake()
    {
        base.Awake();

        //Load all SE & BGM files from resource folder
        this.bgmDic = new Dictionary<string, AudioClip>();
        this.seDic = new Dictionary<string, AudioClip>();
        this.vcDic = new Dictionary<string, AudioClip>();

        object[] bgmList = Resources.LoadAll("Audio/BGM");
        object[] seList = Resources.LoadAll("Audio/SE");
        object[] vcList = Resources.LoadAll("Audio/VC");

        foreach (AudioClip bgm in bgmList)
        {
            this.bgmDic[bgm.name] = bgm;
        }
        foreach (AudioClip se in seList)
        {
            this.seDic[se.name] = se;
        }
        foreach (AudioClip vc in vcList)
        {
            this.vcDic[vc.name] = vc;
        }
    }

    private void Start()
    {
        this.SetupAudio();
    }

    private void Update()
    {
        this.HandleChangeBgm();
    }

    private void SetupAudio()
    {
        //float masterVolume = ObscuredPrefs.GetFloat(CONST.MAS_VOLUME_KEY, CONST.MAS_VOLUME_DEFAULT);
        //this.attachBgmSource.volume = ObscuredPrefs.GetFloat(CONST.BGM_VOLUME_KEY, CONST.BGM_VOLUME_DEFAULT) * masterVolume;
        //this.attachSeSource.volume = ObscuredPrefs.GetFloat(CONST.SE_VOLUME_KEY, CONST.SE_VOLUME_DEFAULT) * masterVolume;

        //bool masterMute = ObscuredPrefs.GetBool(CONST.MAS_MUTE_KEY, CONST.MAS_MUTE_DEFAULT);
        //this.attachBgmSource.mute = masterMute ? false : ObscuredPrefs.GetBool(CONST.BGM_MUTE_KEY, CONST.BGM_MUTE_DEFAULT);
        //this.attachSeSource.mute = masterMute ? false : ObscuredPrefs.GetBool(CONST.SE_MUTE_KEY, CONST.SE_MUTE_DEFAULT);
    }

    private void HandleChangeBgm()
    {
        if (!this.isFadeOut) return;

        //Gradually lower the volume, and when the volume reaches 0
        //return the volume and play the next song
        this.attachBgmSource.volume -= Time.deltaTime * this.bgmFadeSpeedRate;
        if (this.attachBgmSource.volume <= 0)
        {
            this.attachBgmSource.Stop();
            //this.attachBgmSource.volume = ObscuredPrefs.GetFloat(CONST.BGM_VOLUME_KEY, CONST.BGM_VOLUME_DEFAULT);
            this.isFadeOut = false;

            if (!string.IsNullOrEmpty(this.nextBgmName))
            {
                this.PlayBGM(this.nextBgmName);
            }
        }
    }

    public void PlaySE(AudioClip audio)
    {
        this.attachSeSource.PlayOneShot(audio);
    }

    public void PlaySE(string seName, float delay = 0.0f)
    {
        if (!seDic.ContainsKey(seName))
        {
            Debug.LogError(seName + " There is no SE named");
            return;
        }

        if (this.canPlaySe)
        {
            this.canPlaySe = false;
            this.nextSeName = seName;
            this.attachSeSource.PlayOneShot(this.seDic[this.nextSeName] as AudioClip);
            Invoke("DelayPlaySE", delay);
        }
    }

    public void PlaySeLoop(string seName, float delay = 0f)
    {
        if (!this.seDic.ContainsKey(seName))
        {
            Debug.Log($"'{seName}' There is no SE named");
            return;
        }

        this.nextSeName = seName;
        this.attachSeSource.clip = this.seDic[this.nextSeName] as AudioClip;
        this.attachSeSource.Play(0);
        this.attachSeSource.loop = true;
    }

    public void PlaySeStop(string seName, float delay = 0f)
    {
        if (!this.seDic.ContainsKey(seName))
        {
            Debug.Log($"'{seName}' There is no SE named");
            return;
        }

        this.nextSeName = seName;
        this.attachSeSource.clip = this.seDic[this.nextSeName] as AudioClip;
        this.attachSeSource.Stop();
    }

    private void DelayPlaySE()
    {
        this.canPlaySe = true;
    }

    public void PlayVC(string vcName, float delay = 0f)
    {
        if (!this.vcDic.ContainsKey(vcName))
        {
            Debug.Log($"'{vcName}' There is no VC named");
            return;
        }

        if (this.canPlayVc)
        {
            this.canPlayVc = false;
            this.nextVcName = vcName;
            this.attachVcSource.PlayOneShot(this.vcDic[this.nextVcName] as AudioClip);
            Invoke("DelayCanPlayVC", delay);
        }
    }

    private void DelayCanPlayVC()
    {
        this.canPlayVc = true;
    }

    public void PauseBGM()
    {
        if (this.attachBgmSource.clip != null)
        {
            this.attachBgmSource.Stop();
        }
    }

    public void PlayBGM(string bgmName, float fadeSpeedRate = CONST.BGM_FADE_SPEED_RATE_HIGH)
    {
        if (!this.bgmDic.ContainsKey(bgmName))
        {
            Debug.LogError(bgmName + " There is no BGM named");
            return;
        }

        //If bgm is not currently playing, play it as is
        if (!this.attachBgmSource.isPlaying)
        {
            this.nextBgmName = "";
            this.attachBgmSource.clip = this.bgmDic[bgmName] as AudioClip;
            this.attachBgmSource.Play();
        }
        //When a different bgm is playing, fade out the bgm that is playing before playing the next one
        //Throught when the same bgm is playing
        else if (this.attachBgmSource.clip.name != bgmName)
        {
            this.nextBgmName = bgmName;
            this.FadeOutBGM(fadeSpeedRate);
        }
    }

    public void FadeOutBGM(float fadeSpeedRate = CONST.BGM_FADE_SPEED_RATE_LOW)
    {
        bgmFadeSpeedRate = fadeSpeedRate;
        isFadeOut = true;
    }

    public void ChangeBGMVolume(float BGMVolume)
    {
        AttachBGMSource.volume = BGMVolume;
        PlayerPrefs.SetFloat(CONST.BGM_VOLUME_KEY, BGMVolume);
    }

    public void ChangeSEVolume(float SEVolume)
    {
        AttachSESource.volume = SEVolume;
        PlayerPrefs.SetFloat(CONST.SE_VOLUME_KEY, SEVolume);
    }
}
