using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GGJ
{
    public class MainMenuUI : MonoBehaviour
    {
        #region Params
        [Header("Panels")]
        [SerializeField] GameObject mainPanel;
        [SerializeField] GameObject multiplayerPanel;

        [Header("Buttons")]
        [SerializeField] Button playButton;
        [SerializeField] Button quitButton;

        /*[SerializeField] Button hostButton;
        [SerializeField] Button joinButton;*/

        #endregion
        private void Awake()
        {
            playButton.onClick.AddListener(() => ShowMultiplayerPanel());
            quitButton.onClick.AddListener(() => Application.Quit());
        }

        public void ShowMultiplayerPanel()
        {
            mainPanel.SetActive(false);
            multiplayerPanel.SetActive(true);
        }

    }
}
