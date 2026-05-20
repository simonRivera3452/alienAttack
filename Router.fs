module Game.Router
open System
open Game.Utils
open Game.types

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
                0
                12
                [| 
                NewGame, "New Game"
                LoadGame, "Load Game" 
                Exit, "Salir" 
                |] with 
            | NewGame -> ShowingGame
            | LoadGame -> ShowingGame
            | Exit -> Terminated
        | ShowingGame -> 
            Game.Engine.mostrar()
            ShowingMenu
        | Terminated ->
            Terminated

    if nextState <> Terminated then
        mainLoop nextState


let mostrar() =
    initialState
    |> mainLoop