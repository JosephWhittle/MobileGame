using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using Random = UnityEngine.Random;

public class GameRunner : MonoBehaviour
{
    //Inspector values ect
    [Header("Fireable Object")] [SerializeField]
    private GameObject _astronaut;

    [Header("Spawn position game object")] [SerializeField]
    private GameObject _spawnPosition;

    [Header("Sound effects")] 
    [SerializeField] public AudioSource _woosh;
    [SerializeField] public AudioSource _crack;
    [SerializeField] public AudioSource _pop;

    [Header("The audio Mixer volume setter")]
    [SerializeField] private AudioMixer _mainMixer;

    [Header("Wheel Joint")] [SerializeField]
    public WheelJoint2D _wheelJoint2D;

    [Header("Winning Screen")]
    [SerializeField] private GameObject _winScreen;

    [HideInInspector] public bool CanTap;

    [Header("Rotator Values below")] 
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Transform _rotatorTransform;

    [Header("LevelData below - open list and add levels as needed")]
    [SerializeField] private List<Level> _allLevels;

    [Header("Astronauts left text")] [SerializeField]
    public TextMeshProUGUI _astronautsLeftText;

    [Header("Moon coins text below")]
    [SerializeField] private TextMeshProUGUI _moonCoins;

    [Header("Obstacle Object")]
    [SerializeField] private GameObject _obstacle;

    [Header("The child of the rotator")]
    [SerializeField] private GameObject _planetChild;

    [Header("The ad manager")] [SerializeField]
    private AdManager _adManager;
    


    //Game varibles

    //Clearing the projectiles
    [HideInInspector] public int AstronautCountDown;
    [HideInInspector] public int AstronautCounter;
    private List<Moveable> _firedObjects;
    private List<ObjectCollisionHandler> _collisionHandlerObjects;
    private Level _loadedLevel;
    private bool _isGameRunning;
    private bool _runLevel;
    private bool _levelComplete;
    private bool _setUpLevel;
    private bool _isGameOver;
    private int _levelCount;
    private int _amountOfItems;

    //loop varibles
    private JointMotor2D _currentMotor;

    //timer varibless
    private float _time;
    private float _resetTime;
    private bool _setResetTime;
    private bool _currentTimerComplete;
    private int _currentRotationCounter;

    //speed change varibles
    [Header("Set the lerp for rotations below (Speed change behaviour)")]
    private float _interpolatorValue;
    private float _lerpScalar;

    //Circle varibles
    GameObject _spawnedObstacle;

    //Ad varibles
    private int adCounter;

    void Start()
    {
        CanTap = true;
        _isGameRunning = true;
        _setUpLevel = true;
        _runLevel = false;


        _interpolatorValue = 0;

        AstronautCounter = 0;
        _levelCount = 0;
        _currentRotationCounter = 0;

        _mainMixer.SetFloat("Volume", PlayerPrefs.GetFloat("MasterVolume"));
    }

    void Update()
    {
        MouseInput();

        //Start the game
        if (_isGameRunning)
        {
            //set up the level
            if (_setUpLevel)
            {
                //load the level, set the sprite, set the size
                _loadedLevel = _allLevels[_levelCount];

                //set all the planet properties
                _spriteRenderer.sprite = _loadedLevel.Planet;
                _spriteRenderer.color = _loadedLevel.SpriteHue;
                _rotatorTransform.localScale = new Vector3(_loadedLevel.Size, _loadedLevel.Size, 1);
                _amountOfItems = _loadedLevel.AmountOfItems;
                AstronautCountDown = _amountOfItems;
                AstronautCounter = 0;

                //Time varibles
                _time = 0;  
                _resetTime = _loadedLevel.Rotations[0].Duration;
                _currentTimerComplete = false;
                _currentRotationCounter = 0;
                _setResetTime = true;


                //skip this until the next level
                _setUpLevel = false;
                _runLevel = true;

                //if the level has obstacles then spawn em
                if(_loadedLevel.Obstacles.Count > 0)
                {

                    foreach (Obstacle obstacle in _loadedLevel.Obstacles)
                    {
                        if(obstacle == null) break;

                            //First spawn the obstacle
                        _spawnedObstacle = Instantiate(_obstacle, _planetChild.transform);

                        //Set the sprite
                        _spawnedObstacle.GetComponentInChildren<SpriteRenderer>().sprite = obstacle.Sprite;

                        //set the sprite size
                        _spawnedObstacle.GetComponentInChildren<SpriteRenderer>().transform.localScale =
                            obstacle.SpriteSize;

                        //set the overall scale
                        _spawnedObstacle.transform.localScale = new Vector3(obstacle.OverallScale, obstacle.OverallScale, 1);

                        //Collider scale
                        _spawnedObstacle.GetComponentInChildren<CapsuleCollider2D>().gameObject.transform.localScale = obstacle.ColliderScale;

                        //set the rotation
                        _spawnedObstacle.transform.rotation = Quaternion.AngleAxis(obstacle.Rotation, Vector3.forward);

                        //Then set the position to the center of the planet
                        _spawnedObstacle.transform.position = _rotatorTransform.transform.position;

                        ///////The issue is something to do with the fact that the planet gets scaled weirdly
                        //then set it to the edge of the collider of the planet, then add a lil for compensation as the sprites are a little lorge
                        _spawnedObstacle.transform.position += _spawnedObstacle.transform.up * (_planetChild.GetComponent<Collider2D>().bounds.size.magnitude/ 2);

                        //Set for the obstacle data
                        _spawnedObstacle.GetComponentInChildren<ObjectCollisionHandler>().ThisObstacle = obstacle;

                        //For testing
                        Color color = new Color();
                        switch (Random.Range(0, 4))
                        {
                            case 0:
                                color = Color.blue;
                                break;
                            case 1:
                                color = Color.yellow;
                                break;
                            case 2:
                                color = Color.white;
                                break;
                            case 3:
                                color = Color.gray;
                                break;
                        }
                        _spawnedObstacle.GetComponentInChildren<SpriteRenderer>().color = color;
                    }
                }




                //If the level to be loaded has a linear infinite speed, then assign below, if not continue in to the run level loop 
                if (_loadedLevel.Rotations[0].RunInfinity)
                {
                    _currentMotor = new JointMotor2D
                    {
                        maxMotorTorque = _loadedLevel.Rotations[0].MaximumMotorSpeed,
                        motorSpeed = _loadedLevel.Rotations[0].MotorSpeed,
                    };

                    
                    _wheelJoint2D.motor = _currentMotor;
                }

            }

            //Run the level 
            if (_runLevel)
            {
                //if the level loaded isnt infinite
                if (!_loadedLevel.Rotations[0].RunInfinity)
                {
                    //If the current rotation is NOT a target speed behaviour
                    if (!_loadedLevel.Rotations[_currentRotationCounter].AddTargetSpeed)
                    {
                        if (_setResetTime)
                        {
                            _resetTime = _loadedLevel.Rotations[_currentRotationCounter].Duration;
                            _setResetTime = false;

                            _currentMotor = new JointMotor2D
                            {
                                maxMotorTorque = _loadedLevel.Rotations[_currentRotationCounter].MaximumMotorSpeed,
                                motorSpeed = _loadedLevel.Rotations[_currentRotationCounter].MotorSpeed,
                            };
                            _wheelJoint2D.motor = _currentMotor;
                        }
                        //Run the timer
                        _time += Time.deltaTime;
                        //reset the timer
                        if (_time >= _resetTime)
                        {
                            _time = 0;
                            _currentTimerComplete = true;
                            _setResetTime = true;
                        }

                        //If it loads the last rotation then it goes back to the start
                        if (_currentTimerComplete)
                        {
                            _currentTimerComplete = false;
                            _currentRotationCounter++;

                            //if the rotations are complete then, start from 0 again
                            if (_currentRotationCounter >= _loadedLevel.Rotations.Count)
                            {
                                _currentRotationCounter = 0;
                            }

                        }

                    }

                    //If it does have a target speed then run the logic below
                    if(_loadedLevel.Rotations[_currentRotationCounter].AddTargetSpeed)
                    {
                        //Handles the interpolation value
                        _lerpScalar = _loadedLevel.Rotations[_currentRotationCounter].LerpScalar;
                        _interpolatorValue += _lerpScalar * Time.deltaTime;
                        if (_interpolatorValue >= 1)
                        {
                            _interpolatorValue = 0;
                        }
                        float currentMotorSpeed = Mathf.Lerp(_loadedLevel.Rotations[_currentRotationCounter].MotorSpeed, _loadedLevel.Rotations[_currentRotationCounter].TargetSpeed, _interpolatorValue);
                        
                        //Assigns the speed to the wheel motor
                        _currentMotor = new JointMotor2D
                        {
                            maxMotorTorque = _loadedLevel.Rotations[_currentRotationCounter].MaximumMotorSpeed,
                            motorSpeed = currentMotorSpeed,
                        };
                        _wheelJoint2D.motor = _currentMotor;


                        //if the motor speed has completed its turn, then the behaviour is done so more onto the rotation
                        if (_wheelJoint2D.motor.motorSpeed >= _loadedLevel.Rotations[_currentRotationCounter].TargetSpeed * 0.98f)
                        {
                            _currentRotationCounter++;

                            //if the rotations are complete then, start from 0 again
                            if (_currentRotationCounter >= _loadedLevel.Rotations.Count)
                            {
                                _currentRotationCounter = 0;
                            }
                        }
                    }
                }


                
                //set the text
                _astronautsLeftText.text = AstronautCountDown.ToString();
                //check if the level is complete
                if (AstronautCounter >= _amountOfItems)
                {
                    _levelComplete = true;
                    //Run an anim or something
                }
            }

            //If they win end the level and reset
            if (_levelComplete)
            {
                _adManager.AdCounter++;
                _adManager.TryToShowAd();

                _levelCount++;
                ClearLevelObjects();

                //check the level can be laoded in the first place
                if (_levelCount == _allLevels.Count)
                {
                    _isGameOver = true;
                }
                
                _runLevel = false;
                _setUpLevel = true;
                _levelComplete = false;
            }

            if (_isGameOver)
            {
                _isGameRunning = false;
            }

            _moonCoins.text = PlayerPrefs.GetInt("MoonCoins").ToString();
        }
        else
        {
            //if the game isnt running, then show the game over screen, this will just be when the levels run out
            CanTap = false;
            _winScreen.SetActive(true);
        }

    }




    //Just a quick dirty function to clear all the astronauts/objects on the current planet
    public void ClearLevelObjects()
    {
        _firedObjects = FindObjectsOfType<Moveable>().ToList();
        foreach (Moveable moveableObject in _firedObjects)
        {
            Destroy(moveableObject.gameObject);
        }

        _collisionHandlerObjects = FindObjectsOfType<ObjectCollisionHandler>().ToList();
        foreach (ObjectCollisionHandler objectCollisionHandler  in _collisionHandlerObjects)
        {
            Destroy(objectCollisionHandler.transform.parent.gameObject);
        }
    }

     
   
    //Mouse input also works for tapping on screen
    void MouseInput()
    {
        if (Input.GetMouseButtonDown(0) && CanTap)
        {
            //Spawn object
            Instantiate(_astronaut, _spawnPosition.transform.position, Quaternion.Euler(0, 0, 180));

            //play sound
            _woosh.pitch = Random.Range(0.93f, 1.2f);
            _woosh.Play();

            //Play vibrate if setting is on
            if (PlayerPrefs.GetFloat("Vibrate") >= 1)
            {
                Handheld.Vibrate();
            }
        }
    }
}



