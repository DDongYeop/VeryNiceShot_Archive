using System.Collections;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    [SerializeField] private GameObject obj;

    [SerializeField] private float delayTime = 2.5f;

    private void Start()
    {
        StartCoroutine(GenerateRoopCor());
    }

    private IEnumerator GenerateRoopCor()
    {
        while (true)
        {
            GameObject cloneObj = Instantiate(obj, transform);
            cloneObj.transform.position = transform.position;
            yield return new WaitForSeconds(delayTime);
        }
    }
}
