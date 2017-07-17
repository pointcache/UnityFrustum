namespace pointcache.Frustrum {

    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    [RequireComponent(typeof(MeshCollider))]
    public class FrustrumMeshCollider : FrustrumBaseComponent {

        public bool Convex;
        private new MeshCollider collider;

        protected override void Awake() {
            base.Awake();

            collider = GetComponent<MeshCollider>();
            collider.sharedMesh = frustrum.FrustrumMesh;
            collider.convex = true;
        }

        protected override void Update() {
            base.Update();

            collider.sharedMesh = frustrum.FrustrumMesh;
            collider.convex = true;
        }
    }

}