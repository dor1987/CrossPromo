# <Title> CrossPromo

Author: Dor Ashush

## Introduction
 Creating a GameObject in unity which will enable us to promote our games across other games.

### Rationale
To export a CrossPromo.unitypackage file that contains a CrossPromo Prefab.
The CrossPromo should autoplay videos in a loop.
This CrossPromo Prefab will be added into the game, to an existing scene (for example the “Start Game” scene) with minimal configuration (plug&play).
The videos will be played in a loop and users will be able to click on a video in order to install a game from the relevant store.


### Non-Goals

- Not supporting offline state.
- Not supporting network changes states.
- Not tasted on Android device.

## Proposed Design

Based on android MVVM with adjustments for Unity.

### System Architecture

VideoDisplayManager - Incharge of User inputs and displaying videos.
Controller - Provides a connection layer between the repository and the VideoDisplayManager.
Repository - Provides a safe async way to make Api calls and local data caching.

### Data Model

VideoDataModel - Acts as a boundary object.
AdVideoClip - The usable object after mapping.

## How to install

1. Open your unity project
2. Import the CrossPromo prefab to your project
3. Drag the prefab to your scene under a Canvas
4. Drag the your wanted location

- You can provide user ID to each instance of CrossPromo by clicking on it under the Hierachy tree and write it inside the "Controller" script at Play ID field.
- For Resume/Pause/Next/Prev features you can enable the DebugPanel object Inside the prefab.

## Edge cases

- No network - the prefab wouldnt display anything

## Additional features

- All videos will be saved locally for future use.
- When a video is not longer returned at the api call he will be deleted from cache.
- Debug control panel example for Resume/Pause/Next/Prev

## Assumptions

- Videos lists wouldnt be longer than 10(If they would i would implemented a que cache)
- User will have active network connection.
