namespace pointcache.Frustum {

    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class FrustumBaseComponent : MonoBehaviour {

        public bool DoGenerate;
        public bool RealtimeUpdate;
        public FrustumBaseComponent TakeParametersFrom;

        [SerializeField]
        protected FrustumConfiguration frustumConfig = new FrustumConfiguration();

        protected Frustum frustum;
        protected FrustumConfiguration m_config
        {
            get {
                FrustumConfiguration value = null;
                if (TakeParametersFrom != null) {
                    if (TakeParametersFrom.frustumConfig != null)
                        value = TakeParametersFrom.frustumConfig;
                    else
                        value = frustumConfig;
                }
                else
                    value = frustumConfig;
                return value;
            }
        }

        [System.Serializable]
        public class FrustumConfiguration {

            public bool Active = true;

            public float VerticalFov = 45, HorizontalFov = 45, NearPlaneDistance = 1, FarPlaneDistance = 3;

            public float MinimalHorizontalFov = 0.01f, MinimalVerticalFov = 0.01f;

            public float MinimalExtentsDimension = 0.01f;

            public bool SplitMeshVerts;

            public bool GenNear = true, GenFar = true, GenSides = true;

            public bool UseExtents;

            public Vector2 ExtentsMin = Vector2.zero, ExtentsMax = Vector3.one;

        }

        protected virtual void Awake() {

            frustum = new Frustum(
                                m_config.VerticalFov,
                                m_config.HorizontalFov,
                                m_config.NearPlaneDistance,
                                m_config.FarPlaneDistance,
                                m_config.SplitMeshVerts);

            frustum.SetMinimals(m_config.MinimalHorizontalFov, m_config.MinimalVerticalFov, m_config.MinimalExtentsDimension);

        }

        protected virtual void Update() {
            if (!m_config.Active)
                return;

            if (RealtimeUpdate)
                DoGenerate = true;

            if (DoGenerate) {
                Generate();
                DoGenerate = false;
            }

        }

        protected virtual void Generate() {

            frustum.SetParameters(
                    m_config.VerticalFov,
                    m_config.HorizontalFov,
                    m_config.NearPlaneDistance,
                    m_config.FarPlaneDistance);

            frustum.SetGenerationOptions(
                m_config.GenNear,
                m_config.GenFar,
                m_config.GenSides);

            if (m_config.UseExtents)
                frustum.GeneratePartial(
                    m_config.SplitMeshVerts,
                    m_config.ExtentsMin,
                    m_config.ExtentsMax);
            else
                frustum.GenerateFull(m_config.SplitMeshVerts);
        }
    }
}