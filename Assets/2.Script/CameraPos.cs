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

    [Header("Shake")]
    public float shakePower;
    public float shakeTime;

    public IEnumerator Shake()
    {
        Vector3 originalPos = transform.localPosition;
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
