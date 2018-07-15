namespace pointcache.Frustum {

    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;



    public class FrustumCamera : FrustumBaseComponent {

        public Camera targetCamera;

        private void Reset() {
            if (targetCamera == null)
                targetCamera = GetComponent<Camera>();
            SyncWithCamera();
        }

        public void SyncWithCamera() {

            if (!targetCamera)
                return;

            float vfov = frustumConfig.VerticalFov = targetCamera.fieldOfView;

            var radAngle = targetCamera.fieldOfView * Mathf.Deg2Rad;
            var radHFOV = 2 * Mathf.Atan(Mathf.Tan(radAngle / 2) * targetCamera.aspect);
            var hfov = Mathf.Rad2Deg * radHFOV;

            frustumConfig.HorizontalFov = hfov;
            frustumConfig.NearPlaneDistance = targetCamera.nearClipPlane;
            frustumConfig.FarPlaneDistance = targetCamera.farClipPlane;

        }

        protected override void Update() {
            SyncWithCamera();
            base.Update();
        }
    }

}