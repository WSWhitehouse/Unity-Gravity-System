# Installing

## Downloading
1. Download the latest Unity Package. Check the [releases on this repository](https://github.com/DoctorWolfy121/Unity-Gravity-System/releases) to find the version you want, or [click here for the latest release](https://github.com/DoctorWolfy121/Unity-Gravity-System/releases/latest).
2. Extract the Unity Package in your project.

## Setting up your project

### 3D

*Optional - If you leave Global Gravity on, objects will still react as normal as well with the gravity sources, however you might want to make global gravity weaker or gravity sources stronger* - Set 3D global gravity to 0.

  - To change 3D physics, go to ProjectSettings -> Physics

  [![3D Gravity](https://i.gyazo.com/7f819af070ec7c4bccfac03a2524bd1e.png)](https://gyazo.com/7f819af070ec7c4bccfac03a2524bd1e)
  
### 2D  

*Optional - If you leave Global Gravity on, objects will still react as normal as well with the gravity sources, however you might want to make global gravity weaker or gravity sources stronger* - Set 2D global gravity to 0.
  
  - To change 2D physics, go to ProjectSettings -> Physics 2D

  [![2D Gravity](https://i.gyazo.com/7b173d32f32b0fa52a0a8a3ed9d1ee80.png)](https://gyazo.com/7b173d32f32b0fa52a0a8a3ed9d1ee80)
    
## Setting up an item

### 3D

1. Apply a `Collider` and `Rigidbody` to your item.
2. Make sure `Use Gravity` is **TRUE** on the rigidbody. If `Use Gravity` is false, it will not be affected by the gravity sources.
3. Add a `GravityItem` Script to the item.

### 2D

1. Apply a `Collider2D` and `Rigidbody2D` to your item.
2. Make sure `Gravity Scale` is **ABOVE 0** on the rigidbody. If `Gravity Scale` is below 0, it will not be affected by the gravity sources.
3. Add a `GravityItem2D` Script to the item.

## Setting up a Gravity Source

### 3D

1. Create an Empty GameObject in the centre of your planet or platform.
2. Add a `Collider` - *You can use any collider, but sphere colliders are best for planet gravity*
3. Make sure `Is Trigger` is **TRUE** on the collider.
4. You can add multiple colliders and they will all work as the gravity field. - *But they **MUST** be on this GameObject*
5. Add any gravity source script to the GameObject with the Colliders.
6. *Optional* - Set the `Gravity Strength` on the Gravity Source. The default gravity is Earth's Gravity
7. *Optional* - Set `Enable Debug` to true to see the radius of the Gravity Source and what objects are being affected by the Gravity Source.
8. *Optional* - On Directional Gravity Set `Gravity Direction` to control which way gravity will face. The default option is down.

[![Planet Gravity](https://i.gyazo.com/d2c6c3647bc17a2e7d603ce7c9e43274.png)](https://gyazo.com/d2c6c3647bc17a2e7d603ce7c9e43274)

### 2D

1. Create an Empty GameObject in the centre of your planet or platform.
2. Add a `Collider2D` - *You can use any collider, but circle colliders are best for planet gravity*
3. Make sure `Is Trigger` is **TRUE** on the collider 2D.
4. You can add multiple colliders and they will all work as the gravity field. - *But they **MUST** be on this GameObject*
5. Add any Gravity Source 2D script to the GameObject with the Colliders.
6. *Optional* - Set the `Gravity Strength` on the Gravity Source. The default gravity is Earth's Gravity
7. *Optional* - Set `Enable Debug` to true to see the radius of the Gravity Source and what objects are being affected by the Gravity Source.
8. *Optional* - On Directional Gravity Set `Gravity Direction` to control which way gravity will face. The default option is down.

[![Planet Gravity 2D](https://i.gyazo.com/b749667879a429a681d8f387a6fd4949.png)](https://gyazo.com/b749667879a429a681d8f387a6fd4949)

