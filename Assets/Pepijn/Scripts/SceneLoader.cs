using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : EventUser
{
   public SceneLoader()
   {
        eventManager.SubscribeToEvent("LoadScene", new StringDelegate(LoadScene));
   }

   private void LoadScene(string _sceneName)
   {
        SceneManager.LoadScene(_sceneName);
   }
}
