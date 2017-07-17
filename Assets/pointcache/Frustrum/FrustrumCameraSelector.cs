namespace pointcache.Frustrum {

    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class FrustrumCameraSelector : FrustrumCamera {

        public System.Action<GameObject> OnSelected = delegate { };
        public System.Action<GameObject> OnDeselected = delegate { };

        private HashSet<GameObject> m_currentSelection = new HashSet<GameObject>();

        private bool m_dragging;
        private Vector2 m_initialScreenClick;
        private Vector2[] m_sortedExtents = new Vector2[2];

        protected override void Awake() {
            base.Awake();
            m_config.Active = false;
        }

        protected override void Update() {
            base.Update();

            if (!m_dragging) {
                if (Input.GetKeyDown(KeyCode.Mouse0)) {
                    m_dragging = true;
                    m_initialScreenClick = Input.mousePosition;
                    m_config.Active = true;
                }
            }
            if (m_dragging) {
                SortExtents(ConvertScreenPosToExtents(m_initialScreenClick), ConvertScreenPosToExtents(Input.mousePosition));

                frustrumConfig.ExtentsMin = m_sortedExtents[0];
                frustrumConfig.ExtentsMax = m_sortedExtents[1];

                if (Input.GetKeyUp(KeyCode.Mouse0)) {
                    m_dragging = false;
                    frustrumConfig.ExtentsMin = Vector3.zero;
                    frustrumConfig.ExtentsMax = Vector3.one;
                    m_config.Active = false;

                }
            }
        }

        private void SortExtents(Vector3 min, Vector3 max) {

            float minX = Mathf.Min(min.x, max.x);
            float minY = Mathf.Min(min.y, max.y);
            float maxX = Mathf.Max(min.x, max.x);
            float maxY = Mathf.Max(min.y, max.y);

            m_sortedExtents[0] = new Vector2(minX, minY);
            m_sortedExtents[1] = new Vector2(maxX, maxY);

        }

        private Vector2 ConvertScreenPosToExtents(Vector2 pos) {

            pos.x = pos.x / Screen.width;
            pos.y = pos.y / Screen.height;
            return pos;

        }

        private void OnTriggerEnter(Collider other) {
           // OnSelected(other.gameObject);
        }

        private void OnTriggerExit(Collider other) {
           // OnSelected(other.gameObject);
        }

        private void OnTriggerStay(Collider other) {
            Debug.Log(other.name);
        }
    }
}