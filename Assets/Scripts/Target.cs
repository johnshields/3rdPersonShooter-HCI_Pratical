using System.Collections;
using UnityEngine;

public class Target : MonoBehaviour
{
    public GameObject banner;
    private void OnParticleCollision(GameObject other)
    {
        StartCoroutine(DestroyTarget());
    }

    private IEnumerator DestroyTarget()
    {
        yield return new WaitForSeconds(0.25f);
        print("Target hit!");
        banner.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        print("Target neutralized.");
        Destroy(gameObject);
        banner.SetActive(false);
        
    }
}
