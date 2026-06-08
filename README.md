# Backrooms Game

A Unity-based exploration game inspired by the liminal space aesthetic of the Backrooms. Navigate through procedurally generated yellow hallways with mobile touch controls.

## Features

- **Procedural Generation**: Infinite procedurally generated hallways
- **Mobile Controls**: Touch-based movement and camera control
- **Atmospheric Design**: Yellow hallway environment with fluorescent lighting
- **First-Person Exploration**: Immersive exploration gameplay

## Project Structure

```
Assets/
├── Scripts/
│   ├── Player/
│   │   ├── PlayerController.cs
│   │   └── MobileInputHandler.cs
│   ├── Generation/
│   │   ├── HallwayGenerator.cs
│   │   └── HallwaySegment.cs
│   └── UI/
│       └── TouchCanvas.cs
├── Prefabs/
│   ├── HallwaySegment.prefab
│   └── Player.prefab
├── Materials/
│   ├── YellowWall.mat
│   ├── Floor.mat
│   └── Ceiling.mat
├── Scenes/
│   └── MainScene.unity
└── Audio/
    └── (Ambient sounds - future)
```

## Getting Started

1. Clone the repository
2. Open the project in Unity 2021.3 LTS or newer
3. Open the MainScene from Assets/Scenes/
4. Press Play to test

## Controls (Mobile)

- **Left Side of Screen**: Movement (joystick area)
- **Right Side of Screen**: Camera Look (touch drag)

## Development Notes

- Target platforms: iOS, Android
- Uses TextMesh Pro for UI
- Rigidbody-based character controller
- Mobile-optimized mesh generation

## License

MIT
