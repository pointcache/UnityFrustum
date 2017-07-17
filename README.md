# UnityFrustrum

![](https://kek.gg/i/4tnVqZ.gif)
![](https://kek.gg/i/XnPTz.gif)
This little library implements frustrum in a form of a mesh.
It consists of a mesh generator, and components that use it.

# Why this

Many selection solutions on the market use techniques like bounds checking or unity event system and PhysicsRaycaster.
Both have limitations, it one case you would have to iterate over all objects in view, and then check against bounds,
which can be imprecise and require screen space convertion. Other approach is tied to unity event system and it's workflow,
requiring you to use additional logic for reception of events. 
This solution simply uses collider which is the most basic generic way without any additional logic.

Example usage:

* Create visible unit field of view
* Create actual field of view collider using FrustrumCollider
* Use frustrum collider + FrustrumCameraSelector to select objects in the world by dragging on the screen 
the selection box (the original requirement of this lib)
* Other uses.

# Usage

The frustrum is defined by several parameters:

* Vertical FOV
* Horizontal FOV
* Near plane distance
* Far plane distance

Additionally you shrink the current frustrum in its area using
* Extent min (from lower left corner, a 0-1 float value)
* Extent max 

![](http://i.imgur.com/ByEISk5.png)

# Using selection system

Selection system consists of a FrustrumCollider and FrustrumCameraSelector working in tandem.
Instructions:
  1. Attach FrustrumCameraSelector to camera
  2. It will auto setup the parameters in `Reset()`
  3. Add FrustrumMeshCollider to that camera so it can receive physics events.
  4. Drag FrustrumCameraSelector into `TakeParametersFrom` field of FrustrumMeshCollider, 
  this will make it sample the mesh generator settings from it.
  5. Make both UpdateInRealtime.
  6. Any object you wish to be able to select must have a rigidbody (kinematic will do)
  
  Now select camera, play and watch the selection mesh when you drag on the screen.
  Couple of things to note:
   * Collider must be a convex trigger
   * Generator should not be set to split vertex, this improves performance
   * Both selector and meshcollider work in fixed update due to it being tied to physics,
   meaning the selection itself is running at the physics framerate, it may be an issue in the future.
   

