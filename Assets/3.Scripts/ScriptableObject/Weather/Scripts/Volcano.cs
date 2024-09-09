using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Volcano : RandomPosBase
{
    [SerializeField] 
    private List<GameObject> meteor = new List<GameObject>();

    [SerializeField] Transform spawnPos;
    private int maxCount;

    private void Awake()
    {
        FindTerrain();
    }

    private void Start()
    {
        maxCount = Random.Range(20, 40);
        StartCoroutine(SpawnMeteorsWithCooldown());
    }

    private IEnumerator SpawnMeteorsWithCooldown()
    {
        for (int i = 0; i < maxCount; i++)
        {
            SpawnMeteor();
            yield return new WaitForSeconds(Random.Range(5f, 15f));
        }
    }

    private void SpawnMeteor()
    {
        GameObject selectedMeteor = meteor[Random.Range(0, meteor.Count)];

        float randomX = Random.Range(89f, 90);
        float randomY = Random.Range(-180f, 180f);
        Quaternion quaternion = new Quaternion(randomX, randomY, 0f, 0f);
        GameObject meteorInstance =  Instantiate(selectedMeteor, spawnPos.position, quaternion);

        Debug.Log("X = " + randomX + "Y = " + randomY);

        Rigidbody rb = meteorInstance.GetComponent<Rigidbody>();
        float power = Random.Range(20f, 25f);
        Debug.Log(power);
        rb.AddRelativeForce(Vector3.up * power * 10, ForceMode.Impulse);
    }
}
