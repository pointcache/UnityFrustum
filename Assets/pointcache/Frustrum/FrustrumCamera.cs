namespace pointcache.Frustrum {

    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;



    public class FrustrumCamera : FrustrumBaseComponent {

        public Camera targetCamera;

        private void Reset() {
            if (targetCamera == null)
                targetCamera = GetComponent<Camera>();
            SyncWithCamera();
        }

        public void SyncWithCamera() {

            if (!targetCamera)
                return;

            float vfov = frustrumConfig.VerticalFov = targetCamera.fieldOfView;

            var radAngle = targetCamera.fieldOfView * Mathf.Deg2Rad;
            var radHFOV = 2 * Mathf.Atan(Mathf.Tan(radAngle / 2) * targetCamera.aspect);
            var hfov = Mathf.Rad2Deg * radHFOV;

            frustrumConfig.HorizontalFov = hfov;
            frustrumConfig.NearPlaneDistance = targetCamera.nearClipPlane;
            frustrumConfig.FarPlaneDistance = targetCamera.farClipPlane;

        }

        protected override void Update() {
            SyncWithCamera();
            base.Update();
        }
    }

}