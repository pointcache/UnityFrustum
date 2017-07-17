namespace pointcache.Frustrum {

    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    [RequireComponent(typeof(MeshCollider))]
    public class FrustrumMeshCollider : FrustrumBaseComponent {

        public bool Convex;
        protected new MeshCollider collider;

        private void Reset() {
            Convex = true;
            m_config.SplitMeshVerts = false;
        }

        protected override void Awake() {
            base.Awake();

            collider = GetComponent<MeshCollider>();
            collider.sharedMesh = frustrum.FrustrumMesh;
            
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

            collider.sharedMesh = frustrum.FrustrumMesh;
            collider.convex = Convex;
        }
    }
}