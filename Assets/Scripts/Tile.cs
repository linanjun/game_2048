using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
    public Image imgBackground;
    public TextMeshProUGUI txtNumber;

    public void SetTileInfo(TileState state, TileCell cell)
    {
        imgBackground = GetComponent<Image>();
        txtNumber = GetComponentInChildren<TextMeshProUGUI>();
        imgBackground.color = state.backgroundColor;
        txtNumber.color = state.textColor;
        txtNumber.text = state.number.ToString();
        transform.position = cell.transform.position;
    }
}
