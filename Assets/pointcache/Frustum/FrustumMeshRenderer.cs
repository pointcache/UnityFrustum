namespace pointcache.Frustum {

    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    [RequireComponent(typeof(MeshRenderer))]
    [RequireComponent(typeof(MeshFilter))]
    public class FrustumMeshRenderer : FrustumBaseComponent {

        private new MeshRenderer renderer;
        private MeshFilter filter;

        protected override void Awake() {

            base.Awake();

            renderer = GetComponent<MeshRenderer>();
            filter = GetComponent<MeshFilter>();
            filter.mesh = frustum.FrustumMesh;

        }

    }

}