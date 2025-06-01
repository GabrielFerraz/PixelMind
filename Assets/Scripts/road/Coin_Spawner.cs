 using System.Collections;

using System.Collections.Generic;

using UnityEngine;



public class Coin_Spawner : MonoBehaviour

{

    public GameObject coinPrefab;

    
    void Start()

    {

        StartCoroutine(CoinSpawner());

    }



    

    void Update()

    {

        

    }



    void CoinSpawn()

    {

        float rand = Random.Range(-1.8f, 1.8f);

        Instantiate(coinPrefab,new Vector3(rand,transform.position.y,transform.position.z),Quaternion.identity);

    }



    IEnumerator CoinSpawner()

    {

        while(true)

        {

            int time = Random.Range(10, 20);

            yield return new WaitForSeconds(time);

            CoinSpawn();

        }

    }

}