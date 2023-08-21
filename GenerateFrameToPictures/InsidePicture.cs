using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InsidePicture : MonoBehaviour, IIntractable
{

    [SerializeField]
    private List<GameObject> _spawnPoint = new();
    [SerializeField]

    private List<GameObject> _ghosts = new();

    [SerializeField]
    private List<GameObject> _rareAnimation = new();
    [SerializeField]
    private List<GameObject> _staticAnim = new();
    [SerializeField]
    private List<GameObject> _actionAnim = new();

    [SerializeField]
    private bool _ghostSpawned, _needSpawnGhost, _needBorder, _needMaximumSpriteDifference;


    private bool _coroutineRunning, _needRepeatSearch;
    [SerializeField]
    private float _speedFlip;

    private float _timer;

    private GameObject _ghost;
    [SerializeField]

    private SpriteRenderer _spritePlace;
    [SerializeField]
    private GameObject _cube;
    [SerializeField]

    private List<Sprite> _sprites = new();


    [SerializeField]
    private List<Material> _materialForPictures = new();

    [SerializeField]
    private float _frameSize;

    private Vector2 _spriteSize;

    private List<GameObject> _picturesFrames = new();
    [SerializeField]

    private GameObject _glassPicture;

    private Quaternion _originalRotation;
    private Quaternion _fixPosition;
    [SerializeField, Range(0, 1920)]

    private int _minWidth, _maxWidth, _minHeight, _maxHeight;
    [SerializeField]

    private float _spriteWidth, _spriteHeight;

    public enum PictureSize
    {
        Random,
        VerySmall,
        Small,
        Medium,
        Big
    }
    private PictureSize _pictureSize;

    private void Start()
    {
        _originalRotation = transform.rotation;

        _fixPosition = Quaternion.Euler(0f, 0f, 0f);
        transform.localRotation = _fixPosition;

        _cube.SetActive(true);
        _frameSize = 0.5f;

       /*  TrySpawnGhostToPictures(); */
        ChangeSpriteToRandom();
        SpawnScaleToAddRandomVisual();
        ChangeRotation();

    }

    private void ChangeRotation()
    {
        transform.localRotation = _originalRotation;
    }
    
    private void ChangeSpriteToRandom()
    {
        if (!CheckSizeLimits())
        {
            _needRepeatSearch = true;
            if (_needRepeatSearch)
            {

                if (!CheckSizeLimits())
                {
                    Debug.Log("Cant find sprite with true size");
                    this.gameObject.SetActive(false);
                    return;
                }
            }
            else
            {
                Debug.Log("Cant find sprite with true size");
                this.gameObject.SetActive(false);
                return;
            }

        }
        // как далеко должно быть стекло от картины, для микро картинок надо минималку, а для остального как хош
        var glassDistance = -0.001f;

        _glassPicture.transform.localScale = new Vector3(_spriteWidth, _spriteHeight, 0);
        _glassPicture.transform.localPosition = new Vector3(_glassPicture.transform.localPosition.x, _glassPicture.transform.localPosition.y, glassDistance);


        MakeFrameworkToPictures();

    }
    private void ChangePictureSize()
    {

        if (PictureSize.Random == _pictureSize)
        {
            _pictureSize = (PictureSize)1;
        }
        switch (_pictureSize)
        {
            case PictureSize.Random:

                break;

        }
    }

    private void SpawnScaleToAddRandomVisual()
    {
        // все цифры рандом
        int randX = Random.Range(0, 20);
        if (randX > 15)
        {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
        int randY = Random.Range(0, 50);
        if (randY > 45)
        {
            transform.localScale = new Vector3(transform.localScale.x, -transform.localScale.y, transform.localScale.z);
        }
    }

    // проверяем картинку на размеры и если ок, то отдаем true
    private bool CheckSizeLimits()
    {

        Helper.ShuffleList(_sprites);
        bool spriteIsFind = false;



        for (int i = 0; i < _sprites.Count; i++)
        {
            _spritePlace.sprite = _sprites[i];
            Vector2 spriteSizeInPixels = _spritePlace.sprite.rect.size;
            float pixelsPerUnit = _spritePlace.sprite.pixelsPerUnit;
            Vector2 spriteSizeInPoints = spriteSizeInPixels / pixelsPerUnit;

            _spriteWidth = spriteSizeInPoints.x;
            _spriteHeight = spriteSizeInPoints.y;

            if (spriteSizeInPixels.x > _minWidth && spriteSizeInPixels.x <= _maxWidth)
            {

                if (spriteSizeInPixels.y > _minHeight && spriteSizeInPixels.y <= _maxHeight)
                {
                    if (_needRepeatSearch)
                    {
                        spriteIsFind = true;
                        _spriteWidth = spriteSizeInPoints.x;
                        _spriteHeight = spriteSizeInPoints.y;
                        _needRepeatSearch = true;
                        break;
                    }

                    bool currentSpriteExists = MaximumDifferenceSprites();
                    if (currentSpriteExists) continue;

                    spriteIsFind = true;
                    _spriteWidth = spriteSizeInPoints.x;
                    _spriteHeight = spriteSizeInPoints.y;
                    break;
                }
            }
        }

        return spriteIsFind;
    }

    private bool MaximumDifferenceSprites()
    {
        if (!_needMaximumSpriteDifference) return false;
        // если так случилось что все картинки были уже использвованы и над всеравно шото заспавнить, то даем возможность потоврить пикчу

        // если спрайта нету, то добавляем в список и пропускаем
        if (!Helper.SpritesController.Contains(_spritePlace.sprite))
        {
            Helper.SpritesController.Add(_spritePlace.sprite);

            return false;
        }
        else
        {
            return true;
        }

    }


    private void MakeFrameworkToPictures()
    {
        if (!_needBorder)
        {
            _cube.SetActive(false);
            return;
        }


        _frameSize /= 2;

        // 4 = настройка 4 рамок по отдельности
        for (int i = 0; i < 4; i++)
        {
            var cube = Instantiate(_cube, transform.position, Quaternion.identity);
            cube.transform.SetParent(_spritePlace.transform);
            
            if (i == 0 || i == 1)
            {
                cube.transform.localScale = new Vector3(_frameSize, _spriteHeight, 0.2f);

            }
            if (i == 2 || i == 3)
            {
                // +_frameSize * 2 = удлиняем топ и бот рамку шоб красиво
                cube.transform.localScale = new Vector3(_spriteWidth + _frameSize * 2, _frameSize, 0.2f);

            }

            if (i == 0)
            {
                cube.gameObject.name = "left";
                cube.transform.localPosition = new Vector3((_spritePlace.transform.localPosition.x + _spriteWidth / 2) + _frameSize / 2, _spritePlace.transform.localPosition.y, 0);
            }
            if (i == 1)
            {
                cube.gameObject.name = "right";
                cube.transform.localPosition = new Vector3((_spritePlace.transform.localPosition.x - _spriteWidth / 2) - _frameSize / 2, _spritePlace.transform.localPosition.y, 0);
            }
            if (i == 2)
            {
                cube.gameObject.name = "top";
                cube.transform.localPosition = new Vector3(_spritePlace.transform.localPosition.x, (_spritePlace.transform.localPosition.y + _spriteHeight / 2) + _frameSize / 2, 0);

            }
            if (i == 3)
            {
                cube.gameObject.name = "bottom";
                cube.transform.localPosition = new Vector3(_spritePlace.transform.localPosition.x, (_spritePlace.transform.localPosition.y - _spriteHeight / 2) - _frameSize / 2, -0);

            }
            cube.transform.localRotation = _spritePlace.transform.localRotation;
            cube.transform.localPosition = new Vector3(cube.transform.localPosition.x, cube.transform.localPosition.y, -0.1f);

            _picturesFrames.Add(cube);

        }
        MergeMeshes();
        _cube.SetActive(false);
    }

    private void MergeMeshes()
    {
        // Create an array to store CombineInstance objects
        CombineInstance[] combine = new CombineInstance[_picturesFrames.Count];

        for (int i = 0; i < _picturesFrames.Count; i++)
        {
            MeshFilter meshFilter = _picturesFrames[i].GetComponent<MeshFilter>();

            if (meshFilter != null && meshFilter.sharedMesh != null)
            {
                // Assign the mesh and transformation to CombineInstance
                combine[i].mesh = meshFilter.sharedMesh;
                combine[i].transform = meshFilter.transform.localToWorldMatrix;
            }
            else
            {
                // Handle cases where a GameObject does not have a valid mesh
                Debug.LogWarning("Skipping GameObject with missing or invalid mesh.");
            }
        }

        // Create a new mesh for the combined result
        Mesh combinedMesh = new Mesh();
        combinedMesh.CombineMeshes(combine);

        // Create a new GameObject to hold the combined mesh
        GameObject combinedMeshObject = new GameObject("CombinedMesh");

        // Assign the combined mesh to the MeshFilter of the new GameObject
        MeshFilter meshFilterCombined = combinedMeshObject.AddComponent<MeshFilter>();
        meshFilterCombined.sharedMesh = combinedMesh;

        // Optionally, add a MeshRenderer component for rendering
        MeshRenderer meshRendererCombined = combinedMeshObject.AddComponent<MeshRenderer>();

        // Set the material after parenting
        meshRendererCombined.material = _materialForPictures[Random.Range(0, _materialForPictures.Count)];

        // Parent the combined mesh to _spritePlace
        combinedMeshObject.transform.SetParent(_spritePlace.transform);

        // Optionally, adjust the position, rotation, and scale of the combined mesh as needed
        // combinedMeshObject.transform.position = ...
        // combinedMeshObject.transform.rotation = ...
        // combinedMeshObject.transform.localScale = ...

        // Activate or deactivate original GameObjects as needed
        foreach (GameObject pictureFrame in _picturesFrames)
        {
            Destroy(pictureFrame);
        }
        Destroy(_cube);
        combinedMeshObject.gameObject.name = "PicturesFrame";
    }

    private void Update()
    {
        if (_needSpawnGhost && !_ghostSpawned)
        {
            _timer += Time.deltaTime;
            if (_timer > 60f)
            {
                TrySpawnGhostToPictures();
                _timer = 0;
            }
        }
        if (_ghostSpawned)
        {
            _timer += Time.deltaTime;
            if (_timer > 60f)
            {
                UpdateYRotation();
                _timer = 0;
            }
        }
    }
    
    private void TestStartCour()
    {
        if (_coroutineRunning) return;
        StartCoroutine("FaceGoBrrr");
    }
    private IEnumerator FaceGoBrrr()
    {
        _coroutineRunning = true;
        float curTimer = 0;
        float insideTimer = 0;
        bool flipX = false;

        while (curTimer < 2f) // Changed the condition to "<" to run for the first 2 seconds
        {
            curTimer += Time.unscaledDeltaTime; // Increment curTimer
            insideTimer += Time.unscaledDeltaTime; // Increment insideTimer

            if (insideTimer > _speedFlip)
            {
                insideTimer = 0;
                _spritePlace.flipX = !flipX;
                flipX = !flipX;
                yield return null;
            }

            yield return null; // Yield to allow other Unity processes to run
        }

        _coroutineRunning = false;
    }

    
    private void TrySpawnGhostToPictures()
    {
        return;
        var randomChance = Random.Range(0, 50);

        if (randomChance < 95)
        {
            SpawnGhostToPictures();
            ChangeAnimation();
            UpdatePictures();
            _ghostSpawned = true;

        }
    }

    private void SpawnGhostToPictures()
    {
        var randomGhost = _ghosts[Random.Range(0, _ghosts.Count)];
        var randomPoint = _spawnPoint[Random.Range(0, _spawnPoint.Count)];
        var ghost = Instantiate(randomGhost, transform.position, Quaternion.identity);
        ghost.transform.position = randomPoint.transform.position;
        ghost.transform.SetParent(this.gameObject.transform);

        float randomYRotation = Random.Range(0f, 360f);
        ghost.transform.localRotation = Quaternion.Euler(0f, randomYRotation, 0f);

        _ghost = ghost;

    }
    
    private void ChangeAnimation()
    {
        var newAnim = _staticAnim[Random.Range(0, _staticAnim.Count)];
        string clipName = newAnim.name;


        string[] parts = clipName.Split('@');
        if (parts.Length == 2)
        {
            string rightPart = parts[1];
            _ghost.GetComponent<Animator>().Play(rightPart);

        }


    }
    
    private void UpdateYRotation()
    {
        float randomYRotation = Random.Range(0f, 360f);
        _ghost.transform.localRotation = Quaternion.Euler(0f, randomYRotation, 0f);
        UpdatePictures();
    }

    private void UpdatePictures()
    {

    }

    private void ActivateGhost()
    {

    }

    public void Interact(bool isPlayer, int points = 0)
    {
        if (isPlayer)
        {
            if (_coroutineRunning) return;
            StartCoroutine("FaceGoBrrr");
        }
    }
}
