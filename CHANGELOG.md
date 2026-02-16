# Changelog

## [1.0.2] - 2026-02-16

### Changed

- Refactored ISaveable to use generics as save data.
- Made SaveRegistry an internal class to avoid it's use outside of the package.
- Register of ISaveable is now done through a method on the SaveService class. If the game was already load, it will inject the saved state automatically.
- LoadGame returns a `boolean` to indicate if the load was successful or not.

### Added

- Specific methods on SaveService to delete saves.
- Unit tests for SaveSystem.
- New documentation.

---

## [1.0.1] - 2026-02-15

### Fixed

- Reimported .meta files so can be imported correctly.

---

## [1.0.0] - 2026-02-15

### Initial Release

- Save system initial release.