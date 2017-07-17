namespace pointcache.Frustrum {

    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class FrustrumBaseComponent : MonoBehaviour {

        public bool DoGenerate;
        public bool RealtimeUpdate;
        public FrustrumBaseComponent TakeParametersFrom;

        [SerializeField]
        protected FrustrumConfiguration frustrumConfig = new FrustrumConfiguration();

        protected Frustrum frustrum;
        protected FrustrumConfiguration m_config
        {
            get {
                FrustrumConfiguration value = null;
                if (TakeParametersFrom != null) {
                    if (TakeParametersFrom.frustrumConfig != null)
                        value = TakeParametersFrom.frustrumConfig;
                    else
                        value = frustrumConfig;
                }
                else
                    value = frustrumConfig;
                return value;
            }
        }

        [System.Serializable]
        public class FrustrumConfiguration {

            public float VerticalFov = 45, HorizontalFov = 45, NearPlaneDistance = 1, FarPlaneDistance = 3;

            public bool SplitMeshVerts;

            public bool GenNear = true, GenFar = true, GenSides = true;

            public bool UseExtents;

            public Vector2 ExtentsMin = Vector2.zero, ExtentsMax = Vector3.one;

        }

        protected virtual void Awake() {

            frustrum = new Frustrum(
                                m_config.VerticalFov,
                                m_config.HorizontalFov,
                                m_config.NearPlaneDistance,
                                m_config.FarPlaneDistance,
                                m_config.SplitMeshVerts);

        }

        protected virtual void Update() {

            if (RealtimeUpdate)
                DoGenerate = true;

            if (DoGenerate) {
                Generate();
                DoGenerate = false;
            }

        }

        protected virtual void Generate() {

            frustrum.SetParameters(
                    m_config.VerticalFov,
                    m_config.HorizontalFov,
                    m_config.NearPlaneDistance,
                    m_config.FarPlaneDistance);

            frustrum.SetGenerationOptions(
                m_config.GenNear,
                m_config.GenFar,
                m_config.GenSides);

            if (m_config.UseExtents)
                frustrum.GeneratePartial(
                    m_config.SplitMeshVerts,
                    m_config.ExtentsMin,
                    m_config.ExtentsMax);
            else
                frustrum.GenerateFull(m_config.SplitMeshVerts);
        }
    }
}