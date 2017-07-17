using System.Collections;
using System.Collections.Generic;
using pointcache.Frustrum;
using UnityEngine;

public class ExampleSelectorUser : MonoBehaviour {

    FrustrumCameraSelector selector;

    private void Awake() {
        selector = GetComponent<FrustrumCameraSelector>();
        selector.OnSelected += OnSelected;
        selector.OnDeselected += OnSelected;
    }

    void OnSelected(GameObject go) {
        Debug.Log("Selected : " + go.name);
    }

    void OnDeselected(GameObject go) {
        Debug.Log("Deselected : " + go.name);
    }
}
