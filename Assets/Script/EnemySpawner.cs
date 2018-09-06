using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {


    [SerializeField] List<WaveConfig> waveConfigs;

    [SerializeField] int startingWave = 0;
    [SerializeField] bool looping = false;


	// Use this for initialization
	IEnumerator Start () {

        do
        {
            yield return StartCoroutine(SpawnAllWave());
        }
        while (looping);
        
	}

    private IEnumerator SpawnAllEnimiesInWave(WaveConfig waveConfig)
    {
        for (int enemyCount = 0; enemyCount < waveConfig.getNumberOfEnemies(); enemyCount++)
        {
            var newEnemy = Instantiate(waveConfig.getEnemyPrefab(),
                waveConfig.getWayPoints()[0].transform.position, Quaternion.identity);
            newEnemy.GetComponent<EnemyPathing>().setWaveConfig(waveConfig);
            yield return new WaitForSeconds(waveConfig.getTimeBetweenSpawns());
        }
    }

    private IEnumerator SpawnAllWave()
    {
        for(int waveIndex = startingWave;waveIndex < waveConfigs.Count;waveIndex++)
        {
            var currentWave = waveConfigs[waveIndex];
            yield return StartCoroutine(SpawnAllEnimiesInWave(currentWave));

        }
    }

    
}
