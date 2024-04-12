using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour
{
    public enum InputMode
    {
        Mouse,
        Keyboard,
    }

    public InputMode inputMode;
    public PlayerInput playerInput;
    public NavMeshAgent agent;

    private Animator agentAnimater;
    private bool isWalking = false;

    RaycastHit rayHit;
    Vector2 keyVec;

    private void Start()
    {
        agentAnimater = agent.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (inputMode)
        {
            case InputMode.Mouse:
                if (Input.GetMouseButtonDown(0))
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(ray.origin, ray.direction, out rayHit))
                        agent.SetDestination(rayHit.point);

                    isWalking = true;
                    agentAnimater.SetBool("isWalking", true);
                }
                break;

            case InputMode.Keyboard:
                Vector3 dirVec = agent.transform.right * keyVec.x + agent.transform.forward * keyVec.y;
                agent.SetDestination(agent.transform.position + dirVec.normalized);

                isWalking = true;
                agentAnimater.SetBool("isWalking", true);
                break;
        }

        if (isWalking)
        {
            if (Vector3.Distance(agent.transform.position, agent.destination) < 0.1f)
            {
                isWalking = false;
                agentAnimater.SetBool("isWalking", false);
            }
        }
    }

    public void ActionMove(InputAction.CallbackContext context)
    {
        keyVec = context.ReadValue<Vector2>();
        Debug.Log(keyVec);
    }
}