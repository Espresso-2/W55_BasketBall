using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadNextScene : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        {
            AdTotalManager.Instance.ShowBox();
            var Index = SceneManager.GetActiveScene().buildIndex + 1;
            SceneManager.LoadSceneAsync(Index);
        });
    }
}