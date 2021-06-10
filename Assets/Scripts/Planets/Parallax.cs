using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Rendering;

public class Parallax : MonoBehaviour
{
    [Header("Planet prefabs")]
    public GameObject[] planetPrefabs;

    public uint countLayers = 15;
    public Vector2 scaleLimits = new Vector2(0.1f, 0.5f);
    public Vector2 speedLimits = new Vector2(0.2f, 4f);
    public Vector2 Y_limits = new Vector2(-4.25f, 4.25f);
    public Vector2 timerLimits= new Vector2 (1f,3f);
    public uint usedPlanetSize = 6;

    private List<GameObject>[] _layers;
    private List<uint> _usedPlanet;

    // Start is called before the first frame update
    private void Start()
    {
        _usedPlanet = new List<uint>();
        float timer = Random.Range(timerLimits.x, timerLimits.y);
        _layers = new List<GameObject>[15];
        for (int i=0; i<countLayers; ++i)
        {
            _layers[i] = new List<GameObject>();
        }
        Invoke(nameof(CreateRandomPlanet), timer);
    }

    // Update is called once per frame
    private void Update()
    {
        
    }

    private uint GetRandomPlanetIndex ()
    {
        var availablePlanets = new List<uint>();
        for (uint i=0; i<planetPrefabs.Length; ++i)
        {
            if (!_usedPlanet.Contains(i))
                availablePlanets.Add(i);
        }
        var Index = Random.Range(0, availablePlanets.Count);
        var PlanetIndex = availablePlanets[Index];
        return PlanetIndex;
    }

    private void PushIndexToUsedPlanet(uint value)
    {
        if (_usedPlanet.Count >= usedPlanetSize)
            _usedPlanet.RemoveAt(0);
        _usedPlanet.Add(value);
    }

    private void CreateRandomPlanet()
    {
        var layer = Random.Range(0, _layers.Length);
        var timer = Random.Range(timerLimits.x, timerLimits.y);
        var PlanetIndex = GetRandomPlanetIndex();
        PushIndexToUsedPlanet(PlanetIndex);
        var pos_y = Random.Range(Y_limits.x, Y_limits.y);
        var pos = transform.position;
        pos.y = pos_y;
        var koef = (scaleLimits.y - scaleLimits.x) / countLayers;
        var scale = scaleLimits.y - koef * layer;
        var planet = Instantiate<GameObject>(planetPrefabs[PlanetIndex], pos, Quaternion.Euler(0f, 0f, 0f));
        var koef_speed = (speedLimits.y - speedLimits.x) / countLayers;
        var speed = speedLimits.y - koef_speed * layer;
        planet.GetComponent<Planet>().speed = speed;
        
        // TOOD: переделать сортинг лейер
        // planet.GetComponent<SortingGroup>().sortingOrder = -10 - layer;
        planet.transform.localScale = new Vector3(scale, scale, 1);

        Invoke(nameof(CreateRandomPlanet), timer);
    }
}
