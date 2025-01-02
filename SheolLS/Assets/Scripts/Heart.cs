using UnityEngine;

public class Heart : MonoBehaviour {
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject player;
    private int playerVitality;
    void Update() {
        if (player != null) {
             playerVitality = player.GetComponent<Player>().vitality;
        }
        switch (playerVitality) {
            case 1:
            animator.SetTrigger("DeadHeart");
            break;
            case 2:
            animator.SetTrigger("HurtHeart");
            break;
            case 3:
            animator.SetTrigger("GoodHeart");
            break;
        }
    }
}
