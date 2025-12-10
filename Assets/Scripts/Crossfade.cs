using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Crossfade : MonoBehaviour
{
    public Animator transition;
    public float transitionTime = 1;

    public IEnumerator ChangeScene(string sceneName)
    {
        transition.SetTrigger("StartCrossfade");
        
        yield return new WaitForSeconds(transitionTime);
        
        SceneManager.LoadScene(sceneName);
    }
}
