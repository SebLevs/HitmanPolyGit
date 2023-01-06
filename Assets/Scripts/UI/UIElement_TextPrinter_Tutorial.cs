using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIElement_TextPrinter_Tutorial : UIElement_TextPrinter
{
    [SerializeField] private Image[] _backgrounds;

    protected override IEnumerator OnExecutePrintCharByChar()
    {
        UIManager _uiManager = UIManager.Instance;
        GameManager _gameManager = GameManager.Instance;
        // TODO: Set timer at 0 here and stop its clocking
        _gameManager.IsPaused = true;

        HighScoreManager _highscoreManager = HighScoreManager.Instance;
        _highscoreManager.PauseTimer();
        _highscoreManager.ResetTimer();

        int count = 0;
        foreach (string line in m_texts)
        {
            m_title.text = "";
            if (_backgrounds[count] != null)
            {
                _backgrounds[count].gameObject.SetActive(true);
            }

            for (int i = 0; i < line.Length; i++)
            {
                m_title.text += line[i];
                m_audioHandler.PlaySpecificSound("Print");
                yield return new WaitForSeconds(_uiManager.CharacterPrintSpeed.Value);
            }

            for (int i = 0; i < 4; i++)
            {
                m_title.text = (i % 2 == 0) ? line + "_" : line;
                yield return new WaitForSeconds(_uiManager.LinePrintPauseBetween.Value * 0.25f);
            }

            if (_backgrounds[count] != null)
            {
                _backgrounds[count].gameObject.SetActive(false);
            }
            count++;
        }
        _gameManager.IsPaused = false;
        _gameManager.HasSeenTutorial.Value = true;
        _highscoreManager.StartTimer();
        // Start timer clocking here
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();
        if (Entity_Player.Instance.DesiredActions.Contains(PlayerActionsType.LOOKATTARGET))
        {
            Entity_Player.Instance.DesiredActions.ConsumeAllActions(PlayerActionsType.LOOKATTARGET);
            GameManager.Instance.IsPaused = false;
            GameManager.Instance.HasSeenTutorial.Value = true;
            UIManager.Instance.OnSwitchViewSynchronous(UIManager.Instance.View_Crosshair);
            HighScoreManager.Instance.StartTimer();
            // Ensure deactivation of every background
            foreach (Image img in _backgrounds)
            {
                if (img == null) { continue; }
                img.gameObject.SetActive(false);
            }
        }
    }
}
