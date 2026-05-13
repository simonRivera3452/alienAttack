module Game.Router
open Game.Utils
open Game.types
//
// La funcion de este modulo es decidir
// que se muestra en la pantalla
//

type RouterState =
| ShowingMenu
| ShowingGame
| Terminated

let initialState = ShowingMenu

let rec mainLoop state =
    let nextState = // 1. Guardamos el resultado del match en una variable
        match state with 
        | ShowingMenu -> 
            match Game.mainMenu.mostrar 
                10 
                5 
                [| 
                NewGame, "New Game"
                LoadGame, "Load Game" 
                Exit, "Salir" 
                |] with 
            | NewGame -> ShowingGame
            | LoadGame -> ShowingGame
            | Exit -> Terminated
        | ShowingGame -> 
            Game.Game.mostrar()
            ShowingMenu
        | Terminated ->
            Terminated

    if nextState <> Terminated then
        mainLoop nextState


let mostrar() =
    initialState
    |> mainLoop