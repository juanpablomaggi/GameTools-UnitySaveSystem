# GameTools-UnitySaveSystem

A lightweight and flexible save system for Unity, designed to be easy to integrate and adaptable to different project needs.

## Features

- Simple and modular architecture.
- Methods to save, load, and delete game data.
- Fully integrated with the Unity Test Framework.
- Flexible design for different save/load strategies.

---

## Getting Started

### Installation

Add the package directly to your Unity project via manifest.json:

```json
"dependencies": {
  "com.gametools.unitysavesystem": "https://github.com/juanpablomaggi/GameTools-UnitySaveSystem.git#1.0.2"
}
```

Open Unity and let the package import automatically.

### Usage

1. Define a DTO class for the data you want to save

> [!IMPORTANT]
> Remember to make the class serializable.

```csharp
    [System.Serializable]
    public class SavedPlayerData
    {
        public int Health { get; set; } = 100;
        public string Name { get; set; } = "SavedPlayer";
    }
```

2. Implement ISaveable in your object, using the DTO class as type:

```csharp
    public class Player : ISaveable<SavedPlayerData>
    {
        public string SaveKey => "SavedPlayer";
        public string Name { get; set; }
        public int Health { get; set; }

        public SavedPlayerData CaptureState()
        {
            return new SavedPlayerData { Name = Name, Health = Health };
        }

        public void RestoreState(SavedPlayerData state)
        {
            Name = state.Name;
            Health = state.Health;
        }
    }
```

3. Initialize the SaveService and register your saveable objects. When you save the game, it will capture the current state of the object to save.

```csharp
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private Player player;
        private ISaveService saveService;

        private void Awake()
        {
            saveService = new SaveService();
            saveService.Init();
        }

        private void Start()
        {
            saveService.Register(player);

            player.Name = "Player1";
            saveService.SaveGame();
        }
    }
```

4. Load the game by calling LoadGame on the service:

```csharp
    public void LoadGame()
    {
        saveService.LoadGame();
    }
```

> [!TIP]
> If an ISaveable object is registered after loading, the system will immediately inject the previously saved data.

---

## Changelog

[Changelog for this project. Latest release 1.0.2](CHANGELOG.md)

## Future Plans

 - [ ] Create a IStorage interface to abstract the storage mechanism (Currently only Binary serialization).
 - [ ] Create a tool for save preferences management.
