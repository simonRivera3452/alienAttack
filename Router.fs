module Game.Router
open Game.types
open Game.States
open Game.Engine
type RouterState =
| ShowingMainMenu
| ShowingPauseMenu
| ShowingGame
| Terminated

let initialState = ShowingMainMenu
let rec mainLoop state estadoActualDelJuego recordTemporal =
    let nextState, nuevoEstadoJuego, nuevoRecord =
        match state with 
        | ShowingMainMenu -> 
            match Game.mainMenu.mostrarMainMenu 0 12 [| 
                NewGame, "New Game"
                LoadGame, "Load Game" 
                Exit, "Salir" 
            |] with 
            | NewGame -> 
                ShowingGame, estadoInicial, recordTemporal
            | LoadGame -> 
                let partidaCargada = { recordTemporal with ProgramState = Running; RedibujarPantalla = true }
                ShowingGame, partidaCargada, recordTemporal
            | Exit -> 
                Terminated, estadoActualDelJuego, recordTemporal

        | ShowingGame -> 
            let estadoAlSalir = mostrarGame estadoActualDelJuego
            ShowingPauseMenu, estadoAlSalir, recordTemporal

        | ShowingPauseMenu -> 
            match Game.PauseMenu.mostrarPauseMenu 
                0 
                12 
                [| 
                Resume, "Resume"
                SaveGame, "Save Game"
                ExitToMenu, "Exit" 
            |] with 
            | Resume -> 
                let partidaReanudada = { estadoActualDelJuego with ProgramState = Running; RedibujarPantalla = true }
                ShowingGame, partidaReanudada, recordTemporal
            | SaveGame -> 
                
                ShowingPauseMenu, estadoActualDelJuego, estadoActualDelJuego
            | ExitToMenu -> 
                
                ShowingMainMenu, estadoActualDelJuego, recordTemporal
                
        | Terminated -> 
            Terminated, estadoActualDelJuego, recordTemporal

    if nextState <> Terminated then
        mainLoop nextState nuevoEstadoJuego nuevoRecord

let mostrar() =
    mainLoop initialState estadoInicial estadoInicial
