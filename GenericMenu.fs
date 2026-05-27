module Game.GenericMenu


open System
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
    Logo: string array 
}

let initialState x y commands logo = 
    {
        MenuState = Active
        X = x
        Y = y
        CurSorSelection = 0
        CursorX = x - 2
        Commands = commands
        RedrawScreen = true
        Logo = logo
    }

let drawMenu state =

    let logoWidth = 
        if state.Logo.Length > 0 then 
            state.Logo |> Array.map (fun line -> line.TrimEnd().Length) |> Array.max 
        else 
            1
            
    let logoX = max 0 ((Console.BufferWidth / 2) - (logoWidth / 2))

    state.Logo |> Array.iteri (fun i line ->
        displayMessage logoX (2 + i) ConsoleColor.Green (line.TrimEnd())
    )

    // 2. Centramos las opciones dinámicas
    let maxOptionWidth = 
        state.Commands 
        |> Array.map (fun (_, legend) -> legend.Length) 
        |> Array.max
        
    let optionsX = (Console.BufferWidth / 2) - (maxOptionWidth / 2)

    state.Commands
    |> Array.iteri (fun i (_, legend) ->
        displayMessage optionsX (state.Y + i) ConsoleColor.Cyan legend
    )

    displayMessage (optionsX - 4) (state.Y + state.CurSorSelection) ConsoleColor.Yellow ">"

let updateMenuKeyboard (keyInfo: ConsoleKeyInfo) state =
    let key = keyInfo.Key
    let newState =
        match key with 
        | ConsoleKey.UpArrow -> {state with CurSorSelection = max 0 (state.CurSorSelection - 1)}
        | ConsoleKey.DownArrow -> {state with CurSorSelection = min (state.Commands.Length - 1) (state.CurSorSelection + 1)}
        | ConsoleKey.Enter -> {state with MenuState = Terminated}
        | _ -> state

    if newState <> state then 
        {newState with RedrawScreen = true}
    else
        state

let myLoop state = 
    createMainLoop 
        [||]
        (fun s -> s.MenuState = Active) 
        [|updateMenuKeyboard|]
        [| drawMenu|]
        (fun s -> s.RedrawScreen)
        (fun s -> {s with RedrawScreen = false})
        state

let ShowAMenu x y commands logo =
    let oldForeground = Console.ForegroundColor
    Console.CursorVisible <- false

    let state =
        initialState x y commands logo
        |> myLoop
        
    Console.CursorVisible <- true
    Console.ForegroundColor <- oldForeground
    Console.Clear()
    fst state.Commands[state.CurSorSelection]