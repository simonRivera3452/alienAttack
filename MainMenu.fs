module Game.mainMenu 

open System

//
// Esta linea es para traer los simbolos
// del module App.Utils
//
open Game.Utils

type MenuState =
| Active
| Terminated


type State<'C> = {
    MenuState: MenuState
    X: int
    Y: int
    CurSorSelection: int
    CursorX: int
    Commands: ('C * string) array
    RedrawScreen: bool
}


let initialState x y commands = 
    {
        MenuState = Active
        X = x
        Y = y
        CurSorSelection = 0
        CursorX = x-2
        Commands = commands
        RedrawScreen = true
    }
let drawMenu state =
    let logo = [|
        "  ___  _      _____  _____  _   _      ___  _____ _____  ___  _____  _   __"
        " / _ \| |    |_   _||  ___|| \ | |    / _ \|_   _|_   _|/ _ \/  __ \| | / /"
        "/ /_\ \ |      | |  | |__  |  \| |   / /_\ \ | |   | | / /_\ \ |  \/| |/ / "
        "|  _  | |      | |  |  __| | . ` |   |  _  | | |   | | |  _  | | |  |    \ "
        "| | | | |____ _| |_ | |___ | |\  |   | | | | | |   | | | | | | \__/\ | |\  \\"
        "\_| |_/\_____/\___/ \____/ \_| \_/   \_| |_/ \_/   \_/ \_| |_/\____/\_| \_/"
    |]

    // 1. Usamos una longitud fija para el logo (la parte más ancha es ~74)
    let logoWidth = 74
    let logoX = (Console.BufferWidth / 2) - (logoWidth / 2)

    logo |> Array.iteri (fun i line ->
        displayMessage logoX (state.Y - 8 + i) ConsoleColor.Green line
    )

    // 2. Para las opciones, buscamos la más larga ("Load Game" o "New Game") 
    // para que el centro sea el mismo para todas
    let maxOptionWidth = 10 // "Load Game" tiene 9-10 caracteres
    let optionsX = (Console.BufferWidth / 2) - (maxOptionWidth / 2)

    state.Commands
    |> Array.iteri (fun i (_, legend) ->
        displayMessage optionsX (state.Y + i) ConsoleColor.Cyan legend
    )

    // 3. El cursor ahora siempre estará en la misma X
    displayMessage (optionsX - 2) (state.Y + state.CurSorSelection) ConsoleColor.Yellow ">"

let updateMenuKeyboard (keyInfo: ConsoleKeyInfo) state =
    let key = keyInfo.Key
    let newState =
        match key with 
        | ConsoleKey.UpArrow -> {state with CurSorSelection = max 0 (state.CurSorSelection-1)}
        | ConsoleKey.DownArrow -> {state with CurSorSelection = min (state.Commands.Length-1) (state.CurSorSelection+1)}
        | ConsoleKey.Enter -> {state with MenuState = Terminated}
        | _ -> state

    if newState <> state then 
        {newState with RedrawScreen = true}
    else
        state

// Loop 

let myLoop state = 
    createMainLoop 
        [||]
        (fun s -> s.MenuState = Active) 
        [|updateMenuKeyboard|]
        [| drawMenu|]
        (fun s -> s.RedrawScreen)
        (fun s -> {s with RedrawScreen=false})
        state


let mostrar x y commands =
    let oldForeground = Console.ForegroundColor
    Console.CursorVisible <- false

    let state =
        initialState x y commands
        |> myLoop
        
    Console.CursorVisible <- true
    Console.ForegroundColor <- oldForeground
    Console.Clear()
    fst state.Commands[state.CurSorSelection]