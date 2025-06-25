using DG.Tweening;
using UnityEngine;
using System;

[Serializable]
public class InforSkill
{
    public string IDSkill;
    public GameObject skillPrefab;

    public InforSkill(string iDSkill, GameObject skillPrefab)
    {
        IDSkill = iDSkill;
        this.skillPrefab = skillPrefab;
    }
}

public class Skill : MonoBehaviour
{
    public string IDSkill;
    public bool used;
    public float speedMove = 0.5f;
    [SerializeField] private GameObject bullet;
    [SerializeField] private GameObject effect;
    [SerializeField] private new Collider collider;

    private Tween currentTween; // Tween đang chạy

    private void Reset()
    {
        if (string.IsNullOrEmpty(IDSkill)) IDSkill = Guid.NewGuid().ToString();
        collider = GetComponent<Collider>();
    }

    //void OnTriggerEnter(Collider other)
    //{
    //    //if (other.CompareTag("CharacterEnemy") && gameObject.CompareTag("SkillAlly"))
    //    //{
    //    //    currentTween?.Kill();

    //    //    bullet.SetActive(false);
    //    //    effect.SetActive(true);
    //    //    used = false;
    //    //    collider.enabled = false;
    //    //}

    //}

    void OnCollisionEnter(Collision collision)
    {
        //if (collision.gameObject.CompareTag("CharacterEnemy") && gameObject.CompareTag("SkillAlly"))
        //{
        //    currentTween?.Kill();

        //    bullet.SetActive(false);
        //    effect.SetActive(true);
        //    used = false;
        //}

        if (collision.gameObject.CompareTag("SkillEnemy") || gameObject.CompareTag("SkillAlly"))
        {
            currentTween?.Kill();

            bullet.SetActive(false);
            effect.SetActive(true);
            used = false;
            collider.enabled = false;
        }
    }

    public void MoveAttack(Vector3 posStart, Vector3 posEnd, Action moveComplete = null)
    {
        bullet.SetActive(false);
        effect.SetActive(false);
        collider.enabled = true;
        Vector3 direction = (posEnd - posStart).normalized;
        //Vector3 randomOffset = Vector3.Cross(direction, UnityEngine.Random.insideUnitSphere).normalized * UnityEngine.Random.Range(0.05f, 0.2f);
        Vector3 offset = new Vector3( UnityEngine.Random.Range(-0.2f, 0.2f),  UnityEngine.Random.Range(-0.05f, 0.2f), UnityEngine.Random.Range(0.01f, 0.15f));
        Vector3 randomOffset = Quaternion.LookRotation(direction) * offset;
        Vector3 midPoint = (posStart + posEnd) / 2f + randomOffset;
        Vector3[] path = new Vector3[] { posStart, midPoint, posEnd };

        transform.DOMove(posStart, 0).OnComplete(() =>
        {
            bullet.SetActive(true);
            currentTween = transform.DOPath(path, speedMove, PathType.CatmullRom)
                                    .SetEase(Ease.Linear)
                                    .OnComplete(() =>
                                    {
                                        bullet.SetActive(false);
                                        effect.SetActive(true);
                                        used = false;
                                        moveComplete?.Invoke();
                                        collider.enabled = false;
                                    });
        });
    }
}
