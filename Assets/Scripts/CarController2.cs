using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Game;
public class CarController2 : MonoBehaviour
{

    public Wheel[] wheels;
    public Vector2 moveInput;
    public float powerMultiplier = 1;
    public float maxSteer = 30, wheelbase = 2.5f, trackwidth = 1.5f;
    public float breakPower=1;
    float maxSpd = 80;

    public void OnMove(InputAction.CallbackContext ctx) // 이동 메서드(입력 들어가면 실행)
    {
        moveInput = ctx.ReadValue<Vector2>(); //입력받은 값을 vector2로 읽어들임
    }

    public void OnBreak(InputAction.CallbackContext ctx)
    {
        if (ctx.canceled) breakPower = 1;
        else breakPower = 0;
    }
    void FixedUpdate()
    {
        if (!MainManager.instance.isStart) return;
        // 현재 속도 (km/h 단위로 변환)
        float speed = wheels[0].collider.attachedRigidbody.linearVelocity.magnitude * 3.6f;
        float motor = moveInput.y * powerMultiplier;
        // 3. 속도 제한 (80km/h 초과 시 전진 토크 차단)
        if (speed > maxSpd && moveInput.y > 0)
        {
            motor = 0;
        }

        // 입력이 없을 때(중립 상태) 차가 미끄러지지 않도록 브레이크 부여
        float currentBrake = (moveInput.y == 0) || (breakPower == 0) ? (powerMultiplier * 0.5f) : 0;
        foreach (var wheel in wheels)  //각 바퀴의 휠콜라이더의 모터토크를 OnMove로 받아온 위아래값 * power만큼 돌림 
        {
            if (breakPower == 0)
            {
                wheel.collider.motorTorque = 0;

                // [스핀 방지 핵심] 앞바퀴 브레이크를 뒷바퀴보다 강하게 설정
                // 앞바퀴(front)는 bias 2.0, 뒷바퀴(rear)는 bias 0.5
                float bias = (wheel.wheelType == WheelType.front) ? 2.0f : 0.5f;
                wheel.collider.brakeTorque = powerMultiplier * bias;
            }
            else // 브레이크를 떼었을 때 (breakPower가 1일 때)
            {
                wheel.collider.brakeTorque = 0;
                wheel.collider.motorTorque = motor;

                // 엑셀도 떼고 브레이크도 안 밟은 상태(중립)에서 흐르는 것 방지 (선택 사항)
                if (moveInput.y == 0)
                {
                    wheel.collider.brakeTorque = 100f; // 약한 저항
                }
            }
        }

        print("spd: " + speed);
        // 속도가 높아질수록 maxSteer를 줄임 (예: 100km/h일 때 약 10도까지 감소)
        float dynamicSteer = Mathf.Lerp(maxSteer, 10f, speed / 80f);

        float steer = moveInput.x * dynamicSteer;
        if (moveInput.x > 0)  //x가 좌우 입력이므로 0보다 크면 우회전, 작으면 좌회전
        {
            wheels[0].collider.steerAngle = Mathf.Rad2Deg * Mathf.Atan(wheelbase / (trackwidth / 2 + Mathf.Tan(Mathf.Deg2Rad * steer) * wheelbase));
            //바깥쪽 바퀴를 더 작은 각도로 꺾이게 하기위한 조향각 식
            wheels[1].collider.steerAngle = steer; //오른쪽바퀴 최대치만큼 회전
        }
        else if (moveInput.x < 0)
        {
            wheels[0].collider.steerAngle = steer;//왼바퀴 최대치만큼 회전
            wheels[1].collider.steerAngle = Mathf.Rad2Deg * Mathf.Atan(wheelbase / (-trackwidth / 2 + Mathf.Tan(Mathf.Deg2Rad * steer) * wheelbase));
        }
        else
        {
            wheels[0].collider.steerAngle = wheels[1].collider.steerAngle = 0; //좌우 회전 없음
        }

        for (int i = 0; i < wheels.Length; i++)
        {
            Quaternion Rot;
            Vector3 Pos;
            wheels[i].collider.GetWorldPose(out Pos, out Rot); //휠 콜라이더의 월드 위치와 회전값을 Pos와 Rot으로 출력

            Transform[] ChildTranforms = new Transform[wheels[i].collider.transform.childCount]; //휠 콜라이더의 자식(바퀴모델링?) 갯수만큼 임의의 트랜스폼 배열생성?
            int index = 0;

            foreach (var item in ChildTranforms) //트랜스폼 갯수만큼 루프돌면서 휠 콜라이더의 자식(바퀴모델링)의 위치와 회전값을 수정
            {
                wheels[i].collider.transform.GetChild(index).position = Pos;
                wheels[i].collider.transform.GetChild(index).rotation = Rot;
                index++;
            }
        }
        AddDownForce();
    }

    [System.Serializable]
    public class Wheel  //바퀴 클래스 정의
    {
        public WheelCollider collider; //바퀴 콜라이더
        public WheelType wheelType;  //바퀴타입: 앞바퀴 뒷바퀴 설정
    }

    [System.Serializable]
    public enum WheelType //앞바퀴 뒷바퀴 구분용 enum. 그룹 이름 설정한거라 보면됨
    {
        front, rear
    }

    private void AddDownForce()
    {
        for (int i = 0; i < 4; i++)
        {
            wheels[i].collider.attachedRigidbody.AddForce(-transform.up * 100 *
                                                     wheels[i].collider.attachedRigidbody.linearVelocity.magnitude);
        }
    }

}

