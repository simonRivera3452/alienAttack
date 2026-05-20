module Game.types 


type MainMenuCommand =
| NewGame
| LoadGame
| Exit

type PauseCommand =
| Resume
| SaveGame
| ExitToMenu


// Engine Types 
type ProgramState =
| Running
| Finished

type SpriteState =
| Alive
| Hit

type Misil = {
    X: int
    Y: int
}
