using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveEnemy1 : MonoBehaviour
{
    public float enemySpeed = 1.0f;
    public int rangeDesplazamiento = 4;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = transform.position + new Vector3(enemySpeed * Time.deltaTime, 0, 0);
        if (transform.position.x >= rangeDesplazamiento)
        {
            enemySpeed = -enemySpeed;
            if (-transform.position.x >= rangeDesplazamiento)
            {
                enemySpeed = enemySpeed * -1;
                rangeDesplazamiento = rangeDesplazamiento * -1;
            }
        }
        
        
    }
}
