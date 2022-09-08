using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsUIController : MonoBehaviour
{
    [SerializeField] GameObject SettingsUI;
    [SerializeField] IsoToggle BGMButton;
    [SerializeField] SpriteRenderer BGMButtonSprite;
    [SerializeField] IsoToggle SFXButton;
    [SerializeField] SpriteRenderer SFXButtonSprite;
    [SerializeField] IsoButton OKButton;
    [SerializeField] IsoButton MainButton;
    [Space(5.0f)]
    [SerializeField] Transform StartPos;
    [SerializeField] Transform EndPos;
    [Space(5.0f)]
    [SerializeField] Sprite OnTexture;
    [SerializeField] Sprite OffTexture;

    public void Start()
    {
        SettingsUI.transform.position = StartPos.position;
    }

    public void Load()
    {
        BGMButton.Toggle = System.Convert.ToBoolean(IOManager.Ins.BGM);
        SFXButton.Toggle = System.Convert.ToBoolean(IOManager.Ins.SFX);
        BGMButtonSprite.sprite = BGMButton.Toggle ? SetTextureOn("BGM") : SetTextureOff("BGM");
        SFXButtonSprite.sprite = SFXButton.Toggle ? SetTextureOn("SFX") : SetTextureOff("SFX");
    }

    public void Initialize()
    {
        StartCoroutine(StartAnim());
    }

    IEnumerator StartAnim()
    {
        float t = 0.0f;
        SettingsUI.transform.position = StartPos.position;
        while(t < 3.0f)
        {
            t += 0.1f;
            SettingsUI.transform.position = Vector3.Lerp(SettingsUI.transform.position, EndPos.position, 0.1f);
            yield return null;
        }
        SettingsUI.transform.position = EndPos.position;
    }

    IEnumerator EndAnim()
    {
        float t = 0.0f;
        while (t < 3.0f)
        {
            t += 0.1f;
            SettingsUI.transform.position = Vector3.Lerp(SettingsUI.transform.position, StartPos.position, 0.1f);
            yield return null;
        }
        SettingsUI.transform.position = StartPos.position;
    }


    Sprite SetTextureOn(string type)
    {
        if(type == "BGM")
        {
            IOManager.Ins.BGM = 1;
            SceneManager.Ins.Scene.SoundManager.SetVolume(SOUND_TYPE.BGM, true);
        }
        else
        {
            IOManager.Ins.SFX = 1;
            SceneManager.Ins.Scene.SoundManager.SetVolume(SOUND_TYPE.SFX, true);
        }
        return OnTexture;
    }

    Sprite SetTextureOff(string type)
    {
        if (type == "BGM")
        {
            SceneManager.Ins.Scene.SoundManager.SetVolume(SOUND_TYPE.BGM, false);
            IOManager.Ins.BGM = 0;
        }
        else
        {
            SceneManager.Ins.Scene.SoundManager.SetVolume(SOUND_TYPE.SFX, false);
            IOManager.Ins.SFX = 0;
        }
        return OffTexture;
    }

    private void Update()
    {
        BGMButtonSprite.sprite = BGMButton.Toggle ? SetTextureOn("BGM") : SetTextureOff("BGM");
        SFXButtonSprite.sprite = SFXButton.Toggle ? SetTextureOn("SFX") : SetTextureOff("SFX");

        if (OKButton.IsClicked)
        {
            OKButton.IsClicked = false;
            IOManager.Ins.SaveSoundInfo();
            SceneManager.Ins.Scene.ChangeState(GAME_STATE.INGAME);
            StartCoroutine(EndAnim());
        }

        if(MainButton.IsClicked)
        {
            MainButton.IsClicked = false;
            UnityEngine.SceneManagement.SceneManager.LoadScene("MainScene");
        }
    }
}
