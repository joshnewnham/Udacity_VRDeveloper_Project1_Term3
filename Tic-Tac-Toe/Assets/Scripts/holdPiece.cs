using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class holdPiece : MonoBehaviour {
    public GameLogic GameLogic;
    public GameObject raycastHolder;
    public GameObject player;
    public GameObject pieceBeingHeld;
	public GameObject gravityAttractor;

    public bool holdingPiece = false;
    public float hoverHeight = 0.3f;

    private Rigidbody pieceBeingHeldRigidbody;
    private PlayerPiece pieceBeingHeldPlayerPiece;
    private BoxCollider pieceBeingHeldBoxCollider;

    RaycastHit hit;
	public float gravityFactor = 10.0f;
	private Vector3 forceDirection;

    // Use this for initialization
    void Start () {
		
	}
	public void grabPiece(GameObject selectedPiece) {
        if (selectedPiece.GetComponent<PlayerPiece>().hasBeenPlayed == false) {
            pieceBeingHeld = selectedPiece;

            if(pieceBeingHeld != null){
                pieceBeingHeldRigidbody = pieceBeingHeld.GetComponent<Rigidbody>();
                pieceBeingHeldBoxCollider = pieceBeingHeld.GetComponent<BoxCollider>();
                pieceBeingHeldPlayerPiece = pieceBeingHeld.GetComponent<PlayerPiece>();
            }
            holdingPiece = true;
        }
    }
	
    private void FixedUpdate()
    {
        if (GameLogic.playerTurn)
        {
            if (holdingPiece)
            {
                Vector3 forwardDir = raycastHolder.transform.TransformDirection(Vector3.forward) * 100;
                Debug.DrawRay(raycastHolder.transform.position, forwardDir, Color.green);

                if (Physics.Raycast(raycastHolder.transform.position, (forwardDir), out hit))
                {
                    gravityAttractor.transform.position = new Vector3(hit.point.x, hit.point.y + hoverHeight, hit.point.z);

                    pieceBeingHeldRigidbody.useGravity = false;
                    pieceBeingHeldBoxCollider.enabled = false;

                    //pieceBeingHeld.GetComponent<Rigidbody>().AddForce(gravityAttractor.transform.position - pieceBeingHeld.transform.position);
                    //pieceBeingHeld.transform.position = Vector3.Lerp(
                    //pieceBeingHeld.transform.position, gravityAttractor.transform.position, 1.3f * Time.deltaTime);

                    pieceBeingHeld.transform.position += (gravityAttractor.transform.position - pieceBeingHeld.transform.position) * 2.0f * Time.deltaTime;

                    if (hit.collider.gameObject.tag == "Grid Plate")
                    {
                        if (Input.GetMouseButtonDown(0))
                        {
                            holdingPiece = false;
                            hit.collider.gameObject.SetActive(false);
                            pieceBeingHeldPlayerPiece.hasBeenPlayed = true;
                            pieceBeingHeldRigidbody.useGravity = true;
                            pieceBeingHeldRigidbody.drag *= 1.2f;
                            pieceBeingHeldRigidbody.mass *= 1.2f; 
                            pieceBeingHeldBoxCollider.enabled = true;
                            GameLogic.playerMove(hit.collider.gameObject);
                        }

                    }

                }
            }
        }
    }

}








