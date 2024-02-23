using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public TileBoard tileBoard; // 游戏逻辑
    public TextMeshProUGUI txtScore;
    public TextMeshProUGUI txtBest;

    public void NewGame()
    {
        txtScore.text = "0";
        txtBest.text = "0";
        tileBoard.InitGame();
        tileBoard.GameOverPanelSwitch(0);
    }

    public void TryAgain ()
    {
        txtScore.text = "0";
        tileBoard.InitGame();
        tileBoard.GameOverPanelSwitch(0);
    }

    public void GameOver(int curScore)
    {
        tileBoard.GameOverPanelSwitch(1);
        UpdateHistoryScore(curScore);
    }

    public void UpdateScore(int score)
    {
        int curScore = int.Parse(txtScore.text);
        int newScore = curScore + score;
        txtScore.text = newScore.ToString();
        UpdateHistoryScore(newScore);
    }

    public void UpdateHistoryScore(int score)
    {
        int historyScore = int.Parse(txtBest.text);
        if (score > historyScore)
        {
            txtBest.text = score.ToString();
        }
    }

    internal void UpdateScore(string v)
    {
        throw new NotImplementedException();
    }
}
