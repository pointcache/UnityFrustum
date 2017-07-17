# UnityFrustrum

![](https://kek.gg/i/4tnVqZ.gif)

This little library implements frustrum in a form of a mesh.
It consists of a mesh generator, and components that use it.

Example usage:

* Create visible unit field of view
* Create actual field of view using FrustrumCollider
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

