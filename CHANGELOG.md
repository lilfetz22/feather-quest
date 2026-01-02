# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

### Added
- Initial project structure
- MVP architecture with Core, GodotClient, and Tests projects
- Semantic versioning setup
- [GODOT-001] Biome scene with parallax background
  - BiomeCamera.cs script with mouse and touch input handling
  - Biome.tscn scene with 4 parallax layers (Sky, Distant Trees, Midground, Foreground)
  - Placeholder ColorRect assets for each layer
  - Camera bounds clamping (configurable MinX/MaxX)
  - Comprehensive documentation in Scenes/README.md
  - Directory structure: Scenes/, Scripts/, Assets/

## [0.0.1] - 2026-01-01

### Added
- Project initialization
- Basic solution structure with three projects (Core, GodotClient, Tests)
- ROADMAP.md with technical architecture
- Copilot instructions for development guidelines
