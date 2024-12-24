using UnityEngine;

public class Enemy : MonoBehaviour {
    public int vital = 1;
    void Update() {
        if(vital <= 0) {
            Destroy(gameObject);
        }
    }
}