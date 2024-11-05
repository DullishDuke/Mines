using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Interact : MonoBehaviour
{
    [SerializeField] LayerMask mask;

	// Il void update viene eseguito ogni frame e controlla se premiamo
	// il tasto sinistro del mouse, se vero, genera un raycast che
	// parte dalla telecamera e va in direzione del mouse, se colpisce
	// oggetti con la stessa LayerMask "Tile", allora prende il
	// figlio (il gameObject "Text" nella seconda posizione dell'array dei figli
	// e lo rende attivo, mostrandone anche il rendering

	void Update()
    {
		if (Input.GetMouseButtonDown(0))
        {
			RaycastHit hitInfo;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out hitInfo, 100f, mask))
            {
				//Debug.DrawLine(ray.origin, hitInfo.point);
				//Debug.Log(hitInfo.transform.name);

                hitInfo.transform.GetChild(1).gameObject.SetActive(true);
			}
        }
    }
}
