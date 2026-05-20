module Game.Router
open Game.types
open Game.States
open Game.Engine
open Game.letters
open Game.GenericMenu
type RouterState =
| ShowingMainMenu
| ShowingPauseMenu
| ShowingGameOverMenu
| ShowingGame
| Terminated



let generarPantallaGameOver (estadoFinal: Game.States.State) =
    // 1. Traemos las líneas de arte ASCII originales desde tus letras
    let logoOriginal = GameOverLetters() 
    
    // 2. Diseñamos las líneas de texto con las estadísticas bien ordenadas
    let estadisticas = [|
        ""
        "========================================="
        $"   PUNTUACIÓN TOTAL : {estadoFinal.Puntuacion} pts"
        $"   TIEMPO SOBREVIVIDO: {estadoFinal.Clock} segundos"
        $"   ENEMIGOS ELIMINADOS: {estadoFinal.EnemigosDerrotados}"
        "========================================="
        ""
    |]
    
    // 3. Juntamos el arte ASCII con las estadísticas en un solo bloque de texto
    Array.append logoOriginal estadisticas




let initialState = ShowingMainMenu
let rec mainLoop state estadoActualDelJuego recordTemporal =
    let nextState, nuevoEstadoJuego, nuevoRecord =
        match state with 
        | ShowingMainMenu -> 
            match ShowAMenu 0 16 [| 
                NewGame, "New Game"
                LoadGame, "Load Game" 
                Exit, "Salir" 
            |] (MainMenuName()) with
            | NewGame -> 
                ShowingGame, estadoInicial, recordTemporal
            | LoadGame -> 
                let partidaCargada = { recordTemporal with ProgramState = Running; RedibujarPantalla = true }
                ShowingGame, partidaCargada, recordTemporal
            | Exit -> 
                Terminated, estadoActualDelJuego, recordTemporal

        | ShowingGame -> 
            let estadoAlSalir = mostrarGame estadoActualDelJuego
            match estadoAlSalir.ProgramState with
            | GameOver ->
                ShowingGameOverMenu, estadoAlSalir, recordTemporal
            | _ ->
                ShowingPauseMenu, estadoAlSalir, recordTemporal
        | ShowingPauseMenu -> 
            match ShowAMenu 0 16 [| 
                Resume, "Resume"
                SaveGame, "Save Game"
                ExitToMenu, "Exit to Main Menu" 
            |] (Pause()) with
            | Resume -> 
                let partidaReanudada = { estadoActualDelJuego with ProgramState = Running; RedibujarPantalla = true }
                ShowingGame, partidaReanudada, recordTemporal
            | SaveGame -> 
                
                ShowingPauseMenu, estadoActualDelJuego, estadoActualDelJuego
            | ExitToMenu -> 
                ShowingMainMenu, estadoActualDelJuego, recordTemporal
                
        | ShowingGameOverMenu -> 
            let visualGameOver = generarPantallaGameOver estadoActualDelJuego
            match ShowAMenu 0 18 [| 
                Restart, "Restart"
                ExitToMainMenu, "Exit to Main Menu" 
            |] visualGameOver with 
            | Restart -> 
                ShowingGame, estadoInicial, recordTemporal
            | ExitToMainMenu -> 
                ShowingMainMenu, estadoInicial, recordTemporal
                
        | Terminated -> 
            Terminated, estadoActualDelJuego, recordTemporal

    if nextState <> Terminated then
        mainLoop nextState nuevoEstadoJuego nuevoRecord

let mostrar() =
    mainLoop initialState estadoInicial estadoInicial
