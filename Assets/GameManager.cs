using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{

    public TextMeshProUGUI levelNameText;
    private Animator levelNameAnimator;
    // Start is called before the first frame update
    void Start()
    {
        levelNameAnimator = levelNameText.GetComponent<Animator>();
        levelNameAnimator.SetTrigger("LevelName");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
