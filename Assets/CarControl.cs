using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarControl : MonoBehaviour
{
    //Звуки
    [SerializeField]
    private AudioSource IdleSource, MovementSource, BrakeSource, NitroSource;
    //Тралалело тралала
    [SerializeField]
    private float _motor = 800, _steer = 50, _brake = 440, _motorCar, _motorNitro = 4800;
    // Колеса автомобиля
    [SerializeField]
    private WheelInfo _FL, _FR, _BL, _BR;
    // Оси движения 
    private float _vert;
    private float _horz;
    // Позиция колеса
    private Vector3 _position;
    // Поворот колеса
    private Quaternion _rotation;
    // Частицы
    [SerializeField]
    private List<ParticleSystem> _brakeSystem = new List<ParticleSystem>();
    [SerializeField]
    private List<ParticleSystem> _nitroSystem = new List<ParticleSystem>();

    void UpdateVisualWheels()
    {
        _FL.wheelcollider.GetWorldPose(out _position, out _rotation);
        _FL.visualwheel.position = _position;
        _FL.visualwheel.rotation = _rotation;
        //
        _FR.wheelcollider.GetWorldPose(out _position, out _rotation);
        _FR.visualwheel.position = _position;
        _FR.visualwheel.rotation = _rotation;
        //
        _BL.wheelcollider.GetWorldPose(out _position, out _rotation);
        _BL.visualwheel.position = _position;
        _BL.visualwheel.rotation = _rotation;
        //
        _BR.wheelcollider.GetWorldPose(out _position, out _rotation);
        _BR.visualwheel.position = _position;
        _BR.visualwheel.rotation = _rotation;
    }
    void Awake()
    {
        _motorCar = _motor;
    }
    private void Update()
    {
        // Получаем значение оси
        _vert = Input.GetAxis("Vertical");
        _horz = Input.GetAxis("Horizontal");

        if(_vert != 0)
        {
            IdleSource.Pause();
            MovementSource.UnPause();
        }
        else
        {
            IdleSource.UnPause();
            MovementSource.Pause();
        }
    }

    private void FixedUpdate()
    {
        // Тормоз -- левый Shift
        if (Input.GetKey(KeyCode.LeftShift))
        {
            _FL.wheelcollider.brakeTorque = _brake;
            _FR.wheelcollider.brakeTorque = _brake;
            _BL.wheelcollider.brakeTorque = _brake;
            _BR.wheelcollider.brakeTorque = _brake;
            for(int i = 0; i < _brakeSystem.Count; i++)
            {
                _brakeSystem[i].Play();
            }

            if (!BrakeSource.isPlaying)
            {
                BrakeSource.PlayOneShot(BrakeSource.clip);
            }

        }
        else
        {
            _FL.wheelcollider.brakeTorque = 0;
            _FR.wheelcollider.brakeTorque = 0;
            _BL.wheelcollider.brakeTorque = 0;
            _BR.wheelcollider.brakeTorque = 0;
            for (int i = 0; i < _brakeSystem.Count; i++)
            {
                _brakeSystem[i].Stop();
            }
            BrakeSource.Stop();
        }
        if (Input.GetButton("Jump"))
        {
            _motorCar = _motorNitro;
            for (int i = 0; i < _nitroSystem.Count; i++)
            {
                _nitroSystem[i].Play();
            }
            if (!NitroSource.isPlaying)
            {
                NitroSource.PlayOneShot(NitroSource.clip);
            }
        }
        else
        {
            _motorCar = _motor;
            for (int i = 0; i < _nitroSystem.Count; i++)
            {
                _nitroSystem[i].Stop();
            }
            NitroSource.Stop();
        }

        _FL.wheelcollider.steerAngle = _horz * _steer;
        _FR.wheelcollider.steerAngle = _horz * _steer;
        _BL.wheelcollider.motorTorque = _vert * _motorCar;
        _BR.wheelcollider.motorTorque = _vert * _motorCar;
        UpdateVisualWheels();
    }


}
[System.Serializable]
public struct WheelInfo
{
    // Трансформация колеса
    public Transform visualwheel;
    // Коллайдер колеса
    public WheelCollider wheelcollider;
}
