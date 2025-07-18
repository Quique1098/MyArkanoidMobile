using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BrickController : MonoBehaviour, ICustomUpdateable
{
    //Ladrillo con vida para multiples impactos y material que cambie
    public int health = 1; 
    public Material hitMaterial; 
    private Material originalMaterial; 
    private Renderer brickRenderer;

    [SerializeField] private bool isPowerUpEnabled; 
    [SerializeField] private GameObject powerUpPrefab; 

    //Lista de ladrillos que se mantiene actualizada para comprobar colision
    public static List<BrickController> activeBricks = new List<BrickController>();
    public static List<BrickController> allBricks = new List<BrickController>();

    public string id => $"{transform.position.x}_{transform.position.y}_{transform.position.z}";


    private void Start()
    {
        CustomUpdateManager.Instance.AddObject(this);

        brickRenderer = GetComponent<Renderer>();
        originalMaterial = brickRenderer.material;
    }

    private void OnDestroy()
    {
        CustomUpdateManager.Instance.RemoveObject(this);

    }



    private void OnEnable()
    {
        activeBricks.Add(this);
        if (!allBricks.Contains(this))
            allBricks.Add(this);
    }


    private void OnDisable()
    {
        
            activeBricks.Remove(this);

    }

    public void CustomStart()
    {

    }
    public void CustomUpdate() { }
    public void CustomFixedUpdate() { }
    public void CustomLateUpdate() { }
    public void CustomOnPause() { }

    public void TakeDamage()
    {

            GameEvents.OnBlockHit?.Invoke();
            
            health--;
            

            //Cambiamos material al recibir impacto
            if (hitMaterial != null)
                brickRenderer.material = hitMaterial;

            if (health <= 0)
            {
                DestroyBrick();
            }
            else
            {
                //Regresamos al material original despues del impacto
                Invoke("ResetMaterial", 0.1f);
            
            }
    }

    private void ResetMaterial()
    {
        brickRenderer.material = originalMaterial;
    }

    private void DestroyBrick()
    {
        GameEvents.OnBlockDestroyed?.Invoke();
        CustomUpdateManager.Instance.RemoveObject(this);
        activeBricks.Remove(this);

        // Sumar puntos
        ScoreManager.Instance.AddScore(10);

        // Instanciar power-up si corresponde
        if (isPowerUpEnabled && powerUpPrefab != null)
        {
            Instantiate(powerUpPrefab, transform.position, Quaternion.identity);
        }

        // Revisar victoria
        if (activeBricks.Count == 0)
        {
            GameManager.SaveGame();
            WinoLose.TriggerWin();
        }

        gameObject.SetActive(false);
    }


}

