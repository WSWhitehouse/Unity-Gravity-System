# PlanetGravity
 
 Local gravity, examples:
 
 [![Image](https://i.gyazo.com/bc64b11dd1f73dc6bbb21a188de3cbca.gif)](https://gyazo.com/bc64b11dd1f73dc6bbb21a188de3cbca)
 
 [![Image](https://i.gyazo.com/eff8ec9c7a81d9058af8bcce01471892.gif)](https://gyazo.com/eff8ec9c7a81d9058af8bcce01471892)

## Getting Started

These instructions will get you a copy of the project up and running on your local machine for development and testing purposes. See deployment for notes on how to deploy the project on a live system.

### Prerequisites

- Unity

### Installing

1. Set Physics gravity to 0, otherwise the planet gravity will fight with global gravity.

[![Global Gravity Turned Off](https://i.gyazo.com/7f819af070ec7c4bccfac03a2524bd1e.png)](https://gyazo.com/7f819af070ec7c4bccfac03a2524bd1e)

2. Add `GravitySource` script to your planet/ground, along with a trigger collider

3. Everything inside the trigger collider will be affected by this planets gravity. Splitting out the components can help: 

[![Image](https://i.gyazo.com/7b751229a03c2c25c5315da0268d12cd.png)](https://gyazo.com/7b751229a03c2c25c5315da0268d12cd)

[![Image](https://i.gyazo.com/9edb23a6623827479b3f0fdce03a5410.png)](https://gyazo.com/9edb23a6623827479b3f0fdce03a5410)

The `Gravity` field is how strong the gravity is on this planet, making it negative will push items away. The `Radius` field is to check for items on `Awake()` (note: the radius in the gravity source is the same radius of the sphere collider). The `Gravity Colliders` array needs to be occupied with all the trigger colliders that will be used for gravity (note: gravity will always go to the centre of the collider).

4. Add `GravityItem` script to the player or an item (anything you want to be affected by gravity).
5. Make sure a collider and rigidbody is applied to the item.

## Built With

- [Unity 3D](https://unity.com/)
- [JetBrains Rider](https://www.jetbrains.com/rider/) - The IDE used to code the tools

## Versioning

We use [SemVer](http://semver.org/) for versioning. For the versions available, see the [tags on this repository](https://github.com/your/project/tags). 

## Authors

- **David Evans** - *Initial work* - [Phosphoer](https://github.com/phosphoer)
- **William Whitehouse** - *Example and install instructions* - [DoctorWolfy121](https://github.com/DoctorWolfy121)

## License

This project is licensed under the MIT License - see the [LICENSE.md](LICENSE.md) file for details
