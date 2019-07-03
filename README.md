# PlanetGravity
 
 Planet like gravity.

## Getting Started

These instructions will get you a copy of the project up and running on your local machine for development and testing purposes. See deployment for notes on how to deploy the project on a live system.

### Prerequisites

- Unity

### Installing

1. Set Physics gravity to 0, otherwise the planet gravity will fight with global gravity.

[![Global Gravity Turned Off](https://i.gyazo.com/7f819af070ec7c4bccfac03a2524bd1e.png)](https://gyazo.com/7f819af070ec7c4bccfac03a2524bd1e)

2. Add `GravitySource` script to your planet/ground, along with a collider
3. Add `GravityItem` script to the player or an item (anything you want to be affected by gravity).
4. Make sure a collider and rigidbody is applied to the item.

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
