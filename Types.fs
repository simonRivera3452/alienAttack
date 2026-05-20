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
| GameOver

type SpriteState =
| Alive
| Hit


type SpecialAbility = 
| Charging
| Ready

type SpecialAbilityCommand =
| Activate
| None

// Menu Types 

type MenuState =
| Active
| Terminated


// Game over menu types

type GameOverMenuCommand =
    | Restart
    | ExitToMainMenu