# Instructions for Feather Quest
1. **Architecture:** Strict MVP. `/Core` project MUST NOT reference Godot APIs.
2. **Context:** Always check `ROADMAP.md` before creating new classes.
3. **Style:** Use C# Standard 2.1 features.
4. **Testing:** All logic in `/Core` must be verifiable via NUnit/XUnit tests.
5. **Forbidden:** Do not use `GD.Print` in `/Core`.
6. **Unity Migration:** .tscn files are permitted for prototyping in GodotClient, but ALL game logic must remain in C# to facilitate future Unity 3D migration.
7. **Windows CLI:** When executing CLI commands on Windows, always run `Set-ExecutionPolicy -Scope Process -ExecutionPolicy Bypass` first.
8. **Commits:** All commit messages must follow Conventional Commit format.