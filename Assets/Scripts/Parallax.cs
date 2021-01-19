using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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
    void Start()
    {
        _usedPlanet = new List<uint>();
        float timer = Random.Range(timerLimits.x, timerLimits.y);
        _layers = new List<GameObject>[15];
        for (int i=0; i<countLayers; ++i)
        {
            _layers[i] = new List<GameObject>();
        }
        Invoke("createRandomPlanet", timer);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private uint GetRandomPlanetIndex ()
    {
        List<uint> availablePlanets = new List<uint>();
        for (uint i=0; i<planetPrefabs.Length; ++i)
        {
            if (!_usedPlanet.Contains(i))
                availablePlanets.Add(i);
        }
        int Index = Random.Range(0, availablePlanets.Count);
        uint PlanetIndex = availablePlanets[Index];
        return PlanetIndex;
    }

    private void PushIndexToUsedPlanet(uint value)
    {
        if (_usedPlanet.Count >= usedPlanetSize)
            _usedPlanet.RemoveAt(0);
        _usedPlanet.Add(value);
    }

    private void createRandomPlanet()
    {
        int layer = Random.Range(0, _layers.Length);
        float timer = Random.Range(timerLimits.x, timerLimits.y);
        uint PlanetIndex = GetRandomPlanetIndex();
        PushIndexToUsedPlanet(PlanetIndex);
        float pos_y = Random.Range(Y_limits.x, Y_limits.y);
        Vector3 pos = transform.position;
        pos.y = pos_y;
        float koef = (scaleLimits.y - scaleLimits.x) / countLayers;
        float scale = scaleLimits.y - koef * layer;
        GameObject planet = Instantiate<GameObject>(planetPrefabs[PlanetIndex], pos, Quaternion.Euler(0f, 0f, 0f));
        float koef_speed = (speedLimits.y - speedLimits.x) / countLayers;
        float speed = speedLimits.y - koef_speed * layer;
        planet.GetComponent<Planet>().speed = speed;
        planet.GetComponent<SpriteRenderer>().sortingOrder = -10 - layer;
        planet.transform.localScale = new Vector3(scale, scale, 1);

        Invoke("createRandomPlanet", timer);
    }
}
