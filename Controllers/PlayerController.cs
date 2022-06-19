using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float MovementSpeed = 5f;
    public int pixelsPerUnit = 1;
    public int Direction;
    public Rigidbody2D body;
    Vector2 Movement;
    public Animator animator;
    // Start is called before the first frame update
    void Awake()
    {
      body = GetComponent<Rigidbody2D>();  
      animator = GetComponent<Animator>();  
    }

    void Update(){
        if (DialogController.self.DialogRunning || InventoryController.self.InventoryActive || QuestLogController.self.QuestLogOpen || AddItemController.self.ObtainItemShowing){
            Movement.x = 0;
            Movement.y = 0;
        } else {
            Movement.x = Input.GetAxisRaw("Horizontal");
            Movement.y = Input.GetAxisRaw("Vertical");
        }
        animator.SetFloat("Horizontal", Movement.x);
        animator.SetFloat("Vertical", Movement.y);
        animator.SetFloat("Speed", Movement.SqrMagnitude());
        SetDirection();
        animator.SetFloat("Direction",(float)Direction);
    }

    void SetDirection(){
        if (Movement.SqrMagnitude() > 0){
            if (Movement.x == 0 && Movement.y == -1){
                Direction = 2;
            } else if (Movement.x == 0 && Movement.y == 1){
                Direction = 7;
            } else if (Movement.x == -1 && Movement.y == 0){
                Direction = 4;
            } else if (Movement.x == 1 && Movement.y == 0){
                Direction = 5;
            } else if (Movement.x == -1 && Movement.y == 1){
                Direction = 8;
            } else if (Movement.x == 1 && Movement.y == 1){
                Direction = 6;
            } else if (Movement.x == -1 && Movement.y == -1){
                Direction = 1;
            } else if (Movement.x == 1 && Movement.y == -1){
                Direction = 3;
            }
        }
    }

    public void UpdatePosition(){
        transform.position = transform.localPosition;
    }

    // Update is called once per frame
    void FixedUpdate()
    {   
        body.MovePosition(body.position + Movement * MovementSpeed * Time.fixedDeltaTime);
    }
}
