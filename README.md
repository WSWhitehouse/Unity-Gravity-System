# Unity Gravity System
 
 Unity Gravity System - Local customizable gravity for planets and platforms, like Super Mario Galaxy. **Now with 2D version & Directional Gravity!** 
 
<p align="center">
<img src="https://i.gyazo.com/bc64b11dd1f73dc6bbb21a188de3cbca.gif" alt="Planet Gravity 3D">
<img src="https://i.gyazo.com/01b626b39834fd9a179a48c067901fcd.gif" alt="Directional Gravity 3D">
<img src="https://i.gyazo.com/ac2345c1cbb311ddbf6dfe97b19084d8.gif" alt="Planet Gravity 2D">
</p>

## Releases
For releases check the [releases on this repository](https://github.com/DoctorWolfy121/Unity-Gravity-System/releases), for the latest release [click here](https://github.com/DoctorWolfy121/Unity-Gravity-System/releases/latest)!

## Getting Started

These instructions will get you a copy of the project up and running on your local machine for development and testing purposes.

### Prerequisites

- Unity 2018+ *Should be compatible with older versions*

### Installing

Read [INSTALLING.md](INSTALLING.md) for installing instructions.

## Known Issues
1. <s>If there are more than one gravity source at a time, the item might remove a current gravity source and not be affected.</s> - *Fixed in v1.1.0*
2. <s>Change `OnTriggerEnter()` to `OnTriggerStay()` so it can update the gravity source.</s> - *Fixed in v1.1.0*
3. <s>Make the `CurrentGravitySource` a List so the item can be affected by multiple gravity sources at once, by calculating gravity strength and distance to gravity source.</s> - *Fixed in v2.0.0*

## To Do
 - <s>Create a 2D version</s> - *Completed in v1.0.0*
 - <s>Allow Gravity Items to be affected by multiple gravity sources</s> - *Completed in v2.0.0*
 - Add Rotate To Ground, where an item will always face upwards. - *WIP, estimated release v2.1.0*
 - Add Enable Gravity boolean that will enable and disable a Gravity Source - *WIP, estimated release v2.1.0*

## Built With

- [Unity 3D](https://unity.com/)
- [JetBrains Rider](https://www.jetbrains.com/rider/) - The IDE used to code the tools

## Versioning

We use [SemVer](http://semver.org/) for versioning. For the versions available, see the [tags on this repository](https://github.com/DoctorWolfy121/Unity-Gravity-System/tags). 

## Authors & Contributors

- **David Evans** - [*Initial work*](https://gist.github.com/phosphoer/a283cdbeca5d2160d5eed318d0362826) - [Phosphoer](https://github.com/phosphoer)
- **William Whitehouse** - *Examples, install instructions, bug fixes, code improvements/additions & 2D version* - [DoctorWolfy121](https://github.com/DoctorWolfy121)

## Acknowledgments

- [**Bayat Games**](https://assetstore.unity.com/publishers/26641) - [*Free 2D Game Assets used for 2D example*](https://assetstore.unity.com/packages/2d/environments/free-platform-game-assets-85838)

## License

This project is licensed under the MIT License - see the [LICENSE.md](LICENSE.md) file for details
