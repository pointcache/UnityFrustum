namespace pointcache.Frustum {

    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    [RequireComponent(typeof(MeshCollider))]
    public class FrustumMeshCollider : FrustumBaseComponent {

        public bool Convex;
        protected new MeshCollider collider;

        private void Reset() {
            Convex = true;
            m_config.SplitMeshVerts = false;
        }

        protected override void Awake() {
            base.Awake();

            collider = GetComponent<MeshCollider>();
            collider.sharedMesh = frustum.FrustumMesh;
            
        }

        protected void FixedUpdate() {
            base.Update();
            if (!m_config.Active) {
                if (collider.enabled) {
                    collider.enabled = false;
                }
                return;
            }
            else {
                if (!collider.enabled)
                    collider.enabled = true;
            }

            collider.sharedMesh = frustum.FrustumMesh;
            collider.convex = Convex;
        }
    }
}