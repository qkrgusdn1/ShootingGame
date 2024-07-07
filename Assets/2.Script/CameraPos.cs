using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPos : MonoBehaviour
{
    #region ΩÃ±€≈Ê
    private static CameraPos instance;
    public static CameraPos Instance
    {
        get { return instance; }
    }
    #endregion
    void Awake()
    {
        instance = this;
    }
    Vector3 originalPos;
    [Header("Shake")]
    public float shakePower;
    public float shakeTime;

    private void Start()
    {
        originalPos = transform.localPosition;
    }

    public IEnumerator Shake()
    {
        
        float elapsed = 0;

        while (elapsed < shakeTime)
        {
            float x = originalPos.x + Random.Range(-shakePower, shakePower);
            float y = originalPos.y + Random.Range(-shakePower, shakePower);
            float z = originalPos.z;

            transform.localPosition = new Vector3(x, y, z);

            elapsed += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = originalPos;
    }
}
