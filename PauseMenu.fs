module Game.PauseMenu

open System
open Game.letters
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
    let logo = Pause()

    // 1. Buscamos la línea más larga del logo de pausa eliminando espacios invisibles (.TrimEnd())
    let logoWidth = 
        if logo.Length > 0 then 
            logo 
            |> Array.map (fun line -> line.TrimEnd().Length) 
            |> Array.max 
        else 
            1
            
    let logoX = max 0 ((Console.BufferWidth / 2) - (logoWidth / 2))

    logo |> Array.iteri (fun i line ->
        // Lo dibujamos fijo en la parte superior (línea 2) y limpio de espacios fantasmas
        displayMessage logoX (2 + i) ConsoleColor.Green (line.TrimEnd())
    )

    // 2. Buscamos dinámicamente la opción más larga ("Resume", "Save Game", etc.)
    let maxOptionWidth = 
        state.Commands 
        |> Array.map (fun (_, legend) -> legend.Length) 
        |> Array.max
        
    let optionsX = (Console.BufferWidth / 2) - (maxOptionWidth / 2)

    state.Commands
    |> Array.iteri (fun i (_, legend) ->
        displayMessage optionsX (state.Y + i) ConsoleColor.Cyan legend
    )

    // 3. El cursor alineado perfectamente con el mismo estilo "=>" del menú principal
    displayMessage (optionsX - 4) (state.Y + state.CurSorSelection) ConsoleColor.Yellow ">"

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

let mostrarPauseMenu x y commands =
    let oldForeground = Console.ForegroundColor
    Console.CursorVisible <- false

    let state =
        initialState x y commands
        |> myLoop
        
    Console.CursorVisible <- true
    Console.ForegroundColor <- oldForeground
    Console.Clear()
    fst state.Commands[state.CurSorSelection]