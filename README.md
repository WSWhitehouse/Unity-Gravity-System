# Unity Gravity System
 
 Unity Gravity System - Local customizable gravity for planets and platforms, like Super Mario Galaxy. **Now with 2D version & Directional Gravity!**
 
 [![Image](https://i.gyazo.com/bc64b11dd1f73dc6bbb21a188de3cbca.gif)](https://gyazo.com/bc64b11dd1f73dc6bbb21a188de3cbca)
 
 [![Image](https://i.gyazo.com/eff8ec9c7a81d9058af8bcce01471892.gif)](https://gyazo.com/eff8ec9c7a81d9058af8bcce01471892)
 
 [![Image](https://i.gyazo.com/ac2345c1cbb311ddbf6dfe97b19084d8.gif)](https://gyazo.com/ac2345c1cbb311ddbf6dfe97b19084d8)

## Releases
- [**v1.1.0**](https://github.com/DoctorWolfy121/Unity-Gravity-System/releases/tag/v1.1.0) - Major Bug Fixes
- [**v1.0.0**](https://github.com/DoctorWolfy121/Unity-Gravity-System/releases/tag/v1.0.0) - First Release

## Getting Started

These instructions will get you a copy of the project up and running on your local machine for development and testing purposes.

### Prerequisites

- Unity 2018+ *Should be compatible with older versions*

### Installing

Read [INSTALLING.md](INSTALLING.md) for installing instructions.

## Known Issues
1. <s>If there are more than one gravity source at a time, the item might remove a current gravity source and not be affected.</s> - *Fixed in v1.1.0*
2. <s>Change `OnTriggerEnter()` to `OnTriggerStay()` so it can update the gravity source.</s> - *Fixed in v1.1.0*
3. Make the `CurrentGravitySource` a List so the item can be affected by multiple gravity sources at once, by calculating gravity strength and distance to gravity source.

## To Do
 - <s>Create a 2D version</s> - *Done*

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
