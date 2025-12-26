# Instructions for Feather Quest
1. **Architecture:** Strict MVP. `/Core` project MUST NOT reference Godot APIs.
2. **Context:** Always check `ROADMAP.md` before creating new classes.
3. **Style:** Use C# Standard 2.1 features.
4. **Testing:** All logic in `/Core` must be verifiable via NUnit/XUnit tests.
5. **Forbidden:** Do not create .tscn files. Do not use `GD.Print` in `/Core`.