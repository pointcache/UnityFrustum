namespace pointcache.Frustrum {

    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    [RequireComponent(typeof(MeshCollider))]
    public class FrustrumMeshCollider : FrustrumBaseComponent {

        public bool Convex;
        protected new MeshCollider collider;

        protected override void Awake() {
            base.Awake();

            collider = GetComponent<MeshCollider>();
            collider.sharedMesh = frustrum.FrustrumMesh;
            collider.convex = true;
        }

        protected override void Update() {
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