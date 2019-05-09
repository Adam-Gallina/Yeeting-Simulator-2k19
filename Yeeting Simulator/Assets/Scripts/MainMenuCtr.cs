using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MainMenuCtr : MonoBehaviour {

    public void ChangeToScene(int scene)
    {
        Debug.Log("boop");
        if (scene == -1)
        {
            Application.Quit();
            return;
        }
        LoadingBar(scene);
        LoadScene(scene);
        //SceneManager.LoadScene(scene);
    }

    IEnumerator LoadScene(int level)
    {
        Debug.Log("Loading level " + level.ToString());
        /*AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(0);
        while (!asyncLoad.isDone)
            yield return null;*/
        HUDcontroller hud = GameObject.Find("ViveCurvePointers").GetComponent<SmashController>().hud;
        string msg = "";
        string dots = "";
        float nextDots = Time.time + .5f;
        switch (level)
        {
            case -1:
                msg = "Quitting";
                break;
            case 1:
                msg = "Loading Tutorial";
                break;
            case 2:
                msg = "Loading Survival Mode";
                break;
            default:
                msg = "The developers really screwed up, and are trying to load the wrong level\nLOL";
                break;
        }
        Debug.Log(msg);
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(level);
        while (!asyncLoad.isDone)
        {
            if (Time.time > nextDots)
            {
                nextDots = Time.time + .5f;
                dots += ".";
                if (dots == "...")
                    dots = "";
            }
            hud.SendMsg(msg + dots, 0.5f, Color.black);
            yield return null;
        }
    }

    IEnumerator LoadingBar(int level)
    {
        HUDcontroller hud = GameObject.Find("ViveCurvePointers").GetComponent<SmashController>().hud;
        string msg = "";
        string dots = "";
        float nextDots = Time.time + .5f;
        switch (level)
        {
            case -1:
                msg = "Quitting";
                break;
            case 1:
                msg = "Loading Tutorial";
                break;
            case 2:
                msg = "Loading Survival Mode";
                break;
            default:
                msg = "The developers really screwed up, and are trying to load the wrong level\nLOL";
                break;
        }

        while (true)
        {
            if (Time.time > nextDots)
            {
                nextDots = Time.time + .5f;
                dots += ".";
                if (dots == "...")
                    dots = "";
            }
            hud.SendMsg(msg + dots, 0.5f, Color.black);
            yield return null;
        }
    }
}
