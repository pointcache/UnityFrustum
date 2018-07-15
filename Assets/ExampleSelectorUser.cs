using System.Collections;
using System.Collections.Generic;
using pointcache.Frustum;
using UnityEngine;

public class ExampleSelectorUser : MonoBehaviour {

    FrustumCameraSelector selector;

    private void Awake() {
        selector = GetComponent<FrustumCameraSelector>();
        selector.OnSelected += OnSelected;
        selector.OnDeselected += OnDeselected;
    }

    void OnSelected(Collider go) {
        go.SendMessage("Selected");
    }

    void OnDeselected(Collider go) {
        go.SendMessage("DeSelected");
    }
}
