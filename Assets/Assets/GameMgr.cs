using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using System.Security.Cryptography;

public class GameMgr : MonoBehaviour
{
    [SerializeField] Transform pointsHolder;
    [SerializeField] float timeBetweenBlinks;
    [SerializeField] TextMeshProUGUI textCountdown;
    [SerializeField] Light dirLight;
    [SerializeField] GameObject[] propPrefabs;
    [SerializeField] GameObject holdPropUI;
    
    List<GameObject> activeProps = new List<GameObject>();
    List<GameObject> targetPropPrefabs = new List<GameObject>();

    internal static System.Action OnTimeForBlink;
    internal static System.Action PropsReshuffled;
    internal static GameMgr instance;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        ToggleHoldPropUI(false);
        SpawnProps();
        ReshuffleProps();
        ChooseRndPropAsTarget();
        StartCoroutine(CountdownToBlink());
    }

    private void ChooseRndPropAsTarget()
    {
        activeProps[Random.Range(0, activeProps.Count)].GetComponent<Prop>().isTarget = true;
    }

    public IEnumerator CountdownToBlink()
    {
        int secLeft = (int)timeBetweenBlinks;
        textCountdown.text = "0:" + secLeft.ToString("00");
        textCountdown.gameObject.SetActive(true);
        while (secLeft > 0)
        {
            yield return new WaitForSeconds(1);
            secLeft--;
            textCountdown.text = "0:" + secLeft.ToString("00");
        }
        textCountdown.gameObject.SetActive(false);
        OnTimeForBlink?.Invoke();
    }

    private void SpawnProps()
    {
        targetPropPrefabs.AddRange(propPrefabs);

        for (int i = 0; i < pointsHolder.childCount; i++)
        {
            GameObject prefab = propPrefabs[Random.Range(0, propPrefabs.Length)];
            activeProps.Add(Instantiate(prefab));

            // remove to have only whats left for target
            targetPropPrefabs.Remove(prefab);
        }
    }

    public void ReshuffleProps()
    {
        GameObject[] arr = activeProps.ToArray();
        System.Random random = new System.Random();
        arr = arr.OrderBy(x => random.Next()).ToArray();
        activeProps.Clear();
        activeProps.AddRange(arr);

        for (int i = 0; i < activeProps.Count; i++)
        {
            if (activeProps[i].GetComponent<Prop>().isTarget == true)
            {
                GameObject oldProp = activeProps[i];
                activeProps[i] = Instantiate(GetNextTargetPrefab());
            }

            activeProps[i].transform.position = pointsHolder.GetChild(i).position;
            activeProps[i].transform.rotation = Quaternion.Euler(new Vector3(0, Random.Range(0,360), 0));
        }

        NextCycleDirLight();
        PropsReshuffled?.Invoke();
    }

    int dirLightCycle = 0;
    private void NextCycleDirLight()
    {
        dirLightCycle++;
        if (dirLightCycle > 3)
            dirLightCycle = 0;

        float newTemp, newIntensity;
        switch (dirLightCycle)
        {
            default:
                newTemp = 2300;
                newIntensity = 1.5f;
                break;
            case 1:
                newTemp = 6000;
                newIntensity = 2f;
                break;
            case 2:
                newTemp = 13931;
                newIntensity = 0.3f;
                break;
        }
        dirLight.colorTemperature = newTemp;
        dirLight.intensity = newIntensity;
    }

    public void ToggleHoldPropUI(bool newState)
    {
        holdPropUI.SetActive(newState);
    }

    int cycleTargetPrefab = 0;
    private GameObject GetNextTargetPrefab()
    {
        cycleTargetPrefab++;
        if (cycleTargetPrefab >= targetPropPrefabs.Count)
            cycleTargetPrefab = 0;

        return targetPropPrefabs[cycleTargetPrefab];
    }
}
