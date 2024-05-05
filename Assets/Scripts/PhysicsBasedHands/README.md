## Attribution
The scripts included inside [PhysicsBasedHands](https://github.com/oscardelgado02/VR-Prototype---Prospective-Memory-Training/edit/main/Assets/Scripts/PhysicsBasedHands) folder are property of [Amebous Labs](https://amebouslabs.medium.com/developing-physics-based-vr-hands-in-unity-cca4643c296b).

## Modifications
[PhysicsHand.cs](https://github.com/oscardelgado02/VR-Prototype---Prospective-Memory-Training/blob/main/Assets/Scripts/PhysicsBasedHands/PhysicsHand.cs) was modified.

- All the attributes inside the script are now private instead of public.
- It was added a functionality to change of layer when grabbing objects with the XRDirectInteractor. When not grabbing an object, the hand gameObject uses "defaultLayerName" layer (for example, this layer could interact with physics with all the layers). When grabbing an object, the hand gameObject uses "grabbingLayerName" layer (for example, this layer could interact with physics with all the layers but not with the layer of the object that you are grabbing). This way, you can avoid conflicts when grabbing objects.