namespace pointcache.Frustrum {

    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.EventSystems;

    public class FrustrumCameraSelector : FrustrumCamera {

        public float MinimalDragDistance = 10f;
        public Vector2 DragStartPoint { get { return m_initialScreenClick; } }
        public Vector2 mp;
        /// <summary>
        /// When the dragging actually begins, not from the moment you click, but from the moment the extents you drag
        /// grow larger than the minimal drag distance and the frustrum activates.
        /// 
        /// </summary>
        public System.Action OnStartDrag = delegate { };

        /// <summary>
        /// When you release the mouse.
        /// </summary>
        public System.Action OnEndDrag = delegate { };

        /// <summary>
        /// While you are dragging.
        /// </summary>
        public System.Action OnDrag = delegate { };

        public System.Action<Collider> OnSelected = delegate { };
        public System.Action<Collider> OnDeselected = delegate { };

        public Vector2 SelectionFrameAnchorsMin
        {
            get {
                Vector2 val = m_sortedExtents[0];
                val.x = val.x / Screen.width;
                val.y = val.y / Screen.height;
                return val;
            }
        }

        public Vector2 SelectionFrameAnchorsMax
        {
            get {
                Vector2 val = m_sortedExtents[1];
                val.x = val.x / Screen.width;
                val.y = val.y / Screen.height;
                return val;
            }
        }

        private HashSet<Collider> m_currentSelection_hash = new HashSet<Collider>();
        private HashSet<Collider> m_currentStay_hash = new HashSet<Collider>();
        [SerializeField]
        private List<Collider> m_currentSelection = new List<Collider>();

        public List<Collider> CurrentSelection { get { return m_currentSelection; } }

        private bool m_dragging;
        private bool m_draggingInValidZone;
        private Vector2 m_initialScreenClick;
        private Vector2[] m_sortedExtents = new Vector2[2];

        private void Reset() {
            m_config.UseExtents = true;
            m_config.SplitMeshVerts = false;
        }

        protected override void Awake() {
            base.Awake();
            m_config.Active = false;
        }

        protected override void Update() {

            base.Update();
            if (!m_dragging && EventSystem.current != null && EventSystem.current.IsPointerOverGameObject()) {
                return;
            }

            if (!m_dragging) {
                if (Input.GetKeyDown(KeyCode.Mouse0)) {
                    m_dragging = true;
                    m_initialScreenClick = Input.mousePosition;
                }
            }
            if (m_dragging) {
                if ((m_initialScreenClick - (Vector2)Input.mousePosition).sqrMagnitude > MinimalDragDistance) {
                    if (!m_draggingInValidZone) {
                        m_draggingInValidZone = true;
                        OnStartDrag();
                    }
                    m_config.Active = true;

                    SortExtents(m_initialScreenClick, Input.mousePosition);

                    frustrumConfig.ExtentsMin = ConvertScreenPosToExtents(m_sortedExtents[0]);
                    frustrumConfig.ExtentsMax = ConvertScreenPosToExtents(m_sortedExtents[1]);

                    OnDrag();
                }
                else {
                    m_config.Active = false;
                    if (m_draggingInValidZone) {
                        m_draggingInValidZone = false;
                        OnEndDrag();
                    }
                }
                if (Input.GetKeyUp(KeyCode.Mouse0)) {
                    m_dragging = false;
                    m_draggingInValidZone = false;
                    frustrumConfig.ExtentsMin = Vector3.zero;
                    frustrumConfig.ExtentsMax = Vector3.one;
                    m_config.Active = false;
                    OnEndDrag();
                }
            }
        }

        private void FixedUpdate() {
            if (!m_config.Active)
                return;
            for (int i = m_currentSelection.Count - 1; i > -1; i--) {
                if (!m_currentStay_hash.Contains(m_currentSelection[i])) {
                    OnDeselected(m_currentSelection[i]);
                    m_currentSelection_hash.Remove(m_currentSelection[i]);
                    m_currentSelection.RemoveAt(i);
                }
            }
            m_currentStay_hash.Clear();
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

        private void OnTriggerStay(Collider other) {
            m_currentStay_hash.Add(other);

            if (!m_currentSelection_hash.Contains(other)) {
                m_currentSelection.Add(other);
                m_currentSelection_hash.Add(other);
                OnSelected(other);
            }

        }
    }
}