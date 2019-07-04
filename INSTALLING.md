# Installing

## Setting up your project

### 3D

Set 3D global gravity to 0.

- To change 3D physics, go to ProjectSettings -> Physics

  [![3D Gravity](https://i.gyazo.com/7f819af070ec7c4bccfac03a2524bd1e.png)](https://gyazo.com/7f819af070ec7c4bccfac03a2524bd1e)
  
### 2D  

Set 2D global gravity to 0.
  
- To change 2D physics, go to ProjectSettings -> Physics 2D

  [![2D Gravity](https://i.gyazo.com/7b173d32f32b0fa52a0a8a3ed9d1ee80.png)](https://gyazo.com/7b173d32f32b0fa52a0a8a3ed9d1ee80)
    
## Setting up an item

### 3D

1. Apply a `Collider` and `Rigidbody` to your item.
2. Make sure `Use Gravity` is **TRUE** on the rigidbody. If `Use Gravity` is false, it will not be affected by the gravity sources.
3. Add a `GravityItem` Script to the item.
4. *Optional* - Set the `Current Gravity Source` to a gravity source in your scene, if you want the item to be affected by this gravity on start. It will automatically be affected by the gravity source if its within its gravity radius.

### 2D

*Comming Soon*

## Setting up a Gravity Source

### 3D

1. Create an Empty GameObject in the centre of your planet or platform.
2. Add a `Collider` - *This works best with a sphere collider, but all colliders will work*
3. Make sure `Is Trigger` is **TRUE** on the collider.
4. You can add multiple colliders and they will all work as the gravity field. - *But they **MUST** be on this GameObject*
5. Add the `GravitySource` script to the GameObject with the Colliders.
6. *Optional* - Set the `Gravity` on the Gravity Source. The default gravity is Earth's Gravity
7. Set the `Radius` on the Gravity Source. This is where it will check for objects on `Awake()`.
8. *Optional* - Set `Enable Debug` to true to see the radius of the Gravity Source and what objects are being affected by the Gravity Source.

[![Planet Gravity](https://i.gyazo.com/d2c6c3647bc17a2e7d603ce7c9e43274.png)](https://gyazo.com/d2c6c3647bc17a2e7d603ce7c9e43274)

### 2D
*Comming Soon*
