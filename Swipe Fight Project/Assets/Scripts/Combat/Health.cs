using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    Animator animator;
    Transform healthBarParent;
    Image healthBar;

    [SerializeField] private float maxHp = 100;
    private float currentHp;
    

    void Start()
    {
        animator = GetComponent<Animator>();
        healthBarParent = FindObjectOfType<Canvas>()
            .GetComponentsInChildren<Transform>(true).Where(t => t.name == tag + " Health Bar").First();
        healthBar = healthBarParent.GetComponentsInChildren<Image>(true).Where(i => i.name == "Health Bar").FirstOrDefault();

        healthBarParent.gameObject.SetActive(true);
        healthBar.gameObject.SetActive(true);

        currentHp = maxHp;
    }

    public void TakeDamage(float damage)
    {
        currentHp = Mathf.Max(currentHp - damage, 0);
        healthBar.fillAmount = currentHp / maxHp;

        if (currentHp == 0)
        {
            animator.SetTrigger("die");
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene(0);
    }

}
