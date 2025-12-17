
    using System;
    using UnityEngine;

    public class GameAudioManager : MonoBehaviour
    {
        public AudioSource shootAudioSource;

        public void Awake()
        {
            CameraShooter.OnShoot += CameraShooterOnShoot;
        }

        private void OnDestroy()
        {
            CameraShooter.OnShoot -= CameraShooterOnShoot;
        }

        private void CameraShooterOnShoot(int ammoLeft)
        {
            shootAudioSource.Play();
        }
    }
