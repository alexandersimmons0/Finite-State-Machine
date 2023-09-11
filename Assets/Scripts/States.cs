using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerState{
    void Enter(PlayerController controller);
    void Execute(PlayerController controller);
}

public class StandingState : IPlayerState{
    public void Enter(PlayerController controller){
        controller.currentState = this;
    }

    public void Execute(PlayerController controller){
        if(Input.GetKey(KeyCode.W)){
            ForwardState forward = new ForwardState();
            forward.Enter(controller);
        }
        if(Input.GetKey(KeyCode.S)){
            BackwardState backward = new BackwardState();
            backward.Enter(controller);
        }
        if(Input.GetKey(KeyCode.A)){
            LeftState left = new LeftState();
            left.Enter(controller);
        }
        if(Input.GetKey(KeyCode.D)){
            RightState right = new RightState();
            right.Enter(controller);
        }
        if(Input.GetKey(KeyCode.Space)){
            JumpingState jump = new JumpingState();
            jump.Enter(controller);
        }
        if(Input.GetKey(KeyCode.LeftShift)){
            DuckingState duck = new DuckingState();
            duck.Enter(controller);
        }
    }
}

public class DuckingState : IPlayerState{
    Rigidbody _rb;
    public void Enter(PlayerController controller){
        _rb = controller.GetComponent<Rigidbody>();
        _rb.transform.localScale = new Vector3(1,0.5f,1);
        controller.currentState = this;
    }

    public void Execute(PlayerController controller){
        if(!Input.GetKey(KeyCode.LeftShift)){
            _rb.transform.localScale = new Vector3(1,1,1);
            StandingState standing = new StandingState();
            standing.Enter(controller);
        }
    }
}

public class JumpingState : IPlayerState{
    Rigidbody _rb;
    float jumpTime;
    public void Enter(PlayerController controller){
        _rb = controller.GetComponent<Rigidbody>();
        _rb.velocity = new Vector3(0,10,0);
        jumpTime = Time.time;
        controller.currentState = this;
    }

    public void Execute(PlayerController controller){
        if(Physics.Raycast(_rb.transform.position, Vector3.down, 0.5f) && (Time.time - jumpTime > 0.5f)){
            Debug.Log("grounded");
            StandingState standing = new StandingState();
            standing.Enter(controller);            
        }
        if(Input.GetKey(KeyCode.LeftShift)){
            DivingState dive = new DivingState();
            dive.Enter(controller);
        }
    }
}

public class DivingState : IPlayerState{
    Rigidbody _rb;
    public void Enter(PlayerController controller){
        _rb = controller.GetComponent<Rigidbody>();
        _rb.AddForce(0, -10000 * Time.deltaTime, 0, ForceMode.VelocityChange);
        controller.currentState = this;
    }

    public void Execute(PlayerController controller){
        if(Physics.Raycast(_rb.transform.position, Vector3.down, 0.5f)){
            IPlayerState nextState;
            if(Input.GetKey(KeyCode.LeftShift)){
                nextState = new DuckingState();
            }else{
                nextState = new StandingState();
            }
            nextState.Enter(controller);
        }
    }
}

public class ForwardState : IPlayerState{
    Rigidbody _rb;
    public void Enter(PlayerController controller){
        _rb = controller.GetComponent<Rigidbody>();
        _rb.velocity = new Vector3 (0, 0, 5);
        controller.currentState = this;
    }

    public void Execute(PlayerController controller){
        if(!Input.GetKey(KeyCode.W)){
            StandingState standing = new StandingState();
            standing.Enter(controller);
        }
    }
}

public class BackwardState : IPlayerState{
    Rigidbody _rb;
    public void Enter(PlayerController controller){
        _rb = controller.GetComponent<Rigidbody>();
        _rb.velocity = new Vector3 (0, 0, -5);
        controller.currentState = this;
    }

    public void Execute(PlayerController controller){
        if(!Input.GetKey(KeyCode.S)){
            StandingState standing = new StandingState();
            standing.Enter(controller);
        }
    }
}

public class RightState : IPlayerState{
    Rigidbody _rb;
    public void Enter(PlayerController controller){
        _rb = controller.GetComponent<Rigidbody>();
        _rb.velocity = new Vector3 (5, 0, 0);
        controller.currentState = this;
    }

    public void Execute(PlayerController controller){
        if(!Input.GetKey(KeyCode.D)){
            StandingState standing = new StandingState();
            standing.Enter(controller);
        }
    }
}

public class LeftState : IPlayerState{
    Rigidbody _rb;
    public void Enter(PlayerController controller){
        _rb = controller.GetComponent<Rigidbody>();
        _rb.velocity = new Vector3 (-5, 0, 0);
        controller.currentState = this;
    }

    public void Execute(PlayerController controller){
        if(!Input.GetKey(KeyCode.A)){
            StandingState standing = new StandingState();
            standing.Enter(controller);
        }
    }
}