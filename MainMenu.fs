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
    state.Commands
    |> Array.iteri (fun i (_,legend) ->
        displayMessage state.X (state.Y+i) ConsoleColor.Cyan legend
    )

    displayMessage state.CursorX (state.Y+state.CurSorSelection) ConsoleColor.Yellow "*"


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