# GoGarbage Unity Project

## Overview

GoGarbage is a Unity project aimed at managing and visualizing garbage collection tasks. The project includes features such as user authentication, XP management, reward systems, and map-based marker placement for garbage reports.

## Features

- User authentication and key validation
- XP management and server synchronization
- Reward system with coin management
- Map-based marker placement for garbage reports
- Periodic map updates based on real-time location

## Packages Used

The project utilizes several Unity packages to enhance its functionality. Below is a list of the packages used:

- **com.unity.2d.sprite**: 1.0.0
- **com.unity.ai.navigation**: 2.0.0
- **com.unity.collab-proxy**: 2.6.0
- **com.unity.ide.rider**: 3.0.27
- **com.unity.ide.visualstudio**: 2.0.22
- **com.unity.render-pipelines.universal**: 16.0.5
- **com.unity.test-framework**: 1.3.9
- **com.unity.timeline**: 1.8.6
- **com.unity.ugui**: 2.0.0
- **com.unity.visualscripting**: 1.8.0
- **com.unity.modules.accessibility**: 1.0.0
- **com.unity.modules.ai**: 1.0.0
- **com.unity.modules.androidjni**: 1.0.0
- **com.unity.modules.animation**: 1.0.0
- **com.unity.modules.assetbundle**: 1.0.0
- **com.unity.modules.audio**: 1.0.0
- **com.unity.modules.cloth**: 1.0.0
- **com.unity.modules.director**: 1.0.0
- **com.unity.modules.imageconversion**: 1.0.0
- **com.unity.modules.imgui**: 1.0.0
- **com.unity.modules.jsonserialize**: 1.0.0
- **com.unity.modules.particlesystem**: 1.0.0
- **com.unity.modules.physics**: 1.0.0
- **com.unity.modules.physics2d**: 1.0.0
- **com.unity.modules.screencapture**: 1.0.0
- **com.unity.modules.terrain**: 1.0.0
- **com.unity.modules.terrainphysics**: 1.0.0
- **com.unity.modules.tilemap**: 1.0.0
- **com.unity.modules.ui**: 1.0.0
- **com.unity.modules.uielements**: 1.0.0
- **com.unity.modules.umbra**: 1.0.0
- **com.unity.modules.unityanalytics**: 1.0.0
- **com.unity.modules.unitywebrequest**: 1.0.0
- **com.unity.modules.unitywebrequestassetbundle**: 1.0.0
- **com.unity.modules.unitywebrequestaudio**: 1.0.0
- **com.unity.modules.unitywebrequesttexture**: 1.0.0
- **com.unity.modules.unitywebrequestwww**: 1.0.0
- **com.unity.modules.vehicles**: 1.0.0
- **com.unity.modules.video**: 1.0.0
- **com.unity.modules.vr**: 1.0.0
- **com.unity.modules.wind**: 1.0.0
- **com.unity.modules.xr**: 1.0.0

## Getting Started

1. Clone the repository to your local machine:
    ```sh
    git clone https://github.com/VynavinV/GoGarbage_server.git
    cd GoGarbage_Unity
    ```
2. Open the project in Unity:
    - Launch Unity Hub.
    - Click on the "Add" button.
    - Navigate to the cloned repository folder and select it.
    - Click on the "Open" button.
3. Ensure all required packages are installed via the Package Manager.
4. Run the project to start using the GoGarbage application.

## Flask Server and API for GoGarbage

The GoGarbage Unity project connects to a Flask server for backend services. The Flask server handles user authentication, image capture and upload, leaderboard management, and rewards.

### Flask Server Repository

You can find the Flask server repository [here](https://github.com/VynavinV/GoGarbage_server).

### Brief Setup Instructions

1. Clone the Flask server repository:
    ```sh
    git clone https://github.com/VynavinV/GoGarbage_server.git
    cd GoGarbage-Flask-Server
    ```
2. Create a Python virtual environment:
    ```sh
    python3 -m venv env
    source env/bin/activate  # On Windows use `env\Scripts\activate`
    ```
3. Install dependencies:
    ```sh
    pip install -r requirements.txt
    ```
4. Set up environment variables:
    Create a `.env` file in the root directory and add the necessary variables (e.g., `SECRET_KEY`, `SUPABASE_URL`, etc.).
5. Run the Flask server:
    ```sh
    python main.py
    ```

### Endpoints

The Flask server provides various API endpoints for authentication, image capture, leaderboard management, rewards, and more. Refer to the Flask server repository for detailed documentation on the available endpoints.
