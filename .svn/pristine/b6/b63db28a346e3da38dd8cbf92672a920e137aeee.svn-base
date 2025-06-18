using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
	public delegate void onDistanceChange(float pct);
	public onDistanceChange OnDistanceChange;

	public List<GameObject> list_Spawn = new List<GameObject>();
    public Vector3 start;

    public static SpawnManager instance;

    int max_mon;
	int _remain_mon;

	public int remain_mon
    {
        get { return _remain_mon; }

        set 
        {
            _remain_mon = value;

            OnDistanceChange?.Invoke((max_mon - _remain_mon + 1) * 100 / max_mon * 0.01f);
		}
    }

	void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

		StageManager.OnStateChange += OnStateChange;

		StartCoroutine(Setting());
    }

    IEnumerator Setting()
    {
        while (true)
        {
            if (CsvManager.instance && SceneAddManager.instance && SceneAddManager.instance.mapScene.isLoaded)
            {
                string name = string.Format("{0}_{1}", StageManager.instance.num_Chapter + 1 , StageManager.instance.num_Stage + 1);

				remain_mon = max_mon = CsvManager.instance.GetStageMonsterList(name).Count;
                ResetSpawnSpot(max_mon);
				FindAndSortByDistance(4/*7*/, max_mon , 3);
				DeactivateAllSpawnTags();
                FindTarget();
                break;
            }

            yield return null;
        }
    }

    public void ResetSpawnSpot(int max)
    {
        remain_mon = max_mon = max;
	}

    private void Start()
    {
        Initialize();
    }

    void OnStateChange(STAGESTATE state)
    {
        switch (state)
        {
            case STAGESTATE.START:
                Initialize();
                break;
        }
    }

	private void Initialize()
	{
		

	}

	void FindAndSortByDistance(float distance = 10, int count = -1 , float first_dist = 0)
    {        
        list_Spawn.Clear();
             
        GameObject[] spawnObjects = GameObject.FindGameObjectsWithTag("SPAWN").ToArray();
                
        if (count != -1)
        {
            int currentCount = spawnObjects.Length;

            if (currentCount > count)
            {        
                for (int i = count; i < currentCount; i++)
                {
                    Destroy(spawnObjects[i]);
                }

                spawnObjects = spawnObjects.Take(count).ToArray();
            }
            else if (currentCount < count)
            {
                GameObject sample = spawnObjects.Length > 0 ? spawnObjects[0] : null;

                for (int i = currentCount; i < count; i++)
                {
                    if (sample != null)
                    {
                        GameObject newSpawn = Instantiate(sample);
                        newSpawn.name = $"{sample.name}_Clone_{i}";
                        spawnObjects = spawnObjects.Append(newSpawn).ToArray();
                    }
                } 
            }
        }

        System.Array.Sort(spawnObjects, (a, b) =>
        {
            float distA = Vector3.Distance(start, a.transform.position);
            float distB = Vector3.Distance(start, b.transform.position);
            return distA.CompareTo(distB);
        });

        for (int i = 0; i < spawnObjects.Length; i++)
        {
            var obj = spawnObjects[i];
            Vector3 direction = (obj.transform.position - start).normalized;
            obj.transform.position = start + direction * distance * (i + 1) + first_dist * direction;
        }

        list_Spawn.AddRange(spawnObjects);
    }

    void DeactivateAllSpawnTags()
    {
        GameObject[] spawnObjects = GameObject.FindGameObjectsWithTag("SPAWN");

        foreach (GameObject obj in spawnObjects)
        {
            obj.SetActive(false);
        }

        Debug.Log("All 'spawn' tag objects have been deactivated.");
    }

    public bool Is_Boss_Spawn(GameObject _nowTarget)
    {
        int idx = list_Spawn.IndexOf(_nowTarget);
        idx++;
        if (idx >= list_Spawn.Count)
        {
            return true;
        }

        return false;
    }

    public GameObject FindTarget(GameObject _nowTarget = null)
    {
        int idx = 0;

        if (_nowTarget == null)
        {
            idx = 0;
        }
        else
        {
            idx = list_Spawn.IndexOf(_nowTarget);
            idx++;
            if (idx >= list_Spawn.Count)
            {
                idx = 0;
                return null; //rsh_temp                
            }
        }

        if(idx >= 0 && idx < list_Spawn.Count)
        {
			remain_mon--;

			return list_Spawn[idx];
		}

		return null;
    }

}