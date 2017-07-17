using System.Collections;
using System.Collections.Generic;
using pointcache.Frustrum;
using UnityEngine;

public class ExampleSelectorUser : MonoBehaviour {

    FrustrumCameraSelector selector;

    private void Awake() {
        selector = GetComponent<FrustrumCameraSelector>();
        selector.OnSelected += OnSelected;
        selector.OnDeselected += OnDeselected;
    }

    void OnSelected(Collider go) {
        
    }

    void OnDeselected(Collider go) {
        
    }
}
