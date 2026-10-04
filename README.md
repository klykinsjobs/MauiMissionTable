# Maui Mission Table

Maui Mission Table is a .NET MAUI application built around an event‑driven idle/progression gameplay loop. It combines mission management, unit progression, pack‑based rewards, and a lightweight in‑game economy, all implemented with a clean MVVM architecture and dependency injection. The core systems are modeled as independent domain services with clear responsibilities, making the codebase easy to extend, test, and maintain.

Game state is persisted using JSON snapshots mapped through dedicated mappers, ensuring deterministic serialization and version‑friendly data structures. A centralized event bus coordinates UI updates and domain events, while a tick‑based game loop drives time‑dependent mechanics such as mission progress, passive gold generation, and daily reward checks. The application uses observable models, asynchronous operations, and command‑driven view models to keep the UI responsive across platforms.

The project includes a comprehensive test suite with 140+ tests and a GitHub Actions workflow to automatically try building and running tests.

## Requirements
- .NET 10
- .NET MAUI
- CommunityToolkit.MVVM
- CommunityToolkit.Maui

## Getting started
- Clone the repository
- Press F5 in Visual Studio to build and start the application

### Tests
- Click View and then click Test Explorer
- Click Run All Tests In View

## License
Maui Mission Table is licensed under the MIT license. See the LICENSE file for details.

[![Build and Test](https://github.com/klykinsjobs/MauiMissionTable/actions/workflows/build-and-test.yml/badge.svg)](https://github.com/klykinsjobs/MauiMissionTable/actions/workflows/build-and-test.yml)
