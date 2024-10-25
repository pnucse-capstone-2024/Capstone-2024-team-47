using TMPro;
using UnityEngine;

public class PlayerInfoItem : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _nameText;
    [SerializeField] TextMeshProUGUI _levelText;
    [SerializeField] TextMeshProUGUI _mapText;

    public void SetName(string name)
    {
        _nameText.text = name;
    }

    public void SetLevel(int level)
    {
        _levelText.text = $"Level {level}";
    }

    public void SetMapId(int mapId)
    {
        _mapText.text = $"Map {mapId}";
    }
}