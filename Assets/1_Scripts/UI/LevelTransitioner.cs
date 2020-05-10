using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using Photon.Pun;

[RequireComponent(typeof(Canvas))]
public class LevelTransitioner : MonoBehaviour
{
    public Animator animator;
    [SerializeField] private float transitionDelay = 1f;
    void Awake()
    {
        GetComponent<Canvas>().enabled = true;
        GetComponent<Canvas>().worldCamera = Camera.main;
    }
    public void LoadScene(int SceneID)
    {
        StartCoroutine(loadLevel(SceneID));
    }
    public void LoadScene(string SceneName)
    {
        StartCoroutine(loadLevel(SceneName));
    }
    private IEnumerator loadLevel(string SceneName)
    {
        FadeIn();
        yield return new WaitForSeconds(transitionDelay);
        SceneManager.LoadScene(SceneName);
    }
    public void LoadScene(string SceneName, bool UsePhoton)
    {
        if (UsePhoton)
        { StartCoroutine(photonLoadLevel(SceneName)); }
        else 
        { LoadScene(SceneName); }
    }
    private IEnumerator loadLevel(int SceneID)
    {
        FadeIn();
        yield return new WaitForSeconds(transitionDelay);
        SceneManager.LoadScene(SceneID);
    }
    private IEnumerator photonLoadLevel(string SceneName)
    {
        FadeIn();
        yield return new WaitForSeconds(transitionDelay);
        PhotonNetwork.LoadLevel(SceneName);
    }
    public void FadeIn()
    { 
        animator.SetTrigger("fadeIn");
    }
}
