using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HUDControl : MonoBehaviour {

    public Slider _slider;
    public Image _fillImage;
    public int _playerNumber;
    public Color _healthy = Color.green;
    public Color _weak = Color.red;
    public Text _health;

    public Text _lives;
    public Image _charImage;

    public Character _playerCharacter;

    // Use this for initialization
    void Start()
    {

        // _charImage.sprite = Resources.Load<Sprite>(getCharacterImage(GlobalManager._instance._playerTypes[_playerNumber])); //Name of Character Sprite image
        //Debug.Log("PLayer Number: " + _playerNumber);
    }

    void OnEnable()
    {
        //Debug.Log("PLAYER NUMBER: " + _playerNumber);
        _playerCharacter = GlobalManager._instance._players[_playerNumber].GetComponent<Character>();
        setUI();
    }

    // Update is called once per frame
    void Update()
    {
        
        //setUI();
    }

    public void setUI()
    {
        _slider.value = _playerCharacter._health;

        //Debug.Log("Health at: " + ((float)_playerCharacter._health / _playerCharacter._startHealth));

        _fillImage.color = Color.Lerp(_weak, _healthy, ((float)_playerCharacter._health / _playerCharacter._startHealth));

        _lives.text = "" + _playerCharacter._lives;

        //Set Character 
        int charType = GlobalManager._instance._playerTypes[_playerNumber];
        _charImage.sprite = GlobalManager._instance._players[_playerNumber].GetComponent<Character>().isOutOfLives() ? GlobalManager._instance._characterDeadList[charType].sprite : GlobalManager._instance._characterImageList[charType].sprite;

        //Debug.Log("Player Number: " + _playerNumber);
        //Debug.Log(GlobalManager._instance._players[_playerNumber].GetComponent<Character>());
        int hp = GlobalManager._instance._players[_playerNumber].GetComponent<Character>()._health;
        _health.text = "" + (hp < 0 ? 0 : hp);
    }


}
