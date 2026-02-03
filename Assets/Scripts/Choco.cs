using UnityEngine;
using System.Collections.Generic;

public class Choco : MonoBehaviour
{
    [Range(0.5f, 1f)]
    public float slowMultiplier = 0.9999f;

    // ✅ 진입 순간 속도가 0으로 잡히는 문제 방지용(프로젝트에 맞게 조절)
    public float minBaseSpeed = 6f;

    private readonly Dictionary<Rigidbody, float> baseMaxSpeedXZ = new Dictionary<Rigidbody, float>();

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.parent == null) return;
        if (!other.transform.parent.CompareTag("Player")) return;

        var player = other.transform.parent;
        player.SetParent(transform);

        var rb = player.GetComponent<Rigidbody>();
        if (rb == null) return;

        Vector3 v = rb.linearVelocity;
        float xzSpeed = new Vector3(v.x, 0f, v.z).magnitude;

        // ✅ 최소 기준 보장
        xzSpeed = Mathf.Max(xzSpeed, minBaseSpeed);

        if (!baseMaxSpeedXZ.ContainsKey(rb))
            baseMaxSpeedXZ.Add(rb, xzSpeed);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.transform.parent == null) return;
        if (!other.transform.parent.CompareTag("Player")) return;

        var rb = other.transform.parent.GetComponent<Rigidbody>();
        if (rb == null) return;
        if (!baseMaxSpeedXZ.TryGetValue(rb, out float baseMax)) return;

        Vector3 v = rb.linearVelocity;
        float currentXZ = new Vector3(v.x, 0f, v.z).magnitude;

        // ✅ “진입 순간 0 저장”을 이후 프레임에서 자동 보정
        baseMax = Mathf.Max(baseMax, currentXZ, minBaseSpeed);
        baseMaxSpeedXZ[rb] = baseMax;

        float maxXZ = baseMax * slowMultiplier;

        Vector3 xz = new Vector3(v.x, 0f, v.z);
        if (xz.magnitude > maxXZ && xz.magnitude > 0.0001f)
        {
            xz = xz.normalized * maxXZ;
            rb.linearVelocity = new Vector3(xz.x, v.y, xz.z);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.parent == null) return;
        if (!other.transform.parent.CompareTag("Player")) return;

        var rb = other.transform.parent.GetComponent<Rigidbody>();
        if (rb != null) baseMaxSpeedXZ.Remove(rb);

        other.transform.parent.SetParent(null);
    }
}
