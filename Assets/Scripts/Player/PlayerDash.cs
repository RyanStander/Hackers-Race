using System;
using UnityEngine;

namespace Player
{
    public class PlayerDash : MonoBehaviour
    {
        [SerializeField] private PlayerManager playerManager;
        [SerializeField] private int totalDashes = 3;
        [SerializeField] private float dashSpeed = 15f;
        [SerializeField] private float dashCooldown = 3f;

        private float timeStamp;
        [SerializeField] private int dashCount;
        private bool cooldownActive;

        private PlayerController playerController;
        private Rigidbody playerRigidbody;
        private bool wishingToDash;
        
        [SerializeField] private AudioClip dashSound;

        private void OnValidate()
        {
            if (playerManager == null)
                playerManager = GetComponent<PlayerManager>();
        }

        private void Start()
        {
            playerController = playerManager.PlayerController;
            dashCount = totalDashes;
            playerRigidbody = playerManager.Rigidbody;
        }

        private void FixedUpdate()
        {
            if(wishingToDash)
            {
                Vector3 dashDirection =
                    Camera.main.transform.TransformDirection(GetDashDirection(playerController.Forward, playerController.Left));

                playerRigidbody.AddForce(dashDirection * dashSpeed, ForceMode.Impulse);
                playerManager.AudioSource.PlayOneShot(dashSound);

                wishingToDash = false;
            }
        }

        public void HandleDash()
        {
            if(cooldownActive)
                HandleCooldown();
            
            if (!playerController.DashInput)
                return;
            
            playerController.DashInput = false;
            
            if (dashCount <= 0 ||wishingToDash)
                return;

            dashCount--;
            
            if(!cooldownActive)
                timeStamp = Time.time + dashCooldown;

            cooldownActive = true;
            
            wishingToDash = true;
        }

        private void HandleCooldown()
        {
            if (dashCount >= totalDashes)
                return;

            if (timeStamp <= Time.time && cooldownActive)
            {
                dashCount++;
                timeStamp = Time.time + dashCooldown;
                if(dashCount >= totalDashes)
                    cooldownActive = false;
            }
        }

        private Vector3 GetDashDirection(float forward, float left)
        {
            return left switch
            {
                //I swapped my left and right around in my controller, but im too deep to change it now
                //so if its confusing, just ignore it.
                
                //is going left
                //is also going forward
                > 0 when forward > 0 =>
                    //dash diagonal left and forward
                    new Vector3(-0.5f, 0, 0.5f),
                > 0 when forward < 0 =>
                    //dash diagonal left and backwards
                    new Vector3(-0.5f, 0, -0.5f),
                > 0 =>
                    //dash left
                    Vector3.right,
                //is going right
                //is also going forward
                < 0 when forward > 0 =>
                    //dash diagonal right and forward
                    new Vector3(0.5f, 0, 0.5f),
                < 0 when forward < 0 =>
                    //dash diagonal right and backwards
                    new Vector3(0.5f, 0, -0.5f),
                < 0 =>
                    //dash right
                    Vector3.left,
                //dash backwards
                _ => forward < 0
                    ? Vector3.back
                    :
                    //otherwise we just go forward
                    Vector3.forward
            };
        }

        public void ResetDashes()
        {
            dashCount = 3;
            cooldownActive = false;
            timeStamp = 0;
        }
        
        public bool CanDash() => dashCount > 0;

        public int GetDashCount() => dashCount;

        public float GetDashRemainingCooldown() => timeStamp - Time.time;

        public float GetDashCooldownDuration() => dashCooldown;
        
        public bool IsCooldownActive() => cooldownActive;
    }
}
